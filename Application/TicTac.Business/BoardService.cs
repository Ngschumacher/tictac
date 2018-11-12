using System;
using System.Collections.Generic;
using System.Linq;
using TicTac.Core.Interfaces;
using TicTac.Core.Models;
using TicTac.DataAccess;

namespace TicTac.Business {
    public class GameService : IGameService {
        private readonly IGameRepository _gameRepository;


        public GameService(IGameRepository gameRepository) {
            _gameRepository = gameRepository;
        }

        public GameModel NewGame(int player1Id, int player2Id) {
            var game = _gameRepository.CreateGame(player1Id, player2Id);

            return GenerateGameModel(game);
        }

        public GameModel MakeMove(int gameId, int playerId, int move) {
            var positionHorisontal = move % 3;
            var positionVertial = (move / 3 - positionHorisontal) + move % 3;

            var game = _gameRepository.GetGame(gameId);
            if (game == null)
                return null;

            var currentTurnUser = CurrentTurnUser(game);

            if (currentTurnUser.Id != playerId)
                throw new Exception("Not your turn");


            //couldn't get moves out from the game, so retrieving it here. 

            var existingMove = game.Moves.Any(x =>
                x.PositionHorisontal == positionHorisontal && x.PositionVertical == positionVertial);
            if (existingMove)
                throw new Exception("Position already taken");

            var currentMove = new Move() {
                GameId = gameId, PlayerId = playerId, PositionHorisontal = positionHorisontal,
                PositionVertical = positionVertial
            };

            game.Moves.Add(currentMove);
            _gameRepository.SaveGame(game);

            var gameModel = GenerateGameModel(game);
            return gameModel;
        }

        public GameModel GetGame(int gameId) {
            var game = _gameRepository.GetGame(gameId);
            if (game == null)
                return null;

            return GenerateGameModel(game);
        }

        public List<GameStats> GetStatsVersusOpponent(int userId, int opponentId) {
            List<Game> games = _gameRepository.GetGamesVersusOpponenet(userId, opponentId);
            List<GameStats> stats = new List<GameStats>();


            foreach (var game in games) {
                var board = GenerateBoard(game);
                var gameDecider = new GameDecider(board);
                User winner = null;

                if (gameDecider.Ended && !gameDecider.IsDraw) {
                    winner = game.Moves.Last().PlayerId == game.Player1.Id ? game.Player1 : game.Player2;
                }

                var stat = new GameStats() {
                    Id = game.Id,
                    Player1 = game.Player1,
                    Player2 = game.Player2,
                    Winner = winner,
                    Ended = gameDecider.Ended
                };
                stats.Add(stat);
            }

            return stats;
        }

        private GameModel GenerateGameModel(Game game) {
            var board = GenerateBoard(game);

            var gameDecider = new GameDecider(board);
            var whosNext = CurrentTurnUser(game);

            var gameStatus = new GameStatus();
            if (gameDecider.Ended) {
                gameStatus.GameEnded = gameDecider.Ended;

                if (!gameDecider.IsDraw) {
                    var winner = whosNext == game.Player1 ? game.Player2 : game.Player1;
                    gameStatus.Winner = winner;
                }
            }

            GameModel gameModel = new GameModel() {
                Id = game.Id,
                Player1 = game.Player1,
                Player2 = game.Player2,
                WhosNext = whosNext,
                Board = board,
                GameStatus = gameStatus
            };

            return gameModel;
        }

        private Board GenerateBoard(Game game) {
            var board = new Board();

            foreach (var m in game.Moves) {
                var boardPosition = m.PositionVertical * 3 + m.PositionHorisontal;
                var firstPlayer = m.PlayerId.Equals(game.Player1.Id);

                board.Positions[boardPosition] = firstPlayer ? 'x' : 'o';
            }
            return board;
        }

        private User CurrentTurnUser(Game game) {
            var startingPlayerId = game.StartingPlayerId;
            var startingPlayer = startingPlayerId == game.Player1.Id ? game.Player1 : game.Player2;
            var notStartingPlayer = startingPlayerId != game.Player1.Id ? game.Player1 : game.Player2;

            var whosNext = game.Moves.Count % 2 == 0 ? startingPlayer : notStartingPlayer;

            return whosNext;
        }
    }
}