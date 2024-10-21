﻿using DTOs.Product;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetTop8LatestProducts()
        {
            try
            {
                var products = await _productService.GetTop8LatestProductList();
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

        [HttpGet("review/{productIds}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsReview([FromRoute] string productIds)
        {
            try
            {
                if (string.IsNullOrEmpty(productIds))
                {
                    return BadRequest();
                }

                var productIdList = productIds.Split(',').Select(int.Parse).ToList();
                var products = await _productService.GetProductReviews(productIdList);
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
        [Authorize(policy: "WebsiteStaff")]
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
        [Authorize(policy: "WebsiteStaff")]
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

        [HttpPatch("{productId}/toggle-lock")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateProductStatus([FromRoute] int productId)
        {

            try
            {
                await _productService.ToggleLockStatus(productId);
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

        [HttpPut("{productId}/attribute/update")]
        [Authorize(policy: "WebsiteStaff")]
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

        [HttpPut("{productId}/term/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateProductTerm([FromRoute] int productId, [FromBody] IEnumerable<CreateProductTermDto> productTermDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateProductTerm(productId, productTermDtos);
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

        [HttpPut("{productId}/component/update")]
        [Authorize(policy: "WebsiteStaff")]
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

        [HttpPut("{productId}/detail/update")]
        [Authorize(policy: "WebsiteStaff")]
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
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> ChangeProductImages(int productId, [FromBody] List<ImageList> imageList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _productService.ChangeProductImages(productId, imageList);
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
