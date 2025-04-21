using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Interfaces;
using FileToolKit.IO.File.Factories;

namespace CRMSystem.WebAPI.Services
{
    public class StudentFileService(
        StudentRegistrationService service,
        IFileToolFactory<RegisterStudentDto> fileService,
        ILogger<StudentFileService> logger,
        IMapper mapper,
        IValidatorFactory validatorFactory) : IFileService<RegisterStudentDto>
    {
        public async Task<IEnumerable<RegisterStudentDto>> ImportFromFileAsync(IFormFile file)
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

            foreach (var student in importedData)
            {
                student.BirthDate = DateTime.SpecifyKind(student.BirthDate, DateTimeKind.Utc);
                student.SignDate = DateTime.SpecifyKind(student.SignDate, DateTimeKind.Utc);
                
                if (!validatorFactory.Validate(student, out var errorMessage))
                {
                    logger.LogWarning($"Invalid student found during import: {errorMessage}");
                    throw new ApplicationException($"Invalid student data: {errorMessage}.");
                }
                
                await service.CreateStudentAsync(student);
            }

            logger.LogInformation($"Imported {importedData.Count()} students from file: {file.FileName}");
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

            var students = await service.GetAllStudentsAsync();
            var data = mapper.Map<List<RegisterStudentDto>>(students);

            for (var i = 0; i < students.Count(); i++)
                data[i].IsParentRegister = !string.IsNullOrWhiteSpace(students.ElementAt(i).ParentFullName);
            
            var fileData = await tool.ExportAsync(data);

            var contentType = extension.ToLower() switch
            {
                ".csv" => "text/csv",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };

            var finalName = $"{DateTime.UtcNow:yyyyMMdd}-Students-{fileName}";
            return (fileData, contentType, finalName);
        }
    }
}