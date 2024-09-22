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

        [HttpPatch("/{productId}/toggle-delete")]
        public async Task<ActionResult> ToggleProductDelete([FromRoute] int productId)
        {
            try
            {
                await _productService.ToggleProductIsDelete(productId);
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

        [HttpPatch("/{productId}/status")]
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
    }
}
