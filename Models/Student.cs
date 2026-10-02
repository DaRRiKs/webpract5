namespace StudentClient.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public int Course { get; set; }
    public string Email { get; set; } = string.Empty;
}
