using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;

namespace aspnetcore_assignment.ServicesInterfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<Todo> GetTodo(Guid id);
        Task<IEnumerable<Todo>> GetIncompleteTodos();
        Task<Todo> AddTodo(Todo todo);
        Task<Todo> UpdateTodo(Todo todo);
        Task<bool> DeleteTodo(Todo todo);
    }
}