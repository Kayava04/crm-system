using CRMSystem.WebAPI.DTOs.Auth;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.DTOs.User;
using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Validators
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IRequestValidator GetValidator(object type)
        {
            return type switch
            {
                RegisterStudentDto => new StudentValidator(),
                RegisterEmployeeDto => new EmployeeValidator(),
                SignUpRequestDto  => new UserValidator(),
                SignInRequestDto => new UserValidator(),
                UpdateUserDto => new UserValidator(),
                _ => throw new ArgumentException($"Validator for type '{type}' not found.")
            };
        }

        public bool Validate(object dto, out string errorMessage)
        {
            var validator = GetValidator(dto);
            return validator.IsValid(dto, out errorMessage);
        }
    }
}