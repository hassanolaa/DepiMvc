using FinalProject.Models.Dto.Bill;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrdersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var bills = new List<BillDto>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Bill");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            bills = await response.Content.ReadFromJsonAsync<List<BillDto>>();

            return View(bills);
        }
    }
}
