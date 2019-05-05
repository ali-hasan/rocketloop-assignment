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
    public class BoardsController :ControllerBase {

        private readonly IBoardService _boardService;
        private readonly INotificationsHub _notificationHub;
        private readonly ILogger _logger;
        public BoardsController(IBoardService boardService, INotificationsHub notificationHub, ILogger<BoardsController> logger) {
            _boardService = boardService;
            _notificationHub = notificationHub;
            _logger = logger;
        }

        /// <summary>
        /// Gets all boards
        /// </summary>
        /// <returns>Boards[]</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            try
            {
                var boards = await _boardService.GetBoards();
                return Ok(boards);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while retrieving All boards on BoardsController at fucntions Index()", exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Creates a new board
        /// </summary>
        /// <param name="board"></param>
        /// <returns>The Board</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBoard([FromBody] Board board) {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var result = await _boardService.AddNewBoard(board);

                // Notify subsribers about board creation
                await _notificationHub.BoardNotification(result);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while creating a board on BoardsController at fucntions CreateBoard({board})", board, exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Updates an already existing Board
        /// </summary>
        /// <param name="id"></param>
        /// <param name="board"></param>
        /// <returns>The Board</returns>
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBoard(Guid id, [FromBody] Board board) {
            try
            {
                if(string.IsNullOrEmpty(id.ToString()) || !ModelState.IsValid || id != board.Id)
                    return BadRequest($"{ModelState}");
                
                var existingBoard = await _boardService.GetBoard(id);
                if(existingBoard == null)
                    return NotFound($"There is no board for this ID {id}");
                
                // update Title of exisiting Board
                existingBoard.Title = board.Title;

                var result = await _boardService.UpdateBoard(existingBoard);

                // Notify subsribers about board updation
                await _notificationHub.BoardNotification(result);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while updating a board on BoardsController at fucntions UpdateBoard({id}, {board})", id, board, exception);
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Deltes an existing Board
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True/False</returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBoard(Guid id) {
            try
            {
                if(string.IsNullOrEmpty(id.ToString()))
                    return BadRequest($"ID can not be empty");
                
                var board = await _boardService.GetBoard(id);
                if(board == null)
                    return NotFound($"There is no board for this ID {id}");
                
                var result = await _boardService.DeleteBoard(board);
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while deleting a board on BoardsController at fucntions DeleteBoard({id})", id, exception);
                return BadRequest(exception.Message);
            }
        }
    }
}
