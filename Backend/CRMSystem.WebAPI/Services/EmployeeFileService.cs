using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Interfaces;
using FileToolKit.IO.File.Factories;

namespace CRMSystem.WebAPI.Services
{
    public class EmployeeFileService(
        EmployeeRegistrationService service,
        IFileToolFactory<RegisterEmployeeDto> fileService,
        ILogger<EmployeeFileService> logger,
        IMapper mapper,
        IValidatorFactory validatorFactory) : IFileService<RegisterEmployeeDto>
    {
        public async Task<IEnumerable<RegisterEmployeeDto>> ImportFromFileAsync(IFormFile file)
        {
            if (file.Length == 0)
            {
                logger.LogError("Import failed: File is empty.");
                return [];
            }
            
            var extension = Path.GetExtension(file.FileName);
            var tool = fileService.GetTool(extension);
            
            await using var stream = file.OpenReadStream();
            var importedData = await tool.ImportAsync(stream);
            
            foreach (var employee in importedData)
            {
                employee.BirthDate = DateTime.SpecifyKind(employee.BirthDate, DateTimeKind.Utc);
                
                if (!validatorFactory.Validate(employee, out var errorMessage))
                {
                    logger.LogWarning($"Invalid student found during import: {errorMessage}");
                    throw new ApplicationException($"Invalid student data: {errorMessage}.");
                }
                
                await service.CreateEmployeeAsync(employee);
            }
            
            logger.LogInformation($"Imported {importedData.Count()} employees from file: {file.FileName}");
            return importedData;
        }
        
        public async Task<(byte[] data, string contentType, string downloadFileName)> ExportToFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                logger.LogError("Export failed: fileName is empty or null.");
                throw new ArgumentException("FileName cannot be empty");
            }

            var extension = Path.GetExtension(fileName);
            var tool = fileService.GetTool(extension);

            var employees = await service.GetAllEmployeesAsync();
            var data = mapper.Map<IEnumerable<RegisterEmployeeDto>>(employees);
            
            var fileData = await tool.ExportAsync(data);

            var contentType = extension.ToLower() switch
            {
                ".csv" => "text/csv",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };
            
            var finalName = $"{DateTime.UtcNow:yyyyMMdd}-Employees-{fileName}";
            return (fileData, contentType, finalName);
        }
    }
}