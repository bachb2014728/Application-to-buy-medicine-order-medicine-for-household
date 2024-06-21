
using backend.Data;
using backend.Dto;
using backend.Dto.Category;
using backend.Dto.Product;
using backend.Dto.Product.Contraindication;
using backend.Dto.Product.DosageForm;
using backend.Dto.Product.Manufacturer;
using backend.Dto.Product.Use;
using backend.Dto.Store;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class ProductService(ApplicationDbContext context,ConvertInformation convert) : IProduct
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ConvertInformation _convert = convert;
        public async Task<IEnumerable<ProductItem>> GetAll() 
        {
            var listItem = await _context.Products
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Images)
                .ToListAsync();
            if (listItem.Count == 0) 
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            return listItem.Select(ToProductItem);
        }

        private static ProductItem ToProductItem(Product product)
        {
            var store = new StoreItem
            {
                Id = product.CreatedBy.Id,
                Name = product.CreatedBy.Name,
                URL = product.CreatedBy.URL,
                Avatar = null
            };
            return new ProductItem
            {
                Id = product.Id,
                Name = product.Name,
                ImageId = product.ImageIds?[0],
                URL = product.URL,
                Quantity = product.Quantity,
                Price = product.Price,
                Sale = product.Sale,
                Status = product.Status,
                CreatedBy = store
            };
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = await ProductExist(id);
            return await ToProductDto(product);
        }

        public async Task<ProductDto?> Update(UpdateProductRequest updateProductRequest, int id)
        {
            var product = await ProductExist(id);
            var nameHaveExist = await _context.Products.Where(x => x.Id != id)
                .FirstOrDefaultAsync(x => x.Name == updateProductRequest.Name);
            if (nameHaveExist != null)
            {
                throw new AlreadyExistsException($"{updateProductRequest.Name} đã tồn tại.");
            }

            var urlHaveExist = await _context.Products.Where(x => x.URL != product.URL)
                .FirstOrDefaultAsync(x=>x.URL == updateProductRequest.URL);
            if (urlHaveExist !=null)
            {
                throw new AlreadyExistsException($"{updateProductRequest.URL} đã tồn tại.");
            }

            product.Name = updateProductRequest.Name;
            product.URL = updateProductRequest.URL;
            product.Quantity = updateProductRequest.Quantity;
            product.Content = updateProductRequest.Content;
            product.Price = updateProductRequest.Price;
            product.Status = updateProductRequest.Status;
            product.Sale = updateProductRequest.Sale;
            
            await _context.SaveChangesAsync();
            await SaveManufacturer(updateProductRequest.Manufacturer, product.Id);
            await SaveListCategories(updateProductRequest.Categories, product.Id);
            await SaveListUses(updateProductRequest.Uses, product.Id);
            await SaveListContraindications(updateProductRequest.Contraindications, product.Id);
            await SaveListDosageForms(updateProductRequest.DosageForms, product.Id);
            await SaveListImage(updateProductRequest.Images, product.Id);
            return await ToProductDto(product);
        }

        public async Task<ApiObject?> Delete(int id)
        {
            var product = await ProductExist(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ApiObject()
            {
                Message = "Xóa dữ liệu thành công."
            };
        }

        public async Task<ProductDto?> GetByUrl(string url)
        {
            var product = await _context.Products
                .Include(x=>x.Manufacturer)
                .Include(x=>x.Contraindications)
                .Include(x=>x.DosageForms)
                .Include(x=>x.Uses)
                .Include(x=>x.Categories)
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Images)
                .FirstOrDefaultAsync(x => x.URL == url);
            if (product == null)
            {
                throw new NotFoundException($"Không tìm thấy dữ liệu với : {url}");
            }
            return await ToProductDto(product);
        }

        public async Task<ProductDto> Create(CreateProductRequest createProductRequest)
        {
            var existProductName =
                await _context.Products.FirstOrDefaultAsync(x => x.Name.ToUpper() == createProductRequest.Name.ToUpper());
            if (existProductName != null)
            {
                throw new AlreadyExistsException($"{createProductRequest.Name} đã tồn tại.");
            }
            var existProductUrl =
                await _context.Products.FirstOrDefaultAsync(x => x.URL.ToUpper() == createProductRequest.URL.ToUpper());
            if (existProductUrl !=null)
            {
                throw new AlreadyExistsException($"{createProductRequest.URL} đã tồn tại.");
            }

            var store = await _context.Stores.FirstOrDefaultAsync(x => 
                x.CreatedBy == _convert.ToAppUser(GlobalVariables.Token) || x.Id == createProductRequest.StoreId);
            if (store == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu nhà thuốc.");
            }
            
            var newItem = new Product
            {
                Name = createProductRequest.Name,
                URL = createProductRequest.URL,
                Quantity = createProductRequest.Quantity,
                Price = createProductRequest.Price,
                Sale = createProductRequest.Sale,
                Content = createProductRequest.Content,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = store,
            };
            await _context.Products.AddAsync(newItem);
            await _context.SaveChangesAsync();
            await SaveListUses(createProductRequest.Uses, newItem.Id);
            await SaveListDosageForms(createProductRequest.DosageForms, newItem.Id);
            await SaveListContraindications(createProductRequest.Contraindications, newItem.Id);
            await SaveListCategories(createProductRequest.Categories, newItem.Id);
            await SaveManufacturer(createProductRequest.ManufacturerId, newItem.Id);
            await SaveListImage(createProductRequest.Images, newItem.Id);
            
            await _context.SaveChangesAsync();
            return await ToProductDto(newItem);
        }

        private async Task<ProductDto> ToProductDto(Product product)
        {
            List<UseItem> useItems = [];
            foreach (var item in product.Uses)
            {
                var use = await _context.Uses.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (use != null) useItems.Add(ToUseItem(use));
            }
            
            List<DosageFormItem> dosageFormItems = [];
            foreach (var item in product.DosageForms)
            {
                var dosage = await _context.DosageForms.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (dosage != null) dosageFormItems.Add(ToDosageFormItem(dosage));
            }
            
            List<ContraindicationItem> contraindicationItems = [];
            foreach (var item in product.Contraindications)
            {
                var contraindication =
                    await _context.Contraindications.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (contraindication != null) contraindicationItems.Add(ToContraindicationItem(contraindication));
            }

            List<CategoryItem> categoryItems = [];
            foreach (var item in product.Categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (category != null) categoryItems.Add(ToCategoryItem(category));
            }

            List<int> images = [];
            images.AddRange(product.Images.Select(item => item.Id));
            
            var manufacturerItem = ToManufacturerItem(product.Manufacturer);
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Url = product.URL,
                Quantity = product.Quantity,
                Price = product.Price,
                Sale = product.Sale,
                CreatedOn = product.CreatedOn,
                Content = product.Content,
                Status = product.Status,
                Uses = useItems,
                Manufacturer = manufacturerItem,
                DosageForms = dosageFormItems,
                Contraindications = contraindicationItems,
                Categories = categoryItems,
                Images = product.ImageIds,
                CreatedBy = ToStoreItem(product.CreatedBy)
            };
        }

        private static StoreItem ToStoreItem(Store store)
        {
            return new StoreItem()
            {
                Id = store.Id,
                Name = store.Name,
                URL = store.URL,
                Avatar = store.Avatar?.Id
            };
        }
        private static UseItem ToUseItem(Use use)
        {
            return new UseItem()
            {
                Id = use.Id,
                Name = use.Name,
                Info = use.Info
            };
        }

        private static ContraindicationItem ToContraindicationItem(Contraindication item)
        {
            return new ContraindicationItem()
            {
                Id = item.Id,
                Name = item.Name,
                Info = item.Info
            };
        }
        private static ManufacturerItem ToManufacturerItem(Manufacturer item)
        {
            return new ManufacturerItem()
            {
                Id = item.Id,
                Name = item.Name,
                Info = item.Info
            };
        }

        private static DosageFormItem ToDosageFormItem(DosageForm item)
        {
            return new DosageFormItem()
            {
                Id = item.Id,
                Name = item.Name,
                Info = item.Info
            };
        }

        private static CategoryItem ToCategoryItem(Category category)
        {
            return new CategoryItem()
            {
                Id = category.Id,
                Name = category.Name,
                IsEnable = category.IsEnable
            };
        }
        private async Task SaveListImage(List<int>? images, int productId)
        {
            var product = await ProductExist(productId);
            var listImage = await _context.Images.Where(x =>product.ImageIds != null && product.ImageIds.Contains(x.Id)).ToListAsync();
            var listImageNew = await _context.Images.Where(x => images != null && images.Contains(x.Id)).ToListAsync();
            foreach (var image in listImage.Where(image => !listImageNew.Contains(image)))
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
            product.ImageIds?.Clear();
            foreach (var item in listImageNew)
            {
                product.ImageIds?.Add(item.Id);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SaveManufacturer(int manufacturerId, int productId)
        {
            var product = await ProductExist(productId);
            var manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == manufacturerId);
            if (manufacturer == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            product.Manufacturer = manufacturer;
            await _context.SaveChangesAsync();
        }

        private async Task SaveListCategories(List<int>? categories, int productId)
        {
            var product = await ProductExist(productId);
            var listItem = await _context.Categories.Where(x => categories.Contains(x.Id)).ToListAsync();
            if (listItem.Count == 0)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            product.Categories.Clear();
            await _context.SaveChangesAsync();
            foreach (var item in listItem)
            {
                product.Categories.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveListContraindications(List<int> contraindications, int productId )
        {
            var product = await ProductExist(productId);
            var listNew = await _context.Contraindications.Where(x => contraindications.Contains(x.Id)).ToListAsync();
            if (listNew.Count == 0)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            product.Contraindications.Clear();
            await _context.SaveChangesAsync();
            foreach (var item in listNew)
            {
                product.Contraindications.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveListDosageForms(List<int> dosageForms, int productId)
        {
            var product = await ProductExist(productId);
            var listNew = await _context.DosageForms.Where(x => dosageForms.Contains(x.Id)).ToListAsync();
            if (listNew.Count == 0)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            product.DosageForms.Clear();
            await _context.SaveChangesAsync();
            foreach (var item in listNew)
            {
                product.DosageForms.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveListUses(List<int> uses, int productId)
        {
            var product = await ProductExist(productId);
            var listNew = await _context.Uses.Where(x =>uses.Contains(x.Id)).ToListAsync();
            if (listNew.Count == 0)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu.");
            }
            product.Uses.Clear();
            await _context.SaveChangesAsync();
            foreach (var item in listNew)
            {
                product.Uses.Add(item);
                await _context.SaveChangesAsync();
            }
        }
        

        private async Task<Product> ProductExist(int productId)
        {
            var product = await _context.Products
                .Include(x=>x.Manufacturer)
                .Include(x=>x.Contraindications)
                .Include(x=>x.DosageForms)
                .Include(x=>x.Uses)
                .Include(x=>x.Categories)
                .Include(x=>x.CreatedBy)
                .Include(x=>x.Images)
                .FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                throw new NotFoundException($"Không tìm thấy dữ liệu với : {productId}");
            }

            return product;
        }
    }
}