using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAppApi1.Domain.Common;
using TodoAppApi1.Domain.Enums;


namespace TodoAppApi1.Domain.Entities;

public class TodoItems : BaseAuditableEntity
{
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Reminder { get; set; }
    
    public TodoStatus Status { get; set; }
    public TodoTags Tag { get; set; }
    
    // Foreign key
    public int? TodoListId { get; set; }

    // Navigation property
    public TodoList? TodoList { get; set; }
}