using DTOs.Category;

namespace Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task CreateCategory(CategoryRequestDto categoryRequestDto);
        Task<bool> IsCategoryNameExist(string categoryName);
        Task<CategoryDto?> GetCategoryById(int categoryId);
        Task UpdateCategory(int categoryId, CategoryRequestDto categoryRequestDto);
    }
}
