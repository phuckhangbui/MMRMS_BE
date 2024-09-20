using Common;
using DTOs.Category;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServiceImpl(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateCategory(CategoryRequestDto categoryRequestDto)
        {
            await CheckCategoryExist(categoryRequestDto.CategoryName);

            await _categoryRepository.CreateCategory(categoryRequestDto);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task UpdateCategory(int categoryId, CategoryRequestDto categoryRequestDto)
        {
            var category = await _categoryRepository.GetCategoryById(categoryId);
            if (category == null)
            {
                throw new ServiceException(MessageConstant.Category.CategoryNotFound);
            }

            await CheckCategoryExist(categoryRequestDto.CategoryName);

            await _categoryRepository.UpdateCategory(categoryId, categoryRequestDto);
        }

        private async Task CheckCategoryExist(string categoryName)
        {
            var isCategoryNameExist = await _categoryRepository.IsCategoryNameExist(categoryName);
            if (isCategoryNameExist)
            {
                throw new ServiceException(MessageConstant.Category.CategoryExistsError);
            }
        }
    }
}
