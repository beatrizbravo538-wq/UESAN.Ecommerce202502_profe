using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UESAN.Ecommerce.CORE.Core.DTOs;
using UESAN.Ecommerce.CORE.Core.Entities;
using UESAN.Ecommerce.CORE.Core.Interfaces;

namespace UESAN.Ecommerce.CORE.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductListDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetProducts();
            var list = new List<ProductListDTO>();

            foreach (var p in products)
            {
                list.Add(new ProductListDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    Price = p.Price,
                    Discount = p.Discount,
                    CategoryId = p.CategoryId,
                    Category = p.Category == null ? null : new CategoryListDTO { Id = p.Category.Id, Description = p.Category.Description }
                });
            }

            return list;
        }

        public async Task<ProductListDTO> GetProductById(int id)
        {
            var p = await _productRepository.GetProductById(id);
            if (p == null) return null;

            return new ProductListDTO
            {
                Id = p.Id,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Stock = p.Stock,
                Price = p.Price,
                Discount = p.Discount,
                CategoryId = p.CategoryId,
                Category = p.Category == null ? null : new CategoryListDTO { Id = p.Category.Id, Description = p.Category.Description }
            };
        }

        public async Task<int> InsertProduct(ProductCreateDTO productCreateDTO)
        {
            var product = new Product
            {
                Description = productCreateDTO.Description,
                ImageUrl = productCreateDTO.ImageUrl,
                Stock = productCreateDTO.Stock,
                Price = productCreateDTO.Price,
                Discount = productCreateDTO.Discount,
                CategoryId = productCreateDTO.CategoryId,
                IsActive = true
            };

            var id = await _productRepository.InsertProduct(product);
            return id;
        }

        public async Task UpdateProduct(ProductListDTO productListDTO)
        {
            var product = new Product
            {
                Id = productListDTO.Id,
                Description = productListDTO.Description,
                ImageUrl = productListDTO.ImageUrl,
                Stock = productListDTO.Stock,
                Price = productListDTO.Price,
                Discount = productListDTO.Discount,
                CategoryId = productListDTO.CategoryId
            };

            await _productRepository.UpdateProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }
    }
}
