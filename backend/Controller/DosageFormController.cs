
using backend.Dto.Product.DosageForm;
using backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/v1/dosage-forms")]
    public class DosageFormController(IDosageForm dosageFormService) : ControllerBase
    {
        private readonly IDosageForm _dosageFormService = dosageFormService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(){
            var listItem = await _dosageFormService.GetAll();
            return Ok(listItem);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] DosageFormCreate dosageFormCreate) {
            var dosageForm = await _dosageFormService.Create(dosageFormCreate);
            return Ok(dosageForm);
        }
        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne([FromRoute] int id){
            var dosageForm = await _dosageFormService.GetOne(id);
            return Ok(dosageForm);
        }
        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id , [FromBody] DosageFormUpdate useUpdate){
            var dosageForm = await _dosageFormService.Update(id,useUpdate);
            return Ok(dosageForm);
        }
        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOne([FromRoute] int id){
            var response = await _dosageFormService.DeleteOne(id);
            return Ok(response);
        }
    }
}