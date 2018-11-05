using TypeFaster.Core.Models;
using TypeFaster.Models;
using Xunit;

namespace TypeFaster.Business.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GameDecider_initialBoard()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    ' ', ' ', ' ',
                    ' ', ' ', ' ',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);

            var ended = decider.Ended;

            Assert.False(ended);
        }
        
        [Fact]
        public void GameDecider_TwoXInTop()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    'x', 'x', ' ',
                    ' ', ' ', 'o',
                    ' ', 'o', ' '
                }
            };

            var decider = new GameDecider(game);

            var ended = decider.Ended;

            Assert.False(ended);
        }
        
        [Fact]
        public void GameDecider_xTopRow()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    'x', 'x', 'x',
                    ' ', ' ', ' ',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            
            Assert.True(ended);
        }
        
        [Fact]
        public void GameDecider_oMidRow()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    ' ', ' ', ' ',
                    'o', 'o', 'o',
                    ' ', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            
            Assert.True(ended);
        }
        
        [Fact]
        public void GameDecider_oBotRow()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    ' ', ' ', ' ',
                    ' ', ' ', ' ',
                    'o', 'o', 'o'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            
            Assert.True(ended);
        }
        
        [Fact]
        public void GameDecider_oCrossLeftTop()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    'o', ' ', ' ',
                    ' ', 'o', ' ',
                    ' ', ' ', 'o'
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            
            Assert.True(ended);
        }
        
        [Fact]
        public void GameDecider_oCrossRightTop()
        {
            var game = new Board()
            {
                Positions = new char[9]
                {
                    ' ', ' ', 'o',
                    ' ', 'o', ' ',
                    'o', ' ', ' '
                }
            };

            var decider = new GameDecider(game);
            var ended = decider.Ended;
            
            Assert.True(ended);
        }
    }
}