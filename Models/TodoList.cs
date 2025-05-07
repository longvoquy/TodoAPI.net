namespace TodoAppApi1.Models;

public class TodoList
{
    //properties 
    public int Id { get; set; }
    public string? Task { get; set; }
    public TodoStatus Status { get; set; }
    public TodoTag Tag { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //public const string Complete = "Complete";
    public enum TodoStatus
    {   
        Starting=0,
        InProgress=1,
        Completed=2,
        Cancelled=3,
        
    }

    public enum TodoTag
    {
        HouseWork=0,
        Trainning=1,
        Abc=2,
    }
    
}