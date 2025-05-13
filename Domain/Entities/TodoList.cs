using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAppApi1.Domain.Common;

namespace TodoAppApi1.Domain.Entities;

public class TodoList : BaseAuditableEntity
{
    public string? Title { get; set; }
    public IList<TodoItems> Items { get; private set; } = new List<TodoItems>();
}   