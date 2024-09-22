using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ContentDao : BaseDao<Content>
    {
        private static ContentDao instance = null;
        private static readonly object instacelock = new object();

        private ContentDao()
        {

        }

        public static ContentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContentDao();
                }
                return instance;
            }
        }

        public async Task<Content> GetContentById(int contentId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);
            }
        }

        public async Task<IEnumerable<Content>> GetAllInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using (var context = new MmrmsContext())
            {
                IQueryable<Content> query = context.Contents;

                if (startDate.HasValue)
                {
                    query = query.Where(c => c.DateCreate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(c => c.DateCreate <= endDate.Value);
                }

                return await query.ToListAsync();
            }
        }

    }
}
