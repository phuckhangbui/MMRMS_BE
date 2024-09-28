using DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/products")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductList();
                return Ok(products);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<DisplayProductDetailDto>> GetProductDetail([FromRoute] int productId)
        {
            try
            {
                var product = await _productService.GetProductDetailDto(productId);
                return Ok(product);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{productId}/serial-products")]
        public async Task<ActionResult<DisplayProductDetailDto>> GetSerialProductList([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                var product = await _productService.GetSerialProductList(productId);
                return Ok(product);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                var product = await _productService.CreateProduct(createProductDto);

                return StatusCode(201, new { productId = product.ProductId });
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{productId}/status")]
        public async Task<ActionResult> UpdateProductStatus([FromRoute] int productId, [FromQuery] string status)
        {

            try
            {
                await _productService.UpdateProductStatus(productId, status);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{productId}/attribute/update")]
        public async Task<ActionResult> UpdateProductAttribute([FromRoute] int productId, [FromBody] IEnumerable<CreateProductAttributeDto> productAttributeDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateProductAttribute(productId, productAttributeDtos);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{productId}/component/update")]
        public async Task<ActionResult> UpdateProductComponent([FromRoute] int productId, [FromBody] ComponentList componentList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateProductComponent(productId, componentList);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{productId}/detail/update")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductDto updateProductDto)
        {

            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _productService.UpdateProductDetail(productId, updateProductDto);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{productId}/thumbnail")]
        public async Task<ActionResult> ChangeContentImage(int productId, IFormFile imageUrl)
        {
            if (imageUrl == null || imageUrl.Length == 0)
            {
                return BadRequest("Chưa có hình ảnh nào được chọn");
            }

            try
            {
                await _productService.ChangeProductThumbnail(productId, imageUrl);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{productId}/images")]
        public async Task<ActionResult> ChangeContentImages(int productId, List<IFormFile> imageFiles)
        {
            if (imageFiles == null || !imageFiles.Any())
            {
                return BadRequest("Chưa có hình ảnh nào được chọn");
            }

            try
            {
                // Pass the list of image files to the service to handle the thumbnails change
                await _productService.ChangeProductImages(productId, imageFiles);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
