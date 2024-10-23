using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class CategoryDao : BaseDao<Category>
    {
        private static CategoryDao instance = null;
        private static readonly object instacelock = new object();

        private CategoryDao()
        {

        }

        public static CategoryDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Categories
                    .Include(c => c.Machines)
                    .ToListAsync();
            }
        }

        public async Task<bool> IsCategoryNameExist(string categoryName)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Categories.AnyAsync(c => c.CategoryName!.ToLower().Equals(categoryName.ToLower()));
            }
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            }
        }
    }
}
