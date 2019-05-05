using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;
using aspnetcore_assignment.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_assignment.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApiDbContext _context;
        public TodoService(ApiDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Gets add available TODOs
        /// </summary>
        /// <returns>TODO[]</returns>
        public async Task<IEnumerable<Todo>> GetAllTodos() {
            var todos = await _context.Todos.ToListAsync();
            return todos;
        }

        /// <summary>
        /// Get TODO on base of its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The TODO</returns>
        public async Task<Todo> GetTodo(Guid id) {
            var todo = await _context.Todos.FindAsync(id);
            return todo;
        }

        /// <summary>
        /// Gets incomplete (marked as NOT Done) TODOs
        /// </summary>
        /// <returns>TODO[]</returns>
        public async Task<IEnumerable<Todo>> GetIncompleteTodos() {
            var todos = await _context.Todos.Where(td => !td.Done).ToListAsync();
            return todos;
        }

        /// <summary>
        /// Add a new TODO to DB
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>The newly added TODO</returns>
        public async Task<Todo> AddTodo(Todo todo) {
            var result = _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Updates the existing TODO
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>The updatde TODO</returns>
        public async Task<Todo> UpdateTodo(Todo todo) {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }

        /// <summary>
        /// Deletes the existing TODO from DB
        /// </summary>
        /// <param name="todo"></param>
        /// <returns>True/False</returns>
        public async Task<bool> DeleteTodo(Todo todo) {
            _context.Todos.Remove(todo);
            var result = await _context.SaveChangesAsync();
            return true;
        }
    }
}