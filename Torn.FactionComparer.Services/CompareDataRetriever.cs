using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Torn.FactionComparer.Contracts.FactionData;
using Torn.FactionComparer.Contracts.UserData;

namespace Torn.FactionComparer.Services
{
    public interface ICompareDataRetriever
    {
        Task<FactionCompareImageData> GetFactionCompareImageData(string apiKey, int firstFactionId, int seccondFactionId);
    }

    public class CompareDataRetriever : ICompareDataRetriever
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memmoryCache;

        private string _apiKey;
        private int _innerRequestCount = 0;
        private int _requestCount
        {
            get
            {
                return _innerRequestCount;
            }
            set
            {
                _innerRequestCount = value;
                if (_innerRequestCount >= 100)
                {
                    _innerRequestCount = 0;
                    Thread.Sleep(1000 * 60);
                }
            }
        }

        public CompareDataRetriever(IHttpClientFactory httpClientFactory, IMemoryCache memmoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memmoryCache = memmoryCache;
        }

        public async Task<FactionCompareImageData> GetFactionCompareImageData(string apiKey, int firstFactionId, int seccondFactionId)
        {
            _apiKey = apiKey;

            return new FactionCompareImageData()
            {
                FirstFaction = await GetFactionCompareData(firstFactionId),
                SeccondFaction = await GetFactionCompareData(seccondFactionId)
            };
        }
        private async Task<FactionCompareData> GetFactionCompareData(int factionId)
        {
            if (_memmoryCache.TryGetValue(factionId, out var cachedFactionCompareData))
            {
                return (FactionCompareData)cachedFactionCompareData;
            }

            var factionPropertyBag = await GetFactionData(factionId);

            var factionsMembersInfo = await GetFactionMembersInfo(factionPropertyBag.Members.Select(m => m.Id).ToArray());

            var factionCompareData = new FactionCompareData
            {
                ID = factionPropertyBag.Id,
                Name = factionPropertyBag.Name,
                Respect = factionPropertyBag.Respect,
                BestChain = factionPropertyBag.BestChain,
                Members = factionPropertyBag.Members.Count,
                AverageAge = factionsMembersInfo.Average(m => m.Age),
                AverageLevel = factionsMembersInfo.Average(m => m.Level),
                AverageActivity = TimeSpan.Zero,
                AverageActivityPerDay = TimeSpan.Zero,
                AverageAwards = factionsMembersInfo.Average(m => m.Awards),
                Xanax = factionsMembersInfo.Sum(m => m.PersonalStats.XanaxTaken),
                LSD = factionsMembersInfo.Sum(m => m.PersonalStats.LsdTaken),
                Vicodin = factionsMembersInfo.Sum(m => m.PersonalStats.VicodinTaken),
                Overdoses = factionsMembersInfo.Sum(m => m.PersonalStats.TimesOverdosed),
                EnergyRefils = factionsMembersInfo.Sum(m => m.PersonalStats.Refills),
                EnergyDrinks = factionsMembersInfo.Sum(m => m.PersonalStats.EnergyDrinksUsed),
                Boosters = factionsMembersInfo.Sum(m => m.PersonalStats.BoostersUsed),
                StatEnhancers = factionsMembersInfo.Sum(m => m.PersonalStats.StatEnhancersUsed),
                AttacksMade =
                    factionsMembersInfo.Sum(m => m.PersonalStats.AttacksWon) +
                    factionsMembersInfo.Sum(m => m.PersonalStats.AttacksLost) +
                    factionsMembersInfo.Sum(m => m.PersonalStats.AttacksStalemated) +
                    factionsMembersInfo.Sum(m => m.PersonalStats.YouRunAway),
                RevivesGiven = factionsMembersInfo.Sum(m => m.PersonalStats.Revives),
                RevivesReceived = factionsMembersInfo.Sum(m => m.PersonalStats.RevivesReceived),
                BountiesPlaced = factionsMembersInfo.Sum(m => m.PersonalStats.BountiesPlaced),
                BountiesReceived = factionsMembersInfo.Sum(m => m.PersonalStats.BountiesReceived),
                MissionCredits = factionsMembersInfo.Sum(m => m.PersonalStats.MissionCreditsEarned),
                SpecialAmmo = factionsMembersInfo.Sum(m => m.PersonalStats.SpecialAmmoUsed),
                JobPoints = factionsMembersInfo.Sum(m => m.PersonalStats.JobPointsUsed),
                RespectEarned = factionsMembersInfo.Sum(m => m.PersonalStats.RespectForFaction),
                Networth = factionsMembersInfo.Sum(m => m.PersonalStats.Networth),
                Karma = factionsMembersInfo.Sum(m => m.Karma),
                BooksRead = factionsMembersInfo.Sum(m => m.PersonalStats.BooksRead),
            };

            _memmoryCache.Set(factionId, factionCompareData, new TimeSpan(1, 0, 0));

            return factionCompareData;
        }

        private async Task<UserPropertyBag[]> GetFactionMembersInfo(int[] userIds)
        {
            var list = new List<UserPropertyBag>();
            foreach (var userId in userIds)
            {
                var user = await GetUserInfo(userId);
                if (user != null && user.PersonalStats != null)
                    list.Add(user);
            }
            return list.ToArray();
        }

        private async Task<UserPropertyBag> GetUserInfo(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                _requestCount++;

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.torn.com/user/{id}?selections=profile,personalstats&key={_apiKey}");
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentFirstFaction = await httpResponseMessage.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<UserPropertyBag>(contentFirstFaction);
                }
                else
                    return null;
            }
        }

        private async Task<FactionPropertyBag> GetFactionData(int factionId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                _requestCount++;

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.torn.com/faction/{factionId}?selections=basic&key={_apiKey}");
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentFirstFaction = await httpResponseMessage.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<FactionPropertyBag>(contentFirstFaction);
                }
                else
                    return null;
            }
        }
    }
}
