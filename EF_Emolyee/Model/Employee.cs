using System;
using System.ComponentModel.DataAnnotations;

public class EmployeeModel
{
    [Required(ErrorMessage = "GUID is required.")]
    public Guid Guid { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    public string? Department { get; set; }

    [Required(ErrorMessage = "Salary is required.")]
    public decimal? Salary { get; set; }

    [Required(ErrorMessage = "IsActive is required.")]
    public bool? IsActive { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
