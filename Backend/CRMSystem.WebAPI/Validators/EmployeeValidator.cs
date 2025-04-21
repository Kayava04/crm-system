using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Validators
{
    public class EmployeeValidator : IRequestValidator
    {
        public bool IsValid(object dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto is not RegisterEmployeeDto employee)
            {
                errorMessage = ValidationMessages.InvalidEmployeeObject;
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                errorMessage = ValidationMessages.FullNameIsRequired;
                return false;
            }

            if (employee.BirthDate == default)
            {
                errorMessage = ValidationMessages.BirthDateIsRequired;
                return false;
            }
            
            if (employee.BirthDate >= DateTime.Now)
            {
                errorMessage = ValidationMessages.BirthDateMustBeCorrect;
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.Phone))
            {
                errorMessage = ValidationMessages.PhoneNumberIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                errorMessage = ValidationMessages.EmailIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.Position))
            {
                errorMessage = ValidationMessages.PositionIsRequired;
                return false;
            }
            
            if (employee.Salary < 0)
            {
                errorMessage = ValidationMessages.SalaryMustBeCorrect;
                return false;
            }

            return true;
        }
    }
}