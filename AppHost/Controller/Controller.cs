using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoAppApi1.Application.Login.Commands.Login;
using TodoAppApi1.Application.TodoItems.Commands.CreateTodoItem;
using TodoAppApi1.Application.TodoItems.Commands.DeleteTodoItem;
using TodoAppApi1.Application.TodoItems.Commands.UpdateTodoItem;
using TodoAppApi1.Application.TodoLists.Commands.CreateTodoList;
using TodoAppApi1.Application.TodoLists.Commands.DeleteTodoList;
using TodoAppApi1.Application.TodoLists.Commands.UpdateTodoList;


namespace TodoAppApi1.AppHost.Controller
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoListController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteTodoItemCommand(id));
            return NoContent(); // HTTP 204
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id trong URL không khớp với Id trong body");

            try
            {
                await _mediator.Send(command);
                return NoContent(); // HTTP 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Trả về message cụ thể trong exception
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi không xác định khác
                return StatusCode(500, new { error = "Đã xảy ra lỗi không xác định", detail = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateTodoListCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            await _mediator.Send(new DeleteTodoListCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateList([FromBody] UpdateTodoListCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
    
}
