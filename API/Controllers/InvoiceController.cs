using Common;
using DTOs;
using DTOs.Invoice;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("my-invoice")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetCustomerInvoice()
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                var list = await _invoiceService.GetCustomerInvoice(customerId);
                return Ok(list);
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
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<string>> GetPaymentUrl([FromRoute] string invoiceId, [FromBody] UrlDto urlDto)
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

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
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<string>> CheckPostTransaction([FromRoute] string invoiceId)
        {
            int customerId = GetLoginAccountId();

            try
            {
                var result = await _invoiceService.PostTransactionProcess(customerId, invoiceId);
                if (result) return Ok(MessageConstant.Invoice.PayInvoiceSuccessfully);
                return BadRequest(MessageConstant.Invoice.PayInvoiceFail);
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

        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<object>> GetInvoiceDetail(string invoiceId)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceDetail(invoiceId);
                return Ok(invoice);
            }
            catch (ServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
