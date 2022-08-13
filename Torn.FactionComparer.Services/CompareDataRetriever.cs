using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CompareDataRetriever> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDbService _dbService;
        private readonly IStatsCalculator _statsCalculator;
        private string _apiKey;

        public CompareDataRetriever(ILogger<CompareDataRetriever> logger, IHttpClientFactory httpClientFactory, IDbService dbService, IStatsCalculator statsCalculator)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _dbService = dbService;
            _statsCalculator = statsCalculator;
        }

        public async Task<FactionCompareImageData> GetFactionCompareImageData(string apiKey, int firstFactionId, int seccondFactionId)
        {
            _apiKey = apiKey;

            var firstFactionCompareData = await GetFactionCompareData(firstFactionId);
            var seccondFactionCompareData = await GetFactionCompareData(seccondFactionId);

            return new FactionCompareImageData()
            {
                FirstFaction = firstFactionCompareData,
                SeccondFaction = seccondFactionCompareData,
                Stats = _statsCalculator.CalculateStats(firstFactionCompareData, seccondFactionCompareData)
            };
        }
        private async Task<FactionCompareData> GetFactionCompareData(int factionId)
        {
            FactionCompareData cachedData = await _dbService.GetFactionCache(factionId);

            if (cachedData != null)
            {
                _logger.LogInformation("Faction with ID {factionId} was found in cache", factionId);
                return cachedData;
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
                AverageActivity = factionsMembersInfo.Average(m => m.PersonalStats.UserActivity),
                AverageActivityPerDay = factionsMembersInfo.Average(m => m.PersonalStats.UserActivity / (m.Age == 0 ? 1: m.Age)),
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

            await _dbService.AddFactionCache(factionCompareData);

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
                else
                    _logger.LogWarning("User with ID {Id} was not found", userId);
            }
            return list.ToArray();
        }

        private async Task<UserPropertyBag> GetUserInfo(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.torn.com/user/{id}?selections=profile,personalstats&key={_apiKey}");
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentFirstFaction = await httpResponseMessage.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<UserPropertyBag>(contentFirstFaction);

                    if (data.ErrorInfo == null)
                    {
                        return data;
                    }
                    else if (data.ErrorInfo.ErrorCode == 5)
                    {
                        _logger.LogWarning("Too many requests, sleeping for a minute.");
                        Thread.Sleep(1000*60);
                        return await GetUserInfo(id);
                    }
                    else
                    {
                        _logger.LogError("Error happened in GetUserInfo. Code: {Code}, Message: {Message}", data.ErrorInfo.ErrorCode, data.ErrorInfo.ErrorMessage);
                        return null;
                    }
                }
                else
                    return null;
            }
        }

        private async Task<FactionPropertyBag> GetFactionData(int factionId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.torn.com/faction/{factionId}?selections=basic&key={_apiKey}");
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentFirstFaction = await httpResponseMessage.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<FactionPropertyBag>(contentFirstFaction);

                    if (data.ErrorInfo == null)
                    {
                        return data;
                    }
                    else if (data.ErrorInfo.ErrorCode == 5)
                    {
                        _logger.LogWarning("Too many requests, sleeping for a minute.");
                        Thread.Sleep(1000 * 60);
                        return await GetFactionData(factionId);
                    }
                    else
                    {
                        _logger.LogError("Error happened in GetFactionData. Code: {Code}, Message: {Message}", data.ErrorInfo.ErrorCode, data.ErrorInfo.ErrorMessage);
                        return null;
                    }
                }
                else
                    return null;
            }
        }
    }
}
