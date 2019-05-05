using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;
using aspnetcore_assignment.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_assignment.Services
{
    public class BoardService : IBoardService
    {
        private readonly ApiDbContext _context;
        public BoardService(ApiDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Gets all boards from DB
        /// </summary>
        /// <returns>Boards[]</returns>
        public async Task<IEnumerable<Board>> GetBoards() {
            //  TODOs can be included in call if required
            // await _context.Boards.Include(b => b.Todos).ToListAsync();

            var boards = await _context.Boards.ToListAsync();
            return boards;
        }

        /// <summary>
        /// Get single board on base of its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Board</returns>
        public async Task<Board> GetBoard(Guid id) {
            var board = await _context.Boards.FindAsync(id);
            return board;
        }

        /// <summary>
        /// Add new board to DB
        /// </summary>
        /// <param name="board"></param>
        /// <returns>New added board</returns>
        public async Task<Board> AddNewBoard(Board board) {
            var result = _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Updates an already existing board
        /// </summary>
        /// <param name="id"></param>
        /// <param name="board"></param>
        /// <returns>Updated Board</returns>
        public async Task<Board> UpdateBoard(Board board) {
            _context.Entry(board).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return board;
        }

        /// <summary>
        /// Deletes an existing board
        /// </summary>
        /// <param name="board"></param>
        /// <returns>True/False</returns>
        public async Task<bool> DeleteBoard(Board board) {
            _context.Boards.Remove(board);
            var result = await _context.SaveChangesAsync();
            return true;
        } 
    }
}