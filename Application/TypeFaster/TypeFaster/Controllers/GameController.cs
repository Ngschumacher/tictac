using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using TypeFaster.Business;
using TypeFaster.Business.Context;
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

        [HttpGet("SendChallenge")]
        public async Task<IActionResult> SendChallenge(int challengerId, int opponentId)
        {

            var connections = Users.Connections.GetConnections(opponentId).ToList();

            var game = _boardService.NewGame(challengerId, opponentId);

            var challenge = new ChallengeViewModel()
            {
                ChallengerName = game.Player1.Username,
                GameId = game.Id
            };
            
            await _hubContext.Clients.Clients(connections).SendAsync("challengeRecieved", challenge);

            return Ok();
        }


        [HttpGet("AcceptChallenge")]
        public async Task<IActionResult> AcceptChallenge(int accepterId, int gameId)
        {
            var game = _boardService.GetGame(gameId);

            
            var connections = Users.Connections.GetItems(new List<int>() {game.Player1Id, game.Player2Id })
                                    .SelectMany(x => x.Value).ToList();

            var gameViewModel = new GameInformationViewModel()
            {
                Game = game,
                Board = new Board()
            };
            
            await _hubContext.Clients.Clients(connections).SendAsync("gameStarting",gameViewModel );
             
            return Ok();
        }
        
        [HttpGet("NewGame")]
        public ActionResult NewGame()
        {
                var game = _boardService.NewGame(5, 6);
            
                var gameViewModel = new GameInformationViewModel()
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
            //wrong winner

           

            var player = game.Player1Id == game.CurrentTurn ? game.Player1 : game.Player2;

            var ended = gameDecider.Ended;
            var gameStatus = new GameStatusViewModel()
            {
                GameEnded = gameDecider.Ended,
                Winner = ended ? player : null,
                
            };
            
            var gameViewModel = new GameInformationViewModel()
            {
                Game = game,
                Board = board,
                GameStatus = gameStatus
            };
            var connections = Users.Connections.GetItems(new List<int>() {game.Player1Id, game.Player2Id })
                .SelectMany(x => x.Value).ToList();
            
            _hubContext.Clients.Clients(connections).SendAsync("updateBoard", gameViewModel);

            
            return Ok(gameViewModel);
        }


    }
}