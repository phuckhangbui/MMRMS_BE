using DTOs.Term;

namespace Repository.Interface
{
    public interface ITermRepository
    {
        Task CreateTerm(CreateTermDto createTermDto);
        Task DeleteTerm(int termId);
        Task<TermDto> GetTermById(int termId);
        Task<IEnumerable<TermDto>> GetTerms();
        Task UpdateTerm(UpdateTermDto updateTermDto);
    }
}
