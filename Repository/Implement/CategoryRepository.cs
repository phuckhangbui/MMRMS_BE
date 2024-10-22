using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Category;
using Repository.Interface;

namespace Repository.Implement
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMapper _mapper;

        public CategoryRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateCategory(CategoryRequestDto categoryRequestDto)
        {
            var category = _mapper.Map<Category>(categoryRequestDto);

            //category.Status = CategoryEnum.OutOfStock.ToString();
            category.DateCreate = DateTime.Now;

            await CategoryDao.Instance.CreateAsync(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await CategoryDao.Instance.GetCategories();

            var categoryDtos = categories.Select(category => new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                DateCreate = category.DateCreate,
                //Status = category.Status,
                //Quantity = category.Products.Sum(p => p.Quantity) ?? 0
            }).ToList();

            return categoryDtos;
        }

        public async Task<CategoryDto?> GetCategoryById(int categoryId)
        {
            var category = await CategoryDao.Instance.GetCategoryById(categoryId);
            if (category != null)
            {
                return _mapper.Map<CategoryDto>(category);
            }

            return null;
        }

        public async Task<bool> IsCategoryNameExist(string categoryName)
        {
            return await CategoryDao.Instance.IsCategoryNameExist(categoryName);
        }

        public async Task UpdateCategory(int categoryId, CategoryRequestDto categoryRequestDto)
        {
            var category = await CategoryDao.Instance.GetCategoryById(categoryId);

            category.CategoryName = categoryRequestDto.CategoryName;

            await CategoryDao.Instance.UpdateAsync(category);
        }
    }
}
