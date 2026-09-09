namespace StudentManagement.API.DTOs;

public class RegisterDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Student"; // Student හෝ Admin
    public string? StudentRegNumber { get; set; } // Student නම් පමණක් අදාළ වේ
}