namespace CRMSystem.WebAPI.DTOs.Auth
{
    public record SignUpRequestDto(string FullName, DateTime BirthDate, string Email, string Username, string Password, string ConfirmPassword);
}