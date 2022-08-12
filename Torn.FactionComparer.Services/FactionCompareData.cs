namespace Torn.FactionComparer.Services
{
    public class FactionCompareImageData
    {
        public FactionCompareData FirstFaction { get; set; }
        public FactionCompareData SeccondFaction { get; set; }
    }

    public class FactionCompareData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Respect { get; set; }
        public int BestChain { get; set; }
        public int Members { get; set; }
        public double AverageAge { get; set; }
        public double AverageLevel { get; set; }
        public TimeSpan AverageActivity { get; set; }
        public TimeSpan AverageActivityPerDay { get; set; }
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
    }
}
