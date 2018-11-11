using System;
using System.Data.Entity;
using TypeFaster.Models;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TypeFaster.Business.Context;
using TypeFaster.Core.Interfaces;
using TypeFaster.Core.Models;

namespace TypeFaster.Business
{
    public class BoardService : IBoardService
    {
        private readonly GameContext _gameContext;
        
        
        public BoardService(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        

        public Game NewGame(int player1Id, int player2Id)
        {

            using (var context = new GameContext())
            {
                
                var  currentUser = context.User.FirstOrDefault(x => x.Id == player1Id);
                var aiUser = context.User.FirstOrDefault(x => x.Id == player2Id);

                var player1 = currentUser;
                var player2 = aiUser;
                
                Random rnd = new Random();
                var game = new Game()
                {
                    Player1Id = player1.Id,
                    Player2Id = player2.Id,
                    CurrentTurn = (rnd.Next(0, 1) == 0) ? player1.Id : player2.Id,
                };
                _gameContext.Game.Add(game);

                _gameContext.SaveChanges();

                game.Player1 = player1;
                game.Player2 = player2;
                return game;

            }

            return null;
        }
        
        public Board MakeMove(int gameId, int playerId, int move)
        {
            var board = new Board()
            {
            };
            
            var positionHorisontal = move % 3;
            var positionVertial = (move / 3 - positionHorisontal) + move % 3;

            using (var context = new GameContext())
            {
                var game = context.Game.FirstOrDefault(x => x.Id == gameId);
                if (game == null)
                    return null;

                if (game.CurrentTurn != playerId)
                {
                    throw new Exception("Not your turn");
                }

                //couldn't get moves out from the game, so retrieving it here. 
                var moves = context.Move.Where(m => m.GameId == game.Id).ToList();

                var existingMove = moves.Any(x => x.PositionHorisontal == positionHorisontal && x.PositionVertical == positionVertial);
                if (existingMove)
                    throw new Exception("Position already taken");

                var currentMove = new Move()
                {
                    GameId = gameId, PlayerId = playerId, PositionHorisontal = positionHorisontal,
                    PositionVertical = positionVertial
                };

                moves.Add(currentMove);
                game.Moves = moves;
                context.SaveChanges();

                foreach (var m in game.Moves)
                {
                    var boardPosition = m.PositionVertical * 3 + m.PositionHorisontal;
                    var firstPlayer = m.PlayerId == game.Player1Id;
                    
                    board.Positions[boardPosition] = firstPlayer ? 'x' : 'o';
                }
                
                var gameDecider = new GameDecider(board);
                game.CurrentTurn = playerId == game.Player1Id ? game.Player2Id : game.Player1Id;

                game
                
                

                return board;
            }
        }
        
        public Game GetGame(int id)
        {
            using (var context = new GameContext())
            {
                var game =  _gameContext.Game
                    .Include(g => g.Player1)
                    .Include(g => g.Player2)
                    .FirstOrDefault(x => x.Id == id);

                var player1 = _gameContext.User.FirstOrDefault(x => x.Id == game.Player1Id);
                var player2 = _gameContext.User.FirstOrDefault(x => x.Id == game.Player2Id);

                game.Player1 = player1;
                game.Player2 = player2;


                return game;
            }
        }

    }
}