using FinalProject.Models;
using FinalProject.Models.Dto.Employee;
using FinalProject.Models.Dto.Product;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using project_cls.DAL.DataAccess.Models;


namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var employees = new List<EmployeeDto>();

        //    var response = await _httpClient.GetAsync("http://localhost:5024/api/Employee");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorContent = await response.Content.ReadAsStringAsync();
        //        // Log the error content for debugging
        //        return NotFound();
        //    }

        //    employees = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>();

        //    return View(employees);
        //}


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

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var departments = new List<Category>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Category/All");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }
            departments = await response.Content.ReadFromJsonAsync<List<Category>>();

            ViewData["Categories"] = departments;

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductInsertDto product)
        {


            if (!ModelState.IsValid)
            {
                //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                //{

                //    Console.WriteLine(error.ErrorMessage); 
                //}


                var response = await _httpClient.GetAsync("http://localhost:5024/api/Category/All");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var departments = await response.Content.ReadFromJsonAsync<List<Category>>();
                ViewData["Categories"] = departments;
                return View();
            }


            var response2 = await _httpClient.PostAsJsonAsync("http://localhost:5024/api/Product", product);
            if (response2.IsSuccessStatusCode)
            {


                return RedirectToAction("index");
                
            }


            ModelState.AddModelError(string.Empty, "An error occurred while adding the employee.");
            var departmentsAgain = await _httpClient.GetAsync("http://localhost:5024/api/Category/All");
            ViewData["Categories"] = await departmentsAgain.Content.ReadFromJsonAsync<List<Category>>();
            return View();
        }













        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5024/api/Product?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("index"); // Redirect to the employee list after deletion
            }

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee.");
            return RedirectToAction("index");
        }

       

        [HttpGet]
        public async Task<IActionResult> update(int id, string name, int price, string description,string images,int categoryids)
        {

            ViewBag.ProductId = id; // Store the employee ID in ViewBag
            ViewBag.ProductName = name; // Store the employee name in ViewBag
            ViewBag.Price = price; 
            ViewBag.Description = description;
            ViewBag.Image = images;
            ViewBag.CategoryId = categoryids;



            return View();
        }
        [HttpPost]
        public async Task<IActionResult> update(int ProductId, string ProductName, int Price, string Description, int CategoryId,string Image)
        {
            //if (string.IsNullOrWhiteSpace(newPassword))
            //{
            //    ModelState.AddModelError(string.Empty, "Password cannot be empty.");
            //    return View();
            //}

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5024/api/Product", new { ProductId = ProductId, ProductName = ProductName, Price = Price, Description = Description, CategoryId = CategoryId, Image=Image});

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("index"); // Redirect to index after success
            }

            ModelState.AddModelError(string.Empty, "An error occurred while changing the product");
            return View();
        }







    }
}
