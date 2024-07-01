using backend.Dto.Product.Use;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/uses")]
    public class UseController(IUse useService) : ControllerBase
    {
        private readonly IUse _useService = useService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUse(){
            var uses = await _useService.GetAllAsync();
            return Ok(uses);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUse([FromBody] UseCreate useCreate) {
            var use = await _useService.CreateAsync(useCreate);
            return Ok(use);
        }
        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUseById([FromRoute] int id){
            var use = await _useService.GetUseByIdAsync(id);
            return Ok(use);
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> PutUse([FromRoute] int id , [FromBody] UseUpdate useUpdate){
            var use = await _useService.UpdateUseByIdAsync(id,useUpdate);
            return Ok(use);
        }
        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveUseById([FromRoute] int id){
            var response = await _useService.RemoveUseByIdAsync(id);
            return Ok(response);
        }
    }
}