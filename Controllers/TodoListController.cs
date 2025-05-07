using Microsoft.AspNetCore.Mvc;
using TodoAppApi1.Data;
using TodoAppApi1.Models;
//dua theo Api theo ko phai theo MVC
namespace TodoAppApi1.Controllers
{
    [Route("api/[controller]/[action]")] //setting route cho RestApi, duong dan mac dinh : api/TodoList/...
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ApiContext _context;
        //constructor
        public TodoListController(ApiContext context) 
        {
            _context = context;
        }
        
        //set endpoint for create/edit
        [HttpPost]//set request type
        public JsonResult CreateEdit(TodoList todolist)// return object as json
        {
            
            if (todolist.Id == 0)//parameter
            {
                todolist.CreatedAt = DateTime.UtcNow;
                todolist.UpdatedAt = DateTime.UtcNow;
                _context.TodoLists.Add(todolist);
            }
            else
            {
                var todoListInDb = _context.TodoLists.Find(todolist.Id);
                if (todoListInDb == null)
                    return new JsonResult(NotFound());
                //todoListInDb = todolist;
                todoListInDb.Task =todolist.Task;
                todoListInDb.Status = todolist.Status;
                todoListInDb.Tag = todolist.Tag;
                todoListInDb.UpdatedAt = DateTime.UtcNow;
            }
            string statusString = todolist.Status.ToString();
            string tagString = todolist.Tag.ToString();
            
            _context.SaveChanges();
           // return new JsonResult(Ok(todolist));
           return new JsonResult(
               new{
                   todolist.Id,
                   todolist.Task,
                   status = statusString,
                   tag = tagString,
                   createdAt = todolist.CreatedAt,
                   updatedAt = todolist.UpdatedAt
               }); 
        }
        //set endpoint for get
        [HttpGet]
        public JsonResult GetbyId(int id)
        {
            var result = _context.TodoLists.Find(id);
            if(result == null)
                return new JsonResult(NotFound());
            return new JsonResult(RenderObject(result));
        }
        //set endpoint for delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var todolist = _context.TodoLists.Find(id);
            if(todolist == null)
                return new JsonResult("TodoList ID not found");
            _context.TodoLists.Remove(todolist);
            _context.SaveChanges();
            return new JsonResult(NoContent());
        }
        
        //set endpoint for getall
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.TodoLists.ToList();
            return new JsonResult(Ok(result));
        }
        
        //set end point for search Task
        [HttpGet]
        public JsonResult GetByTask(string task)
        {
            var result = _context.TodoLists
                .Where(t => t.Task != null && t.Task.Contains(task)) // tìm tất cả, cho phép chứa chuỗi
                .ToList();

            if (!result.Any())
                return new JsonResult("TodoList get by Task not found");

            var rendered = result.Select(t => RenderObject(t)).ToList();

            return new JsonResult(rendered);
        }

        static object RenderObject(TodoList todoList)
        {
            return new
            {
                todoList.Id,
                todoList.Task,
                status = todoList.Status.ToString(),
                tag = todoList.Tag.ToString()
            };
        }
       
    }
  
}