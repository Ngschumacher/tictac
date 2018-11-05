using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization.Policy;

namespace TypeFaster.Models
{
    public class Board
    {
        public char[] Positions { get; set; }
        
        public char getElt(int x, int y) {
            return Positions[3 * y + x];
        }

        public bool Ended { get; set; }
        
        public char checkEnd() {
            for (int i = 0; i < 3; i++) {
                if (getElt(i, 0) != ' ' && 
                    getElt(i, 0) == getElt(i, 1)  &&  
                    getElt(i, 1) == getElt(i, 2)) {
                    Ended = true;
                    return getElt(i, 0);
                }

                if (getElt(0, i) != ' ' && 
                    getElt(0, i) == getElt(1, i)  &&  
                    getElt(1, i) == getElt(2, i)) {
                    Ended = true;
                    return getElt(0, i);
                }
            }

            if (getElt(0, 0) != ' '  &&  
                getElt(0, 0) == getElt(1, 1)  &&  
                getElt(1, 1) == getElt(2, 2)) {
                Ended = true;
                return getElt(0, 0);
            }

            if (getElt(2, 0) != ' '  &&  
                getElt(2, 0) == getElt(1, 1)  &&  
                getElt(1, 1) == getElt(0, 2)) {
                Ended = true;
                return getElt(2, 0);
            }

            for (int i = 0; i < 9; i++) {
                if (Positions[i] == ' ')
                    return ' ';
            }

            return 'T';
        }
    }

    public class Tile
    {
        public string Block { get; set; }
        public TYPE Type { get; set; }
    }
    
    
    public class MakeMoveReponse {
        public bool GameIsOngoing { get; set; }
        public Board Board { get; set; }
    }
}