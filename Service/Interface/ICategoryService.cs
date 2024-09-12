using DTOs.Category;

namespace Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task CreateCategory(CategoryRequestDto categoryRequestDto);
        Task UpdateCategory(int categoryId, CategoryRequestDto categoryRequestDto);
    }
}
