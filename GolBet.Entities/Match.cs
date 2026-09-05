// GolBet.Entities/Match.cs
using System.ComponentModel.DataAnnotations.Schema;
using GolBet.Entities.Common;
using GolBet.Entities.Enums;

namespace GolBet.Entities;

public class Match : AuditableEntity
{
    public DateTime Date { get; set; }

    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }

    [Column(TypeName = "decimal(5,2)")] // It will have 5 digits in total, 2 of which are after the decimal point. Example: 123.45
    public decimal HomeOdds { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal DrawOdds { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal AwayOdds { get; set; }

    // Two foreign keys to the same table (Team)
    public int HomeTeamId { get; set; } // PK of Team
    public Team HomeTeam { get; set; } = null!; /* The relationship between PK and FK is defined using the name of the property,
                                                   which is of the same type as the class it references (Team).*/

    public int AwayTeamId { get; set; } // Its similar to the HomeTeamId property, but it represents the away team in the match.
    public Team AwayTeam { get; set; } = null!; /* This property will never be null; 
                                                 this represents the value used in this case: null! */
    //Navigation Property 
    public ICollection<Bet> Bets { get; set; } = new List<Bet>(); // A match can have multiple bets placed on it, so we use a collection to represent that relationship.
}
