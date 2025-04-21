using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/employee-registration")]
    public class EmployeeRegistrationController(
        EmployeeRegistrationService service,
        IFileService<RegisterEmployeeDto> fileService,
        IValidatorFactory validatorFactory,
        ILogger<EmployeeRegistrationController> logger) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Create([FromBody] RegisterEmployeeDto employeeDto)
        {
            if (!validatorFactory.Validate(employeeDto, out var errorMessage))
            {
                logger.LogWarning($"Retrieved invalid Employee data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var employee = await service.CreateEmployeeAsync(employeeDto);
            
            logger.LogInformation($"Employee with ID: {employee.Id}, added successfully.");
            return Ok(employee);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await service.GetEmployeeByIdAsync(id);
            
            if (!validatorFactory.Validate(employee, out var errorMessage))
            {
                logger.LogWarning($"Employee with ID: {employee.Id} not found: {errorMessage}");
                return BadRequest(errorMessage);
            }

            logger.LogInformation($"Retrieved employee with ID: {employee.Id}.");
            return Ok(employee);
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetAll([FromQuery] EmployeeFilterDto filter)
        {
            var employees = await service.GetAllEmployeesAsync(filter);
            
            logger.LogInformation($"Retrieved all Employees: {employees.Count()}.");
            return Ok(employees);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RegisterEmployeeDto employeeDto)
        {
            if (!validatorFactory.Validate(employeeDto, out var errorMessage))
            {
                logger.LogWarning($"Retrieved invalid update Employee data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var employee = await service.UpdateEmployeeAsync(id, employeeDto);
            
            if (employee == null)
                return NotFound();

            logger.LogInformation($"Employee with ID: {employee.Id} updated successfully.");
            return Ok(employee);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await service.DeleteEmployeeAsync(id);
            
            logger.LogInformation($"Employee with ID: {id} deleted successfully.");
            return NoContent();
        }

        [HttpPost("import-file")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> ImportFile(IFormFile file)
        {
            var data = await fileService.ImportFromFileAsync(file);

            // foreach (var employee in data)
            // {
            //     if (!validatorFactory.Validate(employee, out var errorMessage))
            //     {
            //         logger.LogWarning($"Invalid data found in file '{file.FileName}': {errorMessage}");
            //         return BadRequest(errorMessage);
            //     }
            // }
            
            logger.LogInformation($"Employee data imported successfully from {file.FileName.ToUpper()} file. Total count: {data.Count()}");
            return Ok(data);
        }

        [HttpPost("export-file")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileName)
        {
            var (data, contentType, downloadName) = await fileService.ExportToFileAsync(fileName);
            
            logger.LogInformation($"Employee data exported successfully to {fileName.ToUpper()} file.");
            return File(data, contentType, downloadName);
        }
    }
}