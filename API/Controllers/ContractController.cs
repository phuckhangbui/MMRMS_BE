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
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContracts()
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
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContractsForCustomer()
        {
            try
            {
                int customerId = GetLoginAccountId();
                var contracts = await _contractService.GetContractsForCustomer(customerId);
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

        //[HttpPost]
        //[Authorize(Policy = "Manager")]
        //public async Task<ActionResult> CreateContract([FromBody] ContractRequestDto contractRequestDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }

        //    try
        //    {
        //        int managerId = GetLoginAccountId();
        //        var contractId = await _contractService.CreateContract(managerId, contractRequestDto);
        //        return Created("", new { contract = contractId });
        //    }
        //    catch (ServiceException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
