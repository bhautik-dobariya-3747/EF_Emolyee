using Employee.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.Models;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        //  Constructor in your EmployeeController file :
        public EmployeeController(IEmployeeService service)
        {
            _employeeService = service;
        }
            
        [HttpPost("Create")]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (_employeeService.IsEmailExists(employee.Email))
                    return BadRequest("Email address already exists.");
                                
                var created = _employeeService.Create(employee);
                if (created == null)
                return StatusCode(500, "Employee creation failed.");
                
                return Ok(new Dictionary<string, object>
                {
                     { "message", "Employee created successfully." }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (_employeeService.IsEmailExists(employee.Email, employee.Guid))
                    return BadRequest("Email already exists for another employee.");

                var updated = _employeeService.Update(employee);
                if (updated == null)
                    return NotFound("Employee not found.");

                return Ok(new { message = "Employee updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                bool success = _employeeService.Delete(guid);
                if (success)
                    return Ok(new { message = "Employee deleted successfully." });

                return NotFound("Employee not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid guid)
        {
            try
            {
                var employee = _employeeService.Get(guid);

                if (employee == null)
                    return NotFound("Employee not found.");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}