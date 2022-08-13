using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Torn.FactionComparer.Infrastructure.Tables
{
    [Table("FactionCompareData")]
    public class FactionCompareDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FactionID { get; set; }
        public string Name { get; set; }
        public int Respect { get; set; }
        public int BestChain { get; set; }
        public int Members { get; set; }
        public double AverageAge { get; set; }
        public double AverageLevel { get; set; }
        public double AverageActivity { get; set; }
        public double AverageActivityPerDay { get; set; }
        public double AverageAwards { get; set; }
        public int Xanax { get; set; }
        public int LSD { get; set; }
        public int Vicodin { get; set; }
        public int Overdoses { get; set; }
        public int EnergyRefils { get; set; }
        public int EnergyDrinks { get; set; }
        public int Boosters { get; set; }
        public int StatEnhancers { get; set; }
        public int AttacksMade { get; set; }
        public int RevivesGiven { get; set; }
        public int RevivesReceived { get; set; }
        public int BountiesPlaced { get; set; }
        public int BountiesReceived { get; set; }
        public int MissionCredits { get; set; }
        public int SpecialAmmo { get; set; }
        public int JobPoints { get; set; }
        public int RespectEarned { get; set; }
        public long Networth { get; set; }
        public int Karma { get; set; }
        public int BooksRead { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
