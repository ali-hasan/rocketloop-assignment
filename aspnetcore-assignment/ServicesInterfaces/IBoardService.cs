using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_assignment.Models;


namespace aspnetcore_assignment.ServicesInterfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<Board>> GetBoards();
        Task<Board> GetBoard(Guid id);
        Task<Board> AddNewBoard(Board board);
        Task<Board> UpdateBoard(Board board);
        Task<bool> DeleteBoard(Board board);
    }
}