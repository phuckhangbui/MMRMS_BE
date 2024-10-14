using BusinessObject;
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
    }
}
