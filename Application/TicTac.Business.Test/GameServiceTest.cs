using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicTac.Core.Interfaces;
using TicTac.Core.Models;
using TicTac.DataAccess;

namespace TicTac.Business.Test {
    public class GameServiceTest {
        public class Scope {
            public Mock<IGameRepository> GameRepository { get; set; }
            public GameService GameService { get; set; }
        }

        private Scope _scope;

        private User Player1 = new User() {
            Id = 1,
            Username = "First User"
        };

        private User Player2 = new User() {
            Id = 2,
            Username = "Second User"
        };

        private Game Game;
        private List<Game> StatsGames;

        [SetUp]
        public void Init() {
            Game = new Game() {
                Id = 1,
                Player1 = Player1,
                Player1Id = Player1.Id,
                Player2 = Player2,
                Player2Id = Player2.Id,
                StartingPlayerId = Player1.Id
            };

            StatsGames = new List<Game>();

            _scope = new Scope() {
                GameRepository = new Mock<IGameRepository>()
            };
            _scope.GameRepository.Setup(s => s.CreateGame(Player1.Id, Player2.Id))
                .Returns(Game);

            _scope.GameRepository.Setup(s => s.GetGame(Game.Id))
                .Returns(Game);

            _scope.GameRepository.Setup(s => s.GetGamesVersusOpponenet(Player1.Id, Player2.Id))
                .Returns(StatsGames);

            _scope.GameService = new GameService(
                _scope.GameRepository.Object
            );
        }


        [Test]
        public void NewGame_ObjectTest() {
            var game = _scope.GameService.NewGame(Player1.Id, Player2.Id);

            Assert.AreEqual(Player1, game.Player1);
            Assert.AreEqual(Player2, game.Player2);
        }

        [Test]
        public void GetGame_InitialGame() {
            var game = _scope.GameService.GetGame(Game.Id);

            Assert.AreEqual(null, game.GameStatus.Winner);
            Assert.False(game.GameStatus.GameEnded);

            Assert.AreEqual(game.WhosNext, Player1);
        }

        [Test]
        public void MakeMove_FirstMove() {
            var game = _scope.GameService.MakeMove(Game.Id, Game.Player1.Id, 0);

            Assert.AreEqual(null, game.GameStatus.Winner);
            Assert.AreEqual(Player2, game.WhosNext);
        }

        [Test]
        public void MakeMove_FirstMove_WrongPlayer() {
            Assert.Throws<Exception>(() => _scope.GameService.MakeMove(Game.Id, Game.Player2.Id, 0));
        }

        [Test]
        public void MakeMove_SecondMove_allreadyTaken() {
            Game.Moves = new List<Move>() {
                new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0}
            };
            Assert.Throws<Exception>(() => _scope.GameService.MakeMove(Game.Id, Game.Player2.Id, 0));
        }

        [Test]
        public void MakeMove_SecondMove() {
            Game.Moves = new List<Move>() {
                new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0}
            };

            var game = _scope.GameService.MakeMove(Game.Id, Game.Player2.Id, 1);

            Assert.AreEqual(null, game.GameStatus.Winner);
            Assert.AreEqual(Player1, game.WhosNext);
        }

        [Test]
        public void MakeMove_WinningMove() {
            Game.Moves = new List<Move>() {
                new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0},
                new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 2},
                new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 1},
                new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 1},
            };

            var game = _scope.GameService.MakeMove(Game.Id, Game.Player1.Id, 6);

            Assert.AreEqual(Player1, game.GameStatus.Winner);
            Assert.IsTrue(game.GameStatus.GameEnded);
            Assert.AreEqual(Player2, game.WhosNext);
        }


        [Test]
        public void GenerateGameModel_ObjectTest() {
            var game = _scope.GameService.NewGame(Player1.Id, Player2.Id);

            Assert.AreEqual(Player1, game.Player1);
            Assert.AreEqual(Player2, game.Player2);
        }

        [Test]
        public void GetStatsVersusOpponent_NoMatches() {
            var games = _scope.GameService.GetStatsVersusOpponent(Player1.Id, Player2.Id);

            Assert.AreEqual(new List<GameStats>(), games);
        }

        [Test]
        public void GetStatsVersusOpponent_OneFinishedMatch() {
            StatsGames.Add(
                new Game() {
                    Id = 1,
                    Player1 = Player1,
                    Player2 = Player2,
                    Moves = new List<Move>() {
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0},
                        new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 2},
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 1},
                        new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 1},
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 2},
                    }
                }
            );

            var games = _scope.GameService.GetStatsVersusOpponent(Player1.Id, Player2.Id);

            var game = games[0];
            Assert.AreEqual(Player1, game.Winner);
        }

        [Test]
        public void GetStatsVersusOpponent_OneUnfinishedMatch() {
            StatsGames.Add(
                new Game() {
                    Id = 1,
                    Player1 = Player1,
                    Player2 = Player2,
                    Moves = new List<Move>() {
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0},
                        new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 2},
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 2},
                    }
                }
            );

            var games = _scope.GameService.GetStatsVersusOpponent(Player1.Id, Player2.Id);

            var game = games[0];
            Assert.AreEqual(null, game.Winner);
        }

        [Test]
        public void GetStatsVersusOpponent_OneEvenMatch() {
            StatsGames.Add(
                new Game() {
                    Id = 1,
                    Player1 = Player1,
                    Player2 = Player2,
                    Moves = new List<Move>() {
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 0},
                        new Move() {PlayerId = 1, PositionHorisontal = 1, PositionVertical = 0},
                        new Move() {PlayerId = 1, PositionHorisontal = 2, PositionVertical = 1},
                        new Move() {PlayerId = 1, PositionHorisontal = 0, PositionVertical = 2},
                        new Move() {PlayerId = 2, PositionHorisontal = 2, PositionVertical = 0},
                        new Move() {PlayerId = 2, PositionHorisontal = 0, PositionVertical = 1},
                        new Move() {PlayerId = 2, PositionHorisontal = 1, PositionVertical = 1},
                        new Move() {PlayerId = 2, PositionHorisontal = 1, PositionVertical = 2},
                        new Move() {PlayerId = 1, PositionHorisontal = 2, PositionVertical = 2},
                    }
                }
            );

            var games = _scope.GameService.GetStatsVersusOpponent(Player1.Id, Player2.Id);

            var game = games[0];
            Assert.AreEqual(null, game.Winner);
        }
    }
}