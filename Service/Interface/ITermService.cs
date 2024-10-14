
using DTOs.Term;

namespace Service.Interface
{
    public interface ITermService
    {
        Task CreateTerm(CreateTermDto createTermDto);
        Task DeleteTerm(int termId);
        Task<IEnumerable<TermDto>> GetProductTerms();
        Task<TermDto> GetTerm(int termId);
        Task<IEnumerable<TermDto>> GetTerms();
        Task UpdateTerm(UpdateTermDto updateTermDto);
    }
}
