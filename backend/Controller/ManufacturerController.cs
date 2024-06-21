using backend.Dto.Product.Manufacturer;
using backend.Interface;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/manufacturers")]
    public class ManufacturerController(IManufacturer manufacturerService) : ControllerBase
    {
        private readonly IManufacturer _manufacturerService = manufacturerService;

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var listItem = await _manufacturerService.GetAllAsync();
            return Ok(listItem);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ManufacturerCreate manufacturerCreate) {
            var item = await _manufacturerService.CreateAsync(manufacturerCreate);
            return Ok(item);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOneById([FromRoute] int id){
            var item = await _manufacturerService.GetOneByIdAsync(id);
            return Ok(item);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id , [FromBody] ManufacturerUpdate manufacturerUpdate){
            var item = await _manufacturerService.UpdateOneByIdAsync(id,manufacturerUpdate);
            return Ok(item);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveOneById([FromRoute] int id){
            var response = await _manufacturerService.RemoveOneByIdAsync(id);
            return Ok(response);
        }
    }
}