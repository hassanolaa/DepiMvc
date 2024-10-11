using FinalProject.Models.Dto.Category;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models.Dto.Department;
using FinalProject.Models.Dto.Employee;

namespace FinalProject.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            List<CategoryDto> categories = new List<CategoryDto>();


            var response = await _httpClient.GetAsync("http://localhost:5024/api/Category/All");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5024/api/Category?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee.");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryInsertDto categoryInsertDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:5024/api/Category/insert", categoryInsertDto);
            //List<Department> departments = new List<Department>();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            Console.WriteLine("error");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the category.");
            return RedirectToAction("Index");
        }
    }
}
