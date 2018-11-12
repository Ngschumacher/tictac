using NUnit.Framework;
using TicTac.Core.Models;

namespace TicTac.Business.Test {
    public class GameDeciderTest {
        [Test]
        public void GameDecider_initialBoard() {
            var game = new Board() {
                Positions = new char[9] {
                    ' ', ' ', ' ',
                    ' ', ' ', ' ',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);

            var ended = decider.Ended;

            Assert.False(ended);
        }

        [Test]
        public void GameDecider_TwoXInTop() {
            var game = new Board() {
                Positions = new char[9] {
                    'x', 'x', ' ',
                    ' ', ' ', 'o',
                    ' ', 'o', ' '
                }
            };

            var decider = new GameDecider(game);

            var ended = decider.Ended;

            Assert.False(ended);
        }

        [Test]
        public void GameDecider_xTopRow() {
            var game = new Board() {
                Positions = new char[9] {
                    'x', 'x', 'x',
                    ' ', ' ', ' ',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;

            Assert.True(ended);
        }

        [Test]
        public void GameDecider_oMidRow() {
            var game = new Board() {
                Positions = new char[9] {
                    ' ', ' ', ' ',
                    'o', 'o', 'o',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;

            Assert.True(ended);
        }

        [Test]
        public void GameDecider_oBotRow() {
            var game = new Board() {
                Positions = new char[9] {
                    ' ', ' ', ' ',
                    ' ', ' ', ' ',
                    'o', 'o', 'o'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;

            Assert.True(ended);
        }

        [Test]
        public void GameDecider_oCrossLeftTop() {
            var game = new Board() {
                Positions = new char[9] {
                    'o', ' ', ' ',
                    ' ', 'o', ' ',
                    ' ', ' ', 'o'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;

            Assert.True(ended);
        }

        [Test]
        public void GameDecider_oCrossRightTop() {
            var game = new Board() {
                Positions = new char[9] {
                    ' ', ' ', 'o',
                    ' ', 'o', ' ',
                    'o', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;

            Assert.True(ended);
        }

        [Test]
        public void GameDecider_allTilesUsed_noWinner() {
            var game = new Board() {
                Positions = new char[9] {
                    'x', 'o', 'o',
                    'o', 'x', 'x',
                    'o', 'x', 'o'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            var isDraw = decider.IsDraw;

            Assert.True(ended);
            Assert.True(isDraw);
        }

        [Test]
        public void GameDecider_allTilesUsed_WithWinner() {
            var game = new Board() {
                Positions = new char[9] {
                    'x', 'o', 'o',
                    'o', 'x', 'o',
                    'o', 'x', 'x'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            var isDraw = decider.IsDraw;

            Assert.True(ended);
            Assert.False(isDraw);
        }
    }
}