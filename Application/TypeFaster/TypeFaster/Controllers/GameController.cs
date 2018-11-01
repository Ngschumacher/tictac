using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using TypeFaster.Models;

namespace TypeFaster.Controllers
{
    [Route("api/[controller]")]
    public class GameController: ControllerBase
    {
        public GameController()
        {
            
            
        }
        
        [HttpGet("NewGame")]
        public ActionResult NewGame()
        {
            using (var context = new GameContext())
            {
                var aiUser = new User() {Username = "AI User"};
                context.User.Add(aiUser);
                var currentUser = new User() {Username = "Niclas"};
                context.User.Add(currentUser);
                
                Random rnd = new Random();
                var game = new Game()
                {
                    Player1 = currentUser,
                    Player2 = aiUser,
                    CurrentTurn = (rnd.Next(0,1) == 0) ? currentUser.Id : aiUser.Id
                };
                context.Game.Add(game);

                context.SaveChanges();
            }
            
            return new EmptyResult();
        }

        [HttpGet("MakeMove")]
        public IActionResult MakeMove(int gameId, int playerId, int move)
        {
            if (move > 8)
                return StatusCode(500, "position out of board");
            
            if (playerId == 0)
                return StatusCode(422, "Please provide playerId");
            
            var positionHorisontal = move % 3;
            var positionVertial = (move / 3 - positionHorisontal) + move % 3;

            var positions = new string[9]
            {
                " ", " ", " ",
                " ", " ", " ",
                " ", " ", " "
            };;
            
            
            using (var context = new GameContext())
            {
                var game = context.Game.FirstOrDefault(x => x.Id == gameId);
                if (game == null)
                    return NotFound("The gameId was not found");

//                if (game.Player1Id != playerId && game.Player2Id != playerId)
//                    return StatusCode(404, "PlayerId does not match");

                if (game.CurrentTurn != playerId)
                {
                    return StatusCode(404, "Not your turn");
                }

                    
                //couldn't get moves out from the game, so retrieving it here. 
                var moves = context.Move.Where(m => m.GameId == game.Id).ToList();

                var existingMove = moves.Any(x => x.PositionHorisontal == positionHorisontal && x.PositionVertical == positionVertial);
                if (existingMove)
                    return StatusCode(500, "Position already taken");
                
                var currentMove = new Move()
                {
                    GameId = gameId, PlayerId = playerId, PositionHorisontal = positionHorisontal,
                    PositionVertical = positionVertial
                };
                game.CurrentTurn = playerId == game.Player1Id ? game.Player2Id : game.Player1Id;

                moves.Add(currentMove);
                game.Moves = moves;
                context.SaveChanges();

                foreach (var m in game.Moves)
                {
                    var boardPosition = m.PositionVertical * 3 + m.PositionHorisontal;
                    var firstPlayer = m.PlayerId == game.Player1Id;
                    
                    positions[boardPosition] = firstPlayer ? "x" : "o";
                }
                
                
                
            }






            var board = new Board()
            {
                Positions = positions
            };

            return Ok(board);
        }
    }
}