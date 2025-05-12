using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppApi1.Models;

public class TodoList
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] //annotation
    public int Id { get; set; }
    public string? Task { get; set; }
    public TodoStatus Status { get; set; }
    public TodoTag Tag { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public enum TodoStatus
    {
        Starting = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3,
    }

    public enum TodoTag
    {
        HouseWork = 0,
        Trainning = 1,
        Abc = 2,
    }
}