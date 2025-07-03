using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Repositories;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository repository = new ProductRepository();

        //Get: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() => repository.GetProducts();

        //POST: ProductsController/Products
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

        //GET: ProductsController/Delete/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var p = repository.GetProductById(id);
            if (p == null) return NotFound();
            repository.DeleteProduct(p);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO p)
        {
            var pTmp = repository.GetProductById(id);
            if (pTmp == null) return NotFound();

            pTmp.ProductName = p.ProductName;
            pTmp.UnitPrice = p.UnitPrice;
            pTmp.UnitsInStock = p.UnitsInStock;
            pTmp.CategoryId = p.CategoryId;

            repository.UpdateProduct(pTmp);
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = repository.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

    }
}
