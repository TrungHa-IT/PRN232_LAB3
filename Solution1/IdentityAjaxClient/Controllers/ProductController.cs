using System.Net.Http;
using System.Net.Http.Json;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAjaxClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ProductController(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _baseUrl = config["AppSettings:ApiBaseUrl"];
        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>($"{_baseUrl}/Products");
            return View(products);
        }

        // GET: ProductController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Products", product);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return View(product);
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"{_baseUrl}/Products/{id}");
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/Products/{id}", product);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return View(product);
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"{_baseUrl}/Products/{id}");
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Products/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
