using FinalProject.Models.Dto.Product;
using FinalProject.Models.Dto.Dto.ProductBill;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FinalProject.Models.Dto.Category;
using project_cls.DAL.DataAccess.Models;

namespace FinalProject.Controllers
{
    public class CashierController : Controller
    {
        private readonly HttpClient _httpClient;
        private static List<ProductBillInsertDto> productBillList = new List<ProductBillInsertDto>(); // Temporary invoice storage

        public CashierController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Products(string category = null)
        {
            var products = new List<ProductDto>();
            var categories = new List<string>();

            // Fetch all products from the API
            var productResponse = await _httpClient.GetAsync("http://localhost:5024/api/Product");
            var categoryResponse = await _httpClient.GetAsync("http://localhost:5024/api/Category/All");

            if (productResponse.IsSuccessStatusCode)
            {
                products = await productResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
            }

            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryDtos = await categoryResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();
                categories = categoryDtos.Select(c => c.CategoryName).ToList();
            }

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.CategoryName == category).ToList();
            }

            ViewBag.SelectedCategory = category;
            ViewBag.Categories = categories;
            ViewBag.ProductBill = productBillList;
            return View(products);
        }

        [HttpPost]
        public IActionResult Select(int id)
        {
            // Check if product already exists in the bill
            var existingItem = productBillList.FirstOrDefault(pb => pb.ProductId == id);

            if (existingItem != null)
            {
                // Increase quantity if it exists
                //existingItem.Quantity++;
            }
            else
            {
                // Add new item to bill
                productBillList.Add(new ProductBillInsertDto { ProductId = id, Quantity = 1 });
            }

            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitBill()
        {
            if (!productBillList.Any())
            {
                ModelState.AddModelError("", "No products in the bill.");
                return RedirectToAction("Products");
            }

            // لف الفاتورة في كائن يحتوي على الخاصية المطلوبة
            var billWrapper = new
            {
                productBills = productBillList
            };

            // إرسال البيانات للـ API
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5024/api/Bill", billWrapper);

            if (response.IsSuccessStatusCode)
            {
                // تفريغ الفاتورة بعد الإرسال الناجح
                productBillList.Clear();
                return RedirectToAction("Products");
            }
            else
            {
                // معالجة الخطأ وإعادة التوجيه لصفحة المنتجات
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Failed to submit the bill. Error: {errorMessage}");
                return RedirectToAction("Products");
            }
        }




        [HttpPost]
        public IActionResult DeleteBill()
        {

            productBillList.Clear();
            return RedirectToAction("Products");
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string action)
        {
            var existingItem = productBillList.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                if (action == "increase")
                {
                    existingItem.Quantity++;
                }
                else if (action == "decrease" && existingItem.Quantity > 1) // تأكد من أن الكمية لا تقل عن 1
                {
                    existingItem.Quantity--;
                }

                // هنا يمكنك إضافة منطق إذا كنت تريد إزالة العنصر إذا كانت الكمية 0
                if (existingItem.Quantity == 0)
                {
                    productBillList.Remove(existingItem);
                }
            }

            return RedirectToAction("Products"); // العودة إلى صفحة المنتجات
        }


    }
}
