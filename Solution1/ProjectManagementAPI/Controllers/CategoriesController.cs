using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Repositories;
namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryRepository repository = new CategoryRepository();
        [HttpGet]
        public IActionResult GetCategories()
        {

            var categories = repository.GetCategories();
            return Ok(categories);
        }
    }
}