
using FinalProject.Models;
using FinalProject.Models.Dto.Employee;
using FinalProject.Models.Dto.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;






namespace FinalProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;

        public DashboardController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IActionResult> index()
        {
            var employees = new List<ProductDto>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Product");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            employees = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

            return View(employees);
        }


    }
}