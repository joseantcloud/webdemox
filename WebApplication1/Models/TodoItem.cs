using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsDone { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? DueAtUtc { get; set; }
}