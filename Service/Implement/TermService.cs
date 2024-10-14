using Common;
using Common.Enum;
using DTOs.Term;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class TermService : ITermService
    {
        private readonly ITermRepository _termRepository;

        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public async Task CreateTerm(CreateTermDto createTermDto)
        {
            if (string.IsNullOrEmpty(createTermDto.Type) || !Enum.TryParse(typeof(TermTypeEnum), createTermDto.Type, true, out _))
            {
                throw new ServiceException(MessageConstant.Term.TermTypeNotCorrect);
            }

            await _termRepository.CreateTerm(createTermDto);
        }

        public async Task DeleteTerm(int termId)
        {
            var term = await _termRepository.GetTermById(termId);

            if (term == null) { throw new ServiceException(MessageConstant.Term.TermNotFound); }

            await _termRepository.DeleteTerm(termId);
        }

        public async Task<IEnumerable<TermDto>> GetProductTerms()
        {
            var list = await _termRepository.GetTerms();

            return list.Where(t => t.Type.Equals(TermTypeEnum.Product.ToString())).ToList();
        }

        public async Task<TermDto> GetTerm(int termId)
        {
            return await _termRepository.GetTermById(termId);
        }

        public async Task<IEnumerable<TermDto>> GetTerms()
        {
            return await _termRepository.GetTerms();

        }

        public async Task UpdateTerm(UpdateTermDto updateTermDto)
        {
            if (string.IsNullOrEmpty(updateTermDto.Type) || !Enum.TryParse(typeof(TermTypeEnum), updateTermDto.Type, true, out _))
            {
                throw new ServiceException(MessageConstant.Term.TermTypeNotCorrect);
            }

            var term = await _termRepository.GetTermById(updateTermDto.TermId);

            if (term == null) { throw new ServiceException(MessageConstant.Term.TermNotFound); }

            await _termRepository.UpdateTerm(updateTermDto);
        }
    }
}
