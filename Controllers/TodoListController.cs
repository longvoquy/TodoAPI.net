using Microsoft.AspNetCore.Mvc;
using TodoAppApi1.Data;
using TodoAppApi1.Models;
using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Entities;
namespace TodoAppApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private  ApiContext _context;

        public TodoListController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult CreateEdit(TodoList todolist)
        {
            try
            {
                if (todolist.Id == 0)
                {
                    // Thêm mới
                    todolist.CreatedAt = DateTime.UtcNow;
                    todolist.UpdatedAt = DateTime.UtcNow;
                    _context.TodoLists.Add(todolist);
                }
                else
                {
                    // Cập nhật
                    var todoListInDb = _context.TodoLists.Find(todolist.Id);
                    if (todoListInDb == null)
                    {
                        return new JsonResult(NotFound());
                    }

                    todoListInDb.Task = todolist.Task;
                    todoListInDb.Status = todolist.Status;
                    todoListInDb.Tag = todolist.Tag;
                    todoListInDb.UpdatedAt = DateTime.UtcNow;
                }

                _context.SaveChanges();

                return new JsonResult(new
                {
                    todolist.Id,
                    todolist.Task,
                    status = todolist.Status.ToString(),
                    tag = todolist.Tag.ToString(),
                    createdAt = todolist.CreatedAt,
                    updatedAt = todolist.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                Console.WriteLine(ex.InnerException?.Message);
                return new JsonResult(BadRequest("Failed to save data"));
            }
        }


        [HttpGet]
        public JsonResult GetById(int id)
        {
            var result = _context.TodoLists.Find(id);
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(RenderObject(result));
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var todo = _context.TodoLists.Find(id);
            if (todo == null)
                return new JsonResult("TodoList ID not found");

            _context.TodoLists.Remove(todo);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.TodoLists.ToList();
            var rendered = result.Select(RenderObject).ToList();
            return new JsonResult(rendered);
        }

        [HttpGet]
        public JsonResult GetByTask(string task)
        {
            var result = _context.TodoLists
                .Where(t => t.Task != null && t.Task.Contains(task))
                .ToList();

            if (!result.Any())
                return new JsonResult("TodoList get by Task not found");

            var rendered = result.Select(RenderObject).ToList();
            return new JsonResult(rendered);
        }

        private static object RenderObject(TodoList todo)
        {
            return new
            {
                todo.Id,
                todo.Task,
                status = todo.Status.ToString(),
                tag = todo.Tag.ToString(),
                createdAt = todo.CreatedAt,
                updatedAt = todo.UpdatedAt
            };
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                
                if (string.IsNullOrEmpty(request.Email))
                        return BadRequest("Email request");
                
                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest("Password request");
                
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);
        
                if (user == null)
                    return NotFound("No User");

                
                if (user.Password != request.Password)
                    return Unauthorized("Wrong password");
                
                var response = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    Message = "Login success"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "System Error");
            }
        }

    }
}
