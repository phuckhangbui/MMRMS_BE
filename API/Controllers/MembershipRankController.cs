using DTOs.MembershipRank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/membershipRanks")]
    public class MembershipRankController : BaseApiController
    {
        private readonly IMembershipRankService _membershipRankService;

        public MembershipRankController(IMembershipRankService membershipRankService)
        {
            _membershipRankService = membershipRankService;
        }

        [HttpGet]
        public async Task<ActionResult> GetMembershipRanks()
        {
            try
            {
                var membershipRanks = await _membershipRankService.GetMembershipRanks();
                return Ok(membershipRanks);
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

        [HttpGet("{membershipRankId}")]
        public async Task<ActionResult<MembershipRankDto>> GetMembershipRankById(int membershipRankId)
        {
            try
            {
                var membershipRank = await _membershipRankService.GetMembershipRankById(membershipRankId);
                return Ok(membershipRank);
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

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> CreateMembershipRank([FromBody] MembershipRankRequestDto membershipRankRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _membershipRankService.CreateMembershipRank(membershipRankRequestDto);
                return Created("", membershipRankRequestDto);
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

        [HttpPut("{membershipRankId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> UpdateMembershipRank(int membershipRankId, [FromBody] MembershipRankRequestDto membershipRankRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _membershipRankService.UpdateMembershipRank(membershipRankId, membershipRankRequestDto);
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

        [HttpDelete("{membershipRankId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> DeleteMembershipRank(int membershipRankId)
        {
            try
            {
                await _membershipRankService.DeleteMembershipRank(membershipRankId);
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

        [HttpPatch("{membershipRankId}/status")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ChangeMembershipRankStatus(int membershipRankId, [FromQuery, BindRequired] string status)
        {
            try
            {
                await _membershipRankService.ChangeMembershipRankStatus(membershipRankId, status);
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

        [HttpGet("customer")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<MembershipRankDto>> GetMembershipRankForCustomer()
        {
            try
            {
                int customerId = GetLoginAccountId();
                var membershipRank = await _membershipRankService.GetMembershipRankForCustomer(customerId);
                return Ok(membershipRank);
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
