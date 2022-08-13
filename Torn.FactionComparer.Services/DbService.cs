using Torn.FactionComparer.Infrastructure;
using Torn.FactionComparer.Infrastructure.Tables;

namespace Torn.FactionComparer.Services
{
    public interface IDbService
    {
        Task AddFactionCache(FactionCompareData data);
        Task ClearFactionCache(int factionId);
        Task<FactionCompareData> GetFactionCache(int factionId);
        Task<DateTime?> GetFactionCacheDateTime(int factionId);
    }

    public class DbService : IDbService
    {
        private readonly ITornContext _context;
        public DbService(ITornContext context)
        {
            _context = context;
        }

        public async Task AddFactionCache(FactionCompareData data)
        {
            await _context.AddFactionCache(new FactionCompareDataTable
            {
                FactionID = data.ID,
                Name = data.Name,
                Respect = data.Respect,
                BestChain = data.BestChain,
                Members = data.Members,
                AverageAge = data.AverageAge,
                AverageLevel = data.AverageLevel,
                AverageActivity = data.AverageActivity,
                AverageActivityPerDay = data.AverageActivityPerDay,
                AverageAwards = data.AverageAwards,
                Xanax = data.Xanax,
                LSD = data.LSD,
                Vicodin = data.Vicodin,
                Overdoses = data.Overdoses,
                EnergyRefils = data.EnergyRefils,
                EnergyDrinks = data.EnergyDrinks,
                Boosters = data.Boosters,
                StatEnhancers = data.StatEnhancers,
                AttacksMade = data.AttacksMade,
                RevivesGiven = data.RevivesGiven,
                RevivesReceived = data.RevivesReceived,
                BountiesPlaced = data.BountiesPlaced,
                BountiesReceived = data.BountiesReceived,
                MissionCredits = data.MissionCredits,
                SpecialAmmo = data.SpecialAmmo,
                JobPoints = data.JobPoints,
                RespectEarned = data.RespectEarned,
                Networth = data.Networth,
                Karma = data.Karma,
                BooksRead = data.BooksRead,
                TimeStamp = DateTime.UtcNow
            });
        }

        public async Task ClearFactionCache(int factionId)
        {
            await _context.ClearFactionCache(factionId);
        }

        public async Task<FactionCompareData> GetFactionCache(int factionId)
        {
            var table = await _context.GetFactionCache(factionId);
            if (table != null)
            {
                return new FactionCompareData()
                {
                    ID = table.FactionID,
                    Name = table.Name,
                    Respect = table.Respect,
                    BestChain = table.BestChain,
                    Members = table.Members,
                    AverageAge = table.AverageAge,
                    AverageLevel = table.AverageLevel,
                    AverageActivity = table.AverageActivity,
                    AverageActivityPerDay = table.AverageActivityPerDay,
                    AverageAwards = table.AverageAwards,
                    Xanax = table.Xanax,
                    LSD = table.LSD,
                    Vicodin = table.Vicodin,
                    Overdoses = table.Overdoses,
                    EnergyRefils = table.EnergyRefils,
                    EnergyDrinks = table.EnergyDrinks,
                    Boosters = table.Boosters,
                    StatEnhancers = table.StatEnhancers,
                    AttacksMade = table.AttacksMade,
                    RevivesGiven = table.RevivesGiven,
                    RevivesReceived = table.RevivesReceived,
                    BountiesPlaced = table.BountiesPlaced,
                    BountiesReceived = table.BountiesReceived,
                    MissionCredits = table.MissionCredits,
                    SpecialAmmo = table.SpecialAmmo,
                    JobPoints = table.JobPoints,
                    RespectEarned = table.RespectEarned,
                    Networth = table.Networth,
                    Karma = table.Karma,
                    BooksRead = table.BooksRead,
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<DateTime?> GetFactionCacheDateTime(int factionId)
        {
            return await _context.GetFactionCacheDateTime(factionId);
        }
    }
}
