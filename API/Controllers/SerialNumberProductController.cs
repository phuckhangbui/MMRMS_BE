using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/serial-number-product")]
    public class SerialNumberProductController : BaseApiController
    {
        //    [HttpPost]
        //    public async Task<IActionResult> CreateSerialNumberProduct([FromBody] CreateProductDto createProductDto)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //            return BadRequest(errorMessages);
        //        }

        //        try
        //        {
        //            var product = await _productService.CreateProduct(createProductDto);

        //            return StatusCode(201, new { productId = product.ProductId });
        //        }
        //        catch (ServiceException ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, ex.Message);
        //        }
        //    }
    }
}
