
using backend.Dto.Category;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;
[ApiController]
[Route("api/v1/categories")]
public class CategoryController(ICategory categoryService) : ControllerBase
{
    private readonly ICategory _service = categoryService;
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(){
        var listItem = await _service.GetAll();
        return Ok(listItem.Select(item => item.ToCategoryDto()));
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CategoryCreate category){
        var item = await _service.Save(category);
        return Ok(item);
    }
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteAll(){
        var response = await _service.DeleteAll();
        return Ok(response);
    }
    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromRoute] int id){
        var response = await _service.GetOne(id);
        return Ok(response);
    }
    [HttpGet]
    [Route("URL/{url}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUrl([FromRoute] string url){
        var response = await _service.GetOneByURL(url);
        return Ok(response);
    }
    [HttpDelete]
    [Authorize]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id){
        var response = await _service.DeleteOne(id);
        return Ok(response);
    }
    [HttpPut]
    [Authorize]
    [Route("{id:int}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CategoryUpdate category){
        var response = await _service.Put(id,category);
        return Ok(response);
    }
        
}