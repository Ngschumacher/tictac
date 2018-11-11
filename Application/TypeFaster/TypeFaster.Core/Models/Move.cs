using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TypeFaster.Core.Models
{
    public class Move
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("gameId")]
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int PositionVertical { get; set; }
        public int PositionHorisontal { get; set; }
        public int PlayerId { get; set; }
    }
}