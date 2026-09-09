namespace StudentManagement.API.Models;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentRegNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }

    public User? User { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}