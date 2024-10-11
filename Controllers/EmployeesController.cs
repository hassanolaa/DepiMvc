using FinalProject.Models;
using FinalProject.Models.Dto.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;


namespace FinalProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployeesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var employees = new List<EmployeeDto>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Employee");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }

            employees = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>();

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var departments = new List<Department>();

            var response = await _httpClient.GetAsync("http://localhost:5024/api/Depatment/All");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content for debugging
                return NotFound();
            }
            departments = await response.Content.ReadFromJsonAsync<List<Department>>();

            ViewData["Departments"] = departments;

            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeInsertDto employee)
        {
           

            if (!ModelState.IsValid)
            {
                //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                //{
                    
                //    Console.WriteLine(error.ErrorMessage); 
                //}
                

                var response = await _httpClient.GetAsync("http://localhost:5024/api/Department/All");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var departments = await response.Content.ReadFromJsonAsync<List<Department>>();
                ViewData["Departments"] = departments; 
                return View();
            }
          

            var response2 = await _httpClient.PostAsJsonAsync("http://localhost:5024/api/Employee", employee);
            if (response2.IsSuccessStatusCode)
            {
                

                return RedirectToAction("Index");
            }
            

            ModelState.AddModelError(string.Empty, "An error occurred while adding the employee.");
            var departmentsAgain = await _httpClient.GetAsync("http://localhost:5024/api/Department/All");
            ViewData["Departments"] = await departmentsAgain.Content.ReadFromJsonAsync<List<Department>>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5024/api/Employee?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the employee list after deletion
            }

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id,string name,string oldpassword)
        {
            
            ViewBag.EmpId = id; // Store the employee ID in ViewBag
            ViewBag.EmpName = name; // Store the employee name in ViewBag
            ViewBag.OldPassword = oldpassword; // Store the employee old password in ViewBag
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int empId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError(string.Empty, "Password cannot be empty.");
                return View();
            }

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5024/api/Employee/employees/update-password", new { empId= empId, password = newPassword });

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to index after success
            }

            ModelState.AddModelError(string.Empty, "An error occurred while changing the password.");
            return View();
        }

        
       



    }
}
