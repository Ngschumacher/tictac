using System.Linq;
using TicTac.Core.Models;

namespace TicTac.Business {
    public class GameDecider {
        private Board _board;
        public bool Ended { get; private set; }
        public bool IsDraw { get; private set; }

        public GameDecider(Board board) {
            _board = board;
            IsDraw = !board.Positions.Any(x => x.Equals(' '));
            Ended = HasWinner() || IsDraw;
        }

        private char GetElement(int x, int y) {
            return _board.Positions[3 * y + x];
        }

        private bool HasWinner() {
            for (int i = 0; i < 3; i++) {
                if (GetElement(i, 0) != ' ' &&
                    GetElement(i, 0) == GetElement(i, 1) &&
                    GetElement(i, 1) == GetElement(i, 2)) {
                    return true;
                }

                if (GetElement(0, i) != ' ' &&
                    GetElement(0, i) == GetElement(1, i) &&
                    GetElement(1, i) == GetElement(2, i)) {
                    return true;
                }
            }

            if (GetElement(0, 0) != ' ' &&
                GetElement(0, 0) == GetElement(1, 1) &&
                GetElement(1, 1) == GetElement(2, 2)) {
                return true;
            }

            if (GetElement(2, 0) != ' ' &&
                GetElement(2, 0) == GetElement(1, 1) &&
                GetElement(1, 1) == GetElement(0, 2)) {
                return true;
            }

            return false;
        }
    }
}