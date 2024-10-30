using DTOs.Term;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/terms")]
    public class TermController : BaseApiController
    {
        private readonly ITermService _termService;

        public TermController(ITermService termService)
        {
            _termService = termService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TermDto>>> GetTerms()
        {
            try
            {
                var terms = await _termService.GetTerms();
                return Ok(terms);
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

        [HttpGet("{termId}")]
        public async Task<ActionResult<TermDto>> GetTerm([FromRoute] int termId)
        {
            try
            {
                var term = await _termService.GetTerm(termId);
                return Ok(term);
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

        [HttpGet("machine")]
        public async Task<ActionResult<IEnumerable<TermDto>>> GetMachineTerms()
        {
            try
            {
                var terms = await _termService.GetMachineTerms();
                return Ok(terms);
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
        public async Task<ActionResult> CreateTerm([FromBody] CreateTermDto createTermDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _termService.CreateTerm(createTermDto);
                return Created();
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


        [HttpPut]
        public async Task<ActionResult> UpdateTerm([FromBody] UpdateTermDto updateTermDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _termService.UpdateTerm(updateTermDto);
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

        [HttpDelete("{termId}")]
        public async Task<ActionResult> DeleteTerm([FromRoute] int termId)
        {
            try
            {
                await _termService.DeleteTerm(termId);
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
