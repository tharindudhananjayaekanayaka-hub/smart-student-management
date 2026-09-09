namespace StudentManagement.API.DTOs;

public class UpdateProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}