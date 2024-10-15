using DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/invoices")]

    public class InvoiceController : BaseApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetAll();
                return Ok(invoices);
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

        [HttpPost("{invoiceId}/pay")]
        public async Task<ActionResult<string>> GetPaymentUrl([FromRoute] string invoiceId, [FromBody] UrlDto urlDto)
        {
            //int customerId = GetLoginAccountId();
            //if (customerId == 0)
            //{
            //    return Unauthorized();
            //}

            int customerId = 0;

            try
            {
                string paymentUrl = await _invoiceService.GetPaymentUrl(customerId, invoiceId, urlDto);
                return Ok(paymentUrl);
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

        [HttpPost("{invoiceId}/post-transaction-check")]
        public async Task<ActionResult<string>> CheckPostTransaction([FromRoute] string invoiceId)
        {
            //int customerId = GetLoginAccountId();
            //if (customerId == 0)
            //{
            //    return Unauthorized();
            //}

            int customerId = 0;

            try
            {
                await _invoiceService.PostTransactionProcess(customerId, invoiceId);
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
