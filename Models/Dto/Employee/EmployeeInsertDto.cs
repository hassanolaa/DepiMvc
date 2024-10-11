using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Dto.Employee
{
    public class EmployeeInsertDto
    {

        [Required(ErrorMessage = "Employee name is required.")]
        public string EmpName { get; set; }
        
        [Required(ErrorMessage = "Employee position is required.")]
        public string postion { get; set; }

        [Required(ErrorMessage = "Employee username is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Employee password is required.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Employee salary is required.")]
        public string salary { get; set; }

        [Required(ErrorMessage = "Employee department is required.")]
        public int DeptId { get; set; } 

    }
}
