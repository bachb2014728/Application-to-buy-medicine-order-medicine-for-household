using backend.Dto.Image;
using backend.Dto.Store;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/store")]
    [Authorize]
    public class StoreController(IStore service) : ControllerBase
    {
        private readonly IStore _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _service.GetAll();
            return Ok(stores.Select(ToStoreDto));
        }
        [HttpGet]
        [Route("my-store")]
        public async Task<IActionResult> GetMyStores(){
            var stores = await _service.GetAllByUser();
            return Ok(stores.Select(ToStoreDto));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var store = await _service.GetOne(id);
        
            return Ok(ToStoreDto(store));
        }
        [HttpGet]
        [Route("URL/{url}")]
        public async Task<IActionResult> GetUrl([FromRoute] string url){
            var response = await _service.GetOneByURL(url);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StoreCreate store)
        {
            var response = await _service.Post(store);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] StoreUpdate store)
        {
            var response = await _service.Update(store,id);
            return Ok(response);
        }
        [HttpPut("URL/{url}/info")]
        public async Task<IActionResult> UpdateByUrl([FromRoute] string url,[FromBody] StoreUpdate store)
        {
            var response = await _service.UpdateInfo(store,url);
            return Ok(response);
        }
        [HttpPut("URL/{url}/avatar")]
        public async Task<IActionResult> UpdateAvatar([FromRoute] string url,[FromBody] AddImage image)
        {
            var imageBytes = await _service.UpdateAvatar(image,url);
            return NoContent();
        }
        
        [HttpPut("URL/{url}/background")]
        public async Task<IActionResult> UpdateBackground([FromRoute] string url,[FromBody] AddImage image)
        {
            var response = await _service.UpdateBackground(image,url);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteOne(id);
            return Ok(response);
        }

        private static StoreDto ToStoreDto(Store store){
            return new StoreDto{
                Id = store.Id,
                Name = store.Name,
                URL = store.URL,
                Avatar = store.Avatar?.Id,
                Background = store.Background?.Id,
                Address = store.Address,
                Info = store.Info,
                Followers = [],
                Status = store.Status,
                Star =store.Star,
                CreatedAt = store.CreatedAt,
            };
        }
    }
}
