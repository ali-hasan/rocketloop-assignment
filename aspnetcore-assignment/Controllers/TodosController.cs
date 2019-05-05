using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;
using aspnetcore_assignment.Hubs;
using aspnetcore_assignment.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace aspnetcore_assignment.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController :ControllerBase {

        private readonly ITodoService _todoService;
        private readonly INotificationsHub _notificationHub;
        private readonly ILogger _logger;
        public TodosController(ITodoService todoService, INotificationsHub notificationHub, ILogger<TodosController> logger) {
            _todoService = todoService;
            _notificationHub = notificationHub;
            _logger = logger;
        }

        /// <summary>
        /// Gets all TODOs
        /// </summary>
        /// <returns>Todo[]</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            try
            {
                var todos = await _todoService.GetAllTodos();
                return Ok(todos);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while retrieving All TODOs on TodosController at fucntions Index()", exception);
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("incomplete")]
        public async Task<IActionResult> GetIncompleteTodos() {
            try
            {
                var todos = await _todoService.GetIncompleteTodos();
                return Ok(todos);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while retrieving All TODOs on TodosController at fucntions GetIncompleteTodos()", exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Creates a new TODO
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>The TODO</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTodo([FromBody] Todo todo) {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                todo.Created = DateTime.UtcNow;
                var result = await _todoService.AddTodo(todo);

                // Notify subsribers about todo creation
                await _notificationHub.TodoNotification(result);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while creating a TODO on TodosController at fucntions CreateTodo({todo})", todo, exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Updates an already existing TODO
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todo"></param>
        /// <returns>The TODO</returns>
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] Todo todo) {
            try
            {
                if(string.IsNullOrEmpty(id.ToString()) || !ModelState.IsValid || id != todo.Id)
                    return BadRequest($"{ModelState}");
                
                var existingTodo = await _todoService.GetTodo(id);
                if(existingTodo == null)
                    return NotFound($"There is no TODO for this ID {id}");
                
                // update title, status and update time of existing Todo
                existingTodo.Title = todo.Title;
                existingTodo.Done = todo.Done;
                existingTodo.Update = DateTime.UtcNow;

                var result = await _todoService.UpdateTodo(existingTodo);

                // Notify subsribers about todo updation
                await _notificationHub.TodoNotification(result);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while updating a TODO on TodosController at fucntions UpdateTodo({id}, {todo})", id, todo, exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Deltes an existing TODO
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True/False</returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id) {
            try
            {
                if(string.IsNullOrEmpty(id.ToString()))
                    return BadRequest($"ID can not be empty");
                
                var todo = await _todoService.GetTodo(id);
                if(todo == null)
                    return NotFound($"There is no TODO for this ID {id}");
                
                var result = await _todoService.DeleteTodo(todo);
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while deleting a TODO on TodosController at fucntions DeleteTodo({id})", id, exception);
                return BadRequest(exception.Message);
            }
        }
    }
}
