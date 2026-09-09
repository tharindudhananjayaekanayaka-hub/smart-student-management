using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.DTOs;
using System.Security.Claims;

namespace StudentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1. තමන්ගේ Profile විස්තර ලබාගැනීම (Student & Admin)
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized("Invalid token claims.");

        var user = await _context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found.");

        return Ok(new
        {
            user.Id,
            user.Email,
            user.Role,
            user.CreatedAt,
            StudentDetails = user.Student != null ? new
            {
                user.Student.Id,
                user.Student.FullName,
                user.Student.StudentRegNumber,
                user.Student.ProfileImageUrl
            } : null
        });
    }

    // 2. Profile එක Update කිරීම (Student Details update)
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized("Invalid token claims.");

        var user = await _context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found.");

        if (user.Student == null)
            return BadRequest("No student profile linked to this account.");

        // දත්ත update කිරීම
        if (!string.IsNullOrWhiteSpace(dto.FullName))
            user.Student.FullName = dto.FullName;

        if (!string.IsNullOrWhiteSpace(dto.ProfileImageUrl))
            user.Student.ProfileImageUrl = dto.ProfileImageUrl;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Profile updated successfully!",
            student = new
            {
                user.Student.FullName,
                user.Student.ProfileImageUrl
            }
        });
    }

    // 3. Admin සඳහා පමණක්: සියලුම Users ලාගේ විස්තර බැලීම (Role-based Authorization)
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Include(u => u.Student)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.Role,
                u.CreatedAt,
                FullName = u.Student != null ? u.Student.FullName : "Admin",
                StudentRegNumber = u.Student != null ? u.Student.StudentRegNumber : null
            })
            .ToListAsync();

        return Ok(users);
    }

    // Token එකෙන් User ID එක ලබාගන්නා Helper method එක
    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}