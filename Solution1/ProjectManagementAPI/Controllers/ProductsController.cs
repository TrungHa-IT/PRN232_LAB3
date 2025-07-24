using BusinessObjects;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository repository = new ProductRepository();

        //[HttpGet]
        //public IActionResult GetCategories()
        //{
        //    try
        //    {
        //        var categories = CategoryDAO.GetCategories();
        //        return Ok(categories); // ✅ Đảm bảo trả về 200 OK
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        // Get all products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() => repository.GetProducts();

        // POST: Add a new product
        [HttpPost]
        public IActionResult PostProduct(ProductDTO p)
        {
            Product product = new Product
            {
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                CategoryId = p.CategoryId
            };
            repository.SaveProduct(product);
            return NoContent();
        }

        // DELETE: Delete product by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var p = repository.GetProductById(id);
            if (p == null) return NotFound();
            repository.DeleteProduct(p);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

            try
            {
                ProductDAO.UpdateProduct(product); // gọi hàm update trong DAO
                return NoContent(); // hoặc return Ok()
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: Get product by ID
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = repository.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // ✅ NEW: Search + Paging + Sorting
        [HttpGet("search")]
        public IActionResult SearchProducts(
            string? keyword = "",
            int page = 1,
            int pageSize = 5,
            string sortBy = "Id",
            bool isAsc = true)
        {
            var products = repository.GetProducts();

            // Filter by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products
                    .Where(p => p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Sort
            products = sortBy.ToLower() switch
            {
                "productname" => isAsc ? products.OrderBy(p => p.ProductName).ToList()
                                       : products.OrderByDescending(p => p.ProductName).ToList(),
                _ => isAsc ? products.OrderBy(p => p.ProductId).ToList()
                           : products.OrderByDescending(p => p.ProductId).ToList()
            };

            // Pagination
            var total = products.Count;
            var pagedData = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalItems = total,
                Page = page,
                PageSize = pageSize,
                Products = pagedData
            });
        }
    }
}
