using FinalProject.Models.Dto.Bill;
using FinalProject.Models.Dto.ProductBill;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class kitchenController : Controller
    {

        private readonly HttpClient _httpClient;

        public kitchenController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var bills = new List<BillDto>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Bill/Statu?isDone=false");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            bills = await response.Content.ReadFromJsonAsync<List<BillDto>>();

            return View(bills);
        }

        [HttpPost]
        public async Task<IActionResult> Done(BillUpdateDto billUpdate) {
        
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5024/api/Bill", billUpdate);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            ModelState.AddModelError(string.Empty, "An error occurred while updating the bill.");
            return RedirectToAction("Index");
         
          
        
        }


    }
}
