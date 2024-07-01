
using backend.Dto.Product;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController(IProduct productService) : ControllerBase
    {
        private readonly IProduct _productService = productService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(){
            var products = await _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        [HttpGet("get-by-url/{url}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUrl([FromRoute] string url){
            var product = await _productService.GetByUrl(url);
            return Ok(product);
        }

        [HttpGet("get-all-product-by-store/{storeUrl}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByStore([FromRoute] string storeUrl)
        {
            var response = await _productService.GetAllByStore(storeUrl);
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest createProductRequest){
             var product =  await _productService.Create(createProductRequest);
             return Ok(product);
        }

        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest updateProductRequest,
            [FromRoute] int id)
        {
            var product = await _productService.Update(updateProductRequest, id);
            return Ok(product);
        }
        [HttpPut("edit-price/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdatePrice updatePrice, [FromRoute] int id)
        {
            var product = await _productService.UpdatePriceAndSale(updatePrice, id);
            return Ok(product);
        }
        [HttpPut("edit-quantity/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateQuantity updateQuantity, [FromRoute] int id)
        {
            var product = await _productService.UpdateQuantity(updateQuantity, id);
            return Ok(product);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _productService.Delete(id);
            return Ok(response);
        }

        [HttpPut("{id:int}/change-status")]
        public async Task<IActionResult> Change([FromBody] IsChangeStatus isChangeStatus, [FromRoute] int id)
        {
            var response = await _productService.ChangeStatus(isChangeStatus, id);
            return Ok(response);
        }
    }
}