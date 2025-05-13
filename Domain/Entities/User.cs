using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAppApi1.Domain.Common;

namespace TodoAppApi1.Domain.Entities;
[Table("users")]
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}