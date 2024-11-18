using Common;
using DTOs.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/contracts")]
    public class ContractController : BaseApiController
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        [Authorize(policy: "Manager")]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContracts([FromQuery] string? status = null)
        {
            try
            {
                var contracts = await _contractService.GetContracts(status);
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
        public async Task<ActionResult<ContractDetailDto>> GetContractDetailById(string contractId)
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

        [HttpGet("customers")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContractsForCustomer([FromQuery] string? status = null)
        {
            try
            {
                int customerId = GetLoginAccountId();
                var contracts = await _contractService.GetContractsForCustomer(customerId, status);
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

        [HttpPost("{contractId}/end-contract")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult> EndContract(string contractId)
        {
            try
            {
                var result = await _contractService.EndContract(contractId);
                if (result) return Ok(MessageConstant.Contract.EndContractSuccessfully);
                return BadRequest(MessageConstant.Contract.EndContractFail);
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

        [HttpPost("{contractId}/extend-contract")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult> ExtendContract(string contractId, [FromBody] ContractExtendDto contractExtendDto)
        {
            try
            {
                var result = await _contractService.ExtendContract(contractId, contractExtendDto);
                if (result) return Ok(MessageConstant.Contract.ExtendContractSuccessfully);
                return BadRequest(MessageConstant.Contract.ExtendContractFail);
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
