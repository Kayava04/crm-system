using System.Net.Mail;
using System.Text.RegularExpressions;
using CRMSystem.WebAPI.DTOs.Auth;
using CRMSystem.WebAPI.DTOs.User;
using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.Validators
{
    public class UserValidator : IRequestValidator
    {
        public bool IsValid(object dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            switch (dto)
            {
                case SignUpRequestDto signUp:
                    return ValidateSignUp(signUp, out errorMessage);
                case SignInRequestDto signIn:
                    return ValidateSignIn(signIn, out errorMessage);
                case UpdateUserDto updateUser:
                    return ValidateUpdating(updateUser, out errorMessage);
                default:
                    errorMessage = ValidationMessages.InvalidObjectType;
                    return false;
            }
        }

        private bool ValidateSignUp(SignUpRequestDto user, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (string.IsNullOrWhiteSpace(user.FullName) || user.FullName.Length < 3)
            {
                errorMessage = ValidationMessages.FullNameMustBeCorrect;
                return false;
            }
            
            if (user.BirthDate == default)
            {
                errorMessage = ValidationMessages.BirthDateIsRequired;
                return false;
            }
            
            if (user.BirthDate >= DateTime.Now)
            {
                errorMessage = ValidationMessages.BirthDateMustBeCorrect;
                return false;
            }

            if (!IsValidEmail(user.Email))
            {
                errorMessage = ValidationMessages.EmailMustBeCorrect;
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Username) || user.Username.Length < 4)
            {
                errorMessage = ValidationMessages.UserNameMustBeCorrect;
                return false;
            }

            if (!IsValidPassword(user.Password))
            {
                errorMessage = ValidationMessages.PasswordMustBeCorrect;
                return false;
            }
            
            if (user.Password != user.ConfirmPassword)
            {
                errorMessage = ValidationMessages.PasswordsDoNotMatch;
                return false;
            }
            
            return true;
        }
        
        private bool ValidateSignIn(SignInRequestDto user, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                errorMessage = ValidationMessages.UsernameIsRequired;
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                errorMessage = ValidationMessages.PasswordIsRequired;
                return false;
            }
            
            return true;
        }

        private bool ValidateUpdating(UpdateUserDto user, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (string.IsNullOrWhiteSpace(user.FullName) || user.FullName.Length < 3)
            {
                errorMessage = ValidationMessages.FullNameMustBeCorrect;
                return false;
            }

            if (user.BirthDate == default)
            {
                errorMessage = ValidationMessages.BirthDateIsRequired;
                return false;
            }
            
            if (user.BirthDate >= DateTime.Now)
            {
                errorMessage = ValidationMessages.BirthDateMustBeCorrect;
                return false;
            }

            if (!IsValidEmail(user.Email))
            {
                errorMessage = ValidationMessages.EmailMustBeCorrect;
                return false;
            }

            var passwordChangeRequested =
                !string.IsNullOrWhiteSpace(user.OldPassword) ||
                !string.IsNullOrWhiteSpace(user.NewPassword) ||
                !string.IsNullOrWhiteSpace(user.ConfirmNewPassword);

            if (passwordChangeRequested)
            {
                if (string.IsNullOrWhiteSpace(user.OldPassword))
                {
                    errorMessage = ValidationMessages.OldPasswordIsRequired;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(user.NewPassword) || user.NewPassword.Length < 6)
                {
                    errorMessage = ValidationMessages.PasswordMustBeCorrect;
                    return false;
                }

                if (user.NewPassword != user.ConfirmNewPassword)
                {
                    errorMessage = ValidationMessages.PasswordsDoNotMatch;
                    return false;
                }
            }
            
            return true;
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 6)
                return false;
            
            var hasLetter = Regex.IsMatch(password, "[a-zA-Z]");
            var hasDigit = Regex.IsMatch(password, "[0-9]");

            return hasLetter && hasDigit;
        }
    }
}