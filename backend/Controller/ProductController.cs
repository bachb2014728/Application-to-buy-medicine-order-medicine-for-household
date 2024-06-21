
using backend.Dto.Product;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController(IProduct productService) : ControllerBase
    {
        private readonly IProduct _productService = productService;

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var products = await _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        [HttpGet("get-by-url/{url}")]
        public async Task<IActionResult> GetByUrl([FromRoute] string url){
            var product = await _productService.GetByUrl(url);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest createProductRequest){
             var product =  await _productService.Create(createProductRequest);
             return Ok(product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest updateProductRequest,
            [FromRoute] int id)
        {
            var product = await _productService.Update(updateProductRequest, id);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _productService.Delete(id);
            return Ok(response);
        }
    }
}