
using backend.Data;
using backend.Dto.Image;
using backend.Error;
using backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/images")]
    [AllowAnonymous]
    public class ImageController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> UploadImage( IFormFile imageData)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await imageData.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                var image = new Image
                {
                    Name = imageData.FileName,
                    Type = imageData.ContentType,
                    File =  imageBytes
                };
                await _context.Images.AddAsync(image);
                await _context.SaveChangesAsync();
                var response = new ImageDto
                {
                    Id = image.Id,
                    Name = image.Name,
                    Type = image.Type,
                    File = Convert.ToBase64String(image.File)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi upload ảnh: {ex.Message}");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var image = await _context.Images.FindAsync(id);
            if (image == null){
                return NotFound();
            }
            var base64Image = Convert.ToBase64String(image.File);
            var response = new ImageDto
            {
                Id = image.Id,
                Name = image.Name,
                Type = image.Type,
                File = base64Image
            };
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var image = await _context.Images.FirstOrDefaultAsync(x=>x.Id == id);
            if (image == null)
            {
                throw new NotFoundException("không tìm thấy hình ảnh");
            }
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return Ok("Xóa thành công.");
        }
    }
}