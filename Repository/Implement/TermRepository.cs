using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Term;
using Repository.Interface;

namespace Repository.Implement
{
    public class TermRepository : ITermRepository
    {
        private readonly IMapper _mapper;

        public TermRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateTerm(CreateTermDto createTermDto)
        {
            var term = _mapper.Map<Term>(createTermDto);

            await TermDao.Instance.CreateAsync(term);
        }

        public async Task DeleteTerm(int termId)
        {
            var term = await TermDao.Instance.GetTerm(termId);

            await TermDao.Instance.RemoveAsync(term);
        }

        public async Task<TermDto> GetTermById(int termId)
        {
            var term = await TermDao.Instance.GetTerm(termId);

            return _mapper.Map<TermDto>(term);
        }

        public async Task<IEnumerable<TermDto>> GetTerms()
        {
            var list = await TermDao.Instance.GetAllAsync();

            return _mapper.Map<IEnumerable<TermDto>>(list);
        }

        public async Task UpdateTerm(UpdateTermDto updateTermDto)
        {
            var term = _mapper.Map<Term>(updateTermDto);

            await TermDao.Instance.UpdateAsync(term);
        }
    }
}
