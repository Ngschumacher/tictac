using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using TypeFaster.Business;
using TypeFaster.Business.Context;
using TypeFaster.Business.Interfaces;
using TypeFaster.Core.Interfaces;
using TypeFaster.Core.Models;
using TypeFaster.Models;

namespace TypeFaster.Controllers
{
    [Route("api/[controller]")]
    public class GameController: ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IBoardService _boardService;
        private readonly IUserService _userService;

        public GameController(IHubContext<ChatHub> hubContext, IBoardService boardService, IUserService userService )
        {
            _hubContext = hubContext;
            _boardService = boardService;
            _userService = userService;
        }

//        [HttpGet("ChallengePlayer")]
//        public ActionResult ChallengePlayer(string challengerId, string opponent)
//        {
//            
//        }

        [HttpGet("AcceptChallenge")]
        public ActionResult AcceptChallenge()
        {
            return null;
        }
        
        [HttpGet("NewGame")]
        public ActionResult NewGame()
        {
                var game = _boardService.NewGame(5, 6);
            
                var gameViewModel = new GameViewModel()
                {
                    Game = game,
                    Board = new Board()
                };
                
                
                return Ok(gameViewModel);
            
            
            return new EmptyResult();
        }

        [HttpGet("MakeMove")]
        public IActionResult MakeMove(int gameId, int playerId, int move)
        {
            if (move > 8)
                return StatusCode(500, "position out of board");
            
            if (playerId == 0)
                return StatusCode(500, "Please provide playerId");


            Board board;
            try
            {
                board = _boardService.MakeMove(gameId, playerId, move);
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            var game = _boardService.GetGame(gameId);
            
            var gameDecider = new GameDecider(board);

           

            var player = game.Player1Id == game.CurrentTurn ? game.Player1.Username : game.Player2.Username;
            
            var gameViewModel = new GameViewModel()
            {
                Game = game,
                Board = board,
                GameEnded = gameDecider.Ended,
                Winner = game.CurrentTurn,
                WinnerName = player
            };
            return Ok(gameViewModel);
        }


//        [HttpGet("SignIn")]
//        public async Task<ActionResult> SignIn(string username)
//        {
//            var user = _userService.GetOrCreateUser(username);
//
//            var userIds = ChatHub._connections.GetKeys();
//
//            var users = _userService.GetUsers(userIds);
//            
//            await _hubContext.Clients.All.SendAsync("connections", users);
//
//            return Ok(user);
//        }
        
     
    }
}