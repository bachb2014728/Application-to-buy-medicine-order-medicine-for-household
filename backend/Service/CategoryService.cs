
using backend.Data;
using backend.Dto;
using backend.Dto.Category;
using backend.Error;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class CategoryService(ApplicationDbContext dBContext) : ICategory
    {
        private readonly ApplicationDbContext _context = dBContext;

        public async Task<ApiObject> DeleteOne(int id)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Không tìm thấy dữ liệu.");
            
            await DeleteChildCategories(id);
            
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
            
            return new ApiObject{
                Message = "Xóa dữ liệu thành công"
            };
        }

        private async Task DeleteChildCategories(int parentId)
        {
            var childCategories = await _context.Categories.Where(x => x.CategoryParent == parentId).ToListAsync();
            
            foreach(Category child in childCategories)
            {
                await DeleteChildCategories(child.Id);
                _context.Categories.Remove(child);
            }
        }


        public async Task<ApiObject> DeleteAll()
        {
            var list =  await _context.Categories.ToListAsync();
            foreach(var item in list){
                _context.Categories.Remove(item);
            }
            await _context.SaveChangesAsync();
            return new ApiObject{
                Message = "Xóa dữ liệu thành công"
            };
        }

        public async Task<List<Category>> GetAll()
        {
            var listItem = await _context.Categories.ToListAsync();
            return listItem;
        }

        public async Task<CategoryDto> GetOne(int id)
        {
            var categoryItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Không tim thấy dữ liệu.");
            return categoryItem.ToCategoryDto();
        }

        public async Task<CategoryDto> Put(int id, CategoryUpdate category)
        {
            var categoryItem = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Không tim thấy dữ liệu.");
            categoryItem.Name = category.Name;
            categoryItem.URL = category.URL;
            categoryItem.IsEnable = category.IsEnable;
            categoryItem.CategoryParent = category.CategoryParent;
            await _context.SaveChangesAsync();
            return categoryItem.ToCategoryDto();
        }

        public async Task<CategoryDto> Save(CategoryCreate category)
        {
            var categoryItem = await _context.Categories.FirstOrDefaultAsync(x => x.Name == category.Name);
            var categoryUrl = await _context.Categories.FirstOrDefaultAsync(x => x.URL == category.URL);
            if(categoryItem != null){
                throw new AlreadyExistsException($"{category.Name} đã tồn tại.");
            }
            if(categoryUrl != null){
                throw new AlreadyExistsException($"{category.URL} đã tồn tại.");
            }
            
            var item = new Category {
                Name = category.Name,
                URL = category.URL,
                IsEnable = category.IsEnable,
            };
            await _context.Categories.AddAsync(item);
            var parent = category.CategoryParent ?? item.Id;
            item.CategoryParent = parent;
            await _context.SaveChangesAsync();
            return item.ToCategoryDto();
        }

        public async Task<CategoryDto> GetOneByURL(string url)
        {
            var categoryItem = await _context.Categories.FirstOrDefaultAsync(x => x.URL == url) ?? throw new NotFoundException("Không tim thấy dữ liệu.");
            return categoryItem.ToCategoryDto();
        }
    }
}