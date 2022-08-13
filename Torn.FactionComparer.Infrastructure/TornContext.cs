using Microsoft.EntityFrameworkCore;
using Torn.FactionComparer.Infrastructure.Tables;

namespace Torn.FactionComparer.Infrastructure
{
    public interface ITornContext
    {
        Task AddFactionCache(FactionCompareDataTable factionCompareDataTable);
        Task ClearFactionCache(int factionId);
        Task<FactionCompareDataTable> GetFactionCache(int factionId);
        Task<DateTime?> GetFactionCacheDateTime(int factionId);
    }

    public class TornContext : DbContext, ITornContext
    {
        private DbSet<FactionCompareDataTable> FactionCompareDatas { get; set; }

        public TornContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync().Wait();
        }

        public async Task ClearFactionCache(int factionId)
        {
            var entitiesToDelete = await FactionCompareDatas.Where(d => d.FactionID == factionId).ToListAsync();
            FactionCompareDatas.RemoveRange(entitiesToDelete);
            await TrySave();
        }

        public async Task<FactionCompareDataTable> GetFactionCache(int factionId)
        {
            return await FactionCompareDatas.Where(d => d.FactionID == factionId).OrderByDescending(d => d.TimeStamp).FirstOrDefaultAsync();
        }

        public async Task<DateTime?> GetFactionCacheDateTime(int factionId)
        {
            var cacheItem = await FactionCompareDatas.Where(d => d.FactionID == factionId).OrderByDescending(d => d.TimeStamp).FirstOrDefaultAsync();
            return cacheItem?.TimeStamp;
        }

        public async Task AddFactionCache(FactionCompareDataTable factionCompareDataTable)
        {
            FactionCompareDatas.Add(factionCompareDataTable);
            await TrySave();
        }

        private async Task TrySave()
        {
            await SaveChangesAsync();
        }
    }
}