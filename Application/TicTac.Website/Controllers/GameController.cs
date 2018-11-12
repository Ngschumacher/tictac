using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TicTac.Core.Interfaces;
using TicTac.Core.Models;
using TicTac.Website.Hubs;
using TicTac.Website.Hubs.Models;

namespace TicTac.Website.Controllers {
    [Route("api/[controller]")]
    public class GameController : ControllerBase {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IGameService _gameService;

        public GameController(IHubContext<ChatHub> hubContext, IGameService gameService) {
            _hubContext = hubContext;
            _gameService = gameService;
        }

        [HttpGet("GetStats")]
        public ActionResult GetStats(int userId, int opponentId) {
            var game = _gameService.GetStatsVersusOpponent(userId, opponentId);
            return Ok(game);
        }

        [HttpGet("NewGame")]
        public ActionResult NewGame() {
            var game = _gameService.NewGame(5, 6);

            return Ok(game);
        }

        [HttpGet("MakeMove")]
        public IActionResult MakeMove(int gameId, int playerId, int move) {
            if (move > 8)
                return StatusCode(500, "position out of board");

            if (playerId == 0)
                return StatusCode(500, "Please provide playerId");

            GameModel game;
            try {
                game = _gameService.MakeMove(gameId, playerId, move);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }

            var connections = Users.Connections.GetItems(new List<int>() {game.Player1.Id, game.Player2.Id})
                .SelectMany(x => x.Value).ToList();

            _hubContext.Clients.Clients(connections).SendAsync("updateBoard", game);


            return Ok(game);
        }

        [HttpGet("SendChallenge")]
        public async Task<IActionResult> SendChallenge(int challengerId, int opponentId) {
            var game = _gameService.NewGame(challengerId, opponentId);

            var challenge = new ChallengeViewModel() {
                ChallengerName = game.Player1.Username,
                GameId = game.Id
            };

            var connections = Users.Connections.GetConnections(opponentId).ToList();
            await _hubContext.Clients.Clients(connections).SendAsync("challengeRecieved", challenge);

            return Ok();
        }


        [HttpGet("AcceptChallenge")]
        public async Task<IActionResult> AcceptChallenge(int gameId) {
            GameModel game = _gameService.GetGame(gameId);
            
            var connections = Users.Connections.GetItems(new List<int>() {game.Player1.Id, game.Player2.Id})
                .SelectMany(x => x.Value).ToList();

            await _hubContext.Clients.Clients(connections).SendAsync("gameStarting", game);
            return Ok();
        }
    }
}