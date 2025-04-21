namespace CRMSystem.WebAPI.Validators
{
    public static class ValidationMessages
    {
        // For Employee Dto
        public const string InvalidEmployeeObject = "Employee object is invalid!";
        public const string FullNameIsRequired = "FullName is required!";
        public const string BirthDateIsRequired = "Birth date is required!";
        public const string BirthDateMustBeCorrect = "Birth date must be correct!";
        public const string PhoneNumberIsRequired = "Phone number is required!";
        public const string EmailIsRequired = "Email address is required!";
        public const string PositionIsRequired = "Position is required!";
        public const string SalaryMustBeCorrect = "Salary cannot be negative!";

        // For Student Dto
        public const string InvalidStudentObject = "Student object is invalid!";
        public const string ParentFullNameRequired = "Parent FullName is required when 'IsParentRegister' is true!";
        public const string GradeMustBeCorrect = "Grade must be between 1 and 12!";
        public const string PairNumberMustBeCorrect = "Pair number must be greater than 0!";
        public const string LanguageIsRequired = "Language is required!";
        public const string LevelIsRequired = "Level is required!";
        public const string GroupNameIsRequired = "GroupName is required!";
        public const string LessonDaysIsRequired = "LessonDays is required!";
        public const string SignDateIsRequired = "SignDate is required!";
        public const string SignDateMustBeCorrect = "SignDate must be correct!";
        public const string PaymentAmountMustBeCorrect = "Payment amount cannot be negative!";
        
        // For SignUp Dto
        public const string InvalidObjectType = "Invalid object type!";
        public const string FullNameMustBeCorrect = "FullName must be at least 3 characters long!";
        public const string UserNameMustBeCorrect = "UserName must be at least 4 characters long!";
        public const string PasswordMustBeCorrect = "Password must be at least 6 characters long and contain at least one letter and one number!";
        public const string PasswordsDoNotMatch = "Passwords do not match!";
        public const string OldPasswordIsRequired = "Old password is required!";
        public const string EmailMustBeCorrect = "Invalid email address!";
        
        // For SignIn Dto
        public const string UsernameIsRequired = "Username is required!";
        public const string PasswordIsRequired = "Password is required!";
    }
}