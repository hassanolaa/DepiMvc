using FinalProject.Models;
using FinalProject.Models.Dto.Department;
using FinalProject.Models.Dto.Employee;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HttpClient _httpClient;

        public DepartmentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            List<Department> departments = new List<Department>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Depatment/All");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            departments = await response.Content.ReadFromJsonAsync<List<Department>>();

            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5024/api/Depatment?id={id}");
            //List<Department> departments = new List<Department>();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the department.");
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Add(DepartmentInsertDto DepartmentInsertDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:5024/api/Depatment/insert", DepartmentInsertDto);
            //List<Department> departments = new List<Department>();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            Console.WriteLine("error");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the department.");
            return RedirectToAction("Index");
        }



    }
}
