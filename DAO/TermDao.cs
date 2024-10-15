using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class TermDao : BaseDao<Term>
    {
        private static TermDao instance = null;
        private static readonly object instacelock = new object();

        private TermDao()
        {

        }

        public static TermDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TermDao();
                }
                return instance;
            }
        }

        public async Task<Term> GetTerm(int termId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Terms.FirstOrDefaultAsync(t => t.TermId == termId);
            }
        }

        public async Task<IEnumerable<Term>> GetTermsByTermType(TermTypeEnum termType)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Terms
                    .Where(t => t.Type.Equals(termType.ToString()))
                    .ToListAsync();
            }
        }
    }
}
