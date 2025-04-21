using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Validators
{
    public class StudentValidator : IRequestValidator
    {
        public bool IsValid(object dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto is not RegisterStudentDto student)
            {
                errorMessage = ValidationMessages.InvalidStudentObject;
                return false;
            }
            
            if (student.IsParentRegister && string.IsNullOrWhiteSpace(student.ParentFullName))
            {
                errorMessage = ValidationMessages.ParentFullNameRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.FullName))
            {
                errorMessage = ValidationMessages.FullNameIsRequired;
                return false;
            }

            if (student.BirthDate == default)
            {
                errorMessage = ValidationMessages.BirthDateIsRequired;
                return false;
            }
            
            if (student.BirthDate >= DateTime.Now)
            {
                errorMessage = ValidationMessages.BirthDateMustBeCorrect;
                return false;
            }
            
            if (student.Grade.HasValue && (student.Grade < 0 || student.Grade > 12))
            {
                errorMessage = ValidationMessages.GradeMustBeCorrect;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.Phone))
            {
                errorMessage = ValidationMessages.PhoneNumberIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.Email))
            {
                errorMessage = ValidationMessages.EmailIsRequired;
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(student.LanguageName))
            {
                errorMessage = ValidationMessages.LanguageIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.Level))
            {
                errorMessage = ValidationMessages.LevelIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.GroupName))
            {
                errorMessage = ValidationMessages.GroupNameIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.LessonDays))
            {
                errorMessage = ValidationMessages.LessonDaysIsRequired;
                return false;
            }

            if (student.PairNumber < 0)
            {
                errorMessage = ValidationMessages.PairNumberMustBeCorrect;
                return false;
            }

            if (student.SignDate == default)
            {
                errorMessage = ValidationMessages.SignDateIsRequired;
                return false;
            }
            
            if (student.SignDate > DateTime.Now)
            {
                errorMessage = ValidationMessages.SignDateMustBeCorrect;
                return false;
            }
            
            if (student.PaymentAmount < 0)
            {
                errorMessage = ValidationMessages.PaymentAmountMustBeCorrect;
                return false;
            }

            return true;
        }
    }
}