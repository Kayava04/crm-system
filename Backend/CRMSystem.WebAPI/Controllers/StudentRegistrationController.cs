using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/student-registration")]
    public class StudentRegistrationController(
        StudentRegistrationService service,
        IFileService<RegisterStudentDto> fileService,
        IValidatorFactory validatorFactory,
        ILogger<StudentRegistrationController> logger) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Create([FromBody] RegisterStudentDto studentDto)
        {
            if (!validatorFactory.Validate(studentDto, out var errorMessage))
            {
                logger.LogWarning($"Retrieved invalid Student data: {errorMessage}");
                return BadRequest(errorMessage);
            }

            var student = await service.CreateStudentAsync(studentDto);
            
            logger.LogInformation($"Student with ID: {student.Id}, added successfully.");
            return Ok(student);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await service.GetStudentByIdAsync(id);

            if (!validatorFactory.Validate(student, out var errorMessage))
            {
                logger.LogWarning($"Student with ID: {student.Id} not found: {errorMessage}");
                return BadRequest(errorMessage);
            }

            logger.LogInformation($"Retrieved student with ID: {student.Id}.");
            return Ok(student);
        }
        
        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetAll([FromQuery] StudentFilterDto? filter)
        {
            var students = await service.GetAllStudentsAsync(filter);
            
            logger.LogInformation($"Retrieved all Students: {students.Count()}.");
            return Ok(students);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RegisterStudentDto studentDto)
        {
            if (!validatorFactory.Validate(studentDto, out var errorMessage))
            {
                logger.LogWarning($"Retrieved invalid update Student data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var student = await service.UpdateStudentAsync(id, studentDto);

            if (student == null)
                return NotFound();
            
            logger.LogInformation($"Student with ID: {student.Id} updated successfully.");
            return Ok(student);
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await service.DeleteStudentAsync(id);
            
            logger.LogInformation($"Student with ID: {id} deleted successfully.");
            return NoContent();
        }
        
        [HttpPost("import-file")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> ImportFile(IFormFile file)
        {
            var data = await fileService.ImportFromFileAsync(file);
            
            // foreach (var student in data)
            // {
            //     if (!validatorFactory.Validate(student, out var errorMessage))
            //     {
            //         logger.LogWarning($"Invalid data found in file '{file.FileName}': {errorMessage}");
            //         return BadRequest(errorMessage);
            //     }
            // }
            
            logger.LogInformation($"Students data imported successfully from {file.FileName.ToUpper()} file. Total count: {data.Count()}");
            return Ok(data);
        }

        [HttpPost("export-file")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileName)
        {
            var (data, contentType, downloadName) = await fileService.ExportToFileAsync(fileName);
            
            logger.LogInformation($"Students data exported successfully to {fileName.ToUpper()} file.");
            return File(data, contentType, downloadName);
        }
    }
}