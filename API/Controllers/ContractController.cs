using DTOs.Content;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        public async Task<ActionResult> GetContracts()
        {
            try
            {
                var contracts = await _contractService.GetContracts();
                return Ok(contracts);
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

        [HttpGet("{contractId}")]
        public async Task<ActionResult<ContentDto>> GetContentDetailById(string contractId)
        {
            try
            {
                var contract = await _contractService.GetContractDetailById(contractId);
                return Ok(contract);
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

        [HttpGet("customers/{accountId}")]
        public async Task<ActionResult> GetContractsForCustomer(int accountId)
        {
            try
            {
                var contracts = await _contractService.GetContractsForCustomer(accountId);
                return Ok(contracts);
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
