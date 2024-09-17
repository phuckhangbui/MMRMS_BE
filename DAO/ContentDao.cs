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
    }
}
