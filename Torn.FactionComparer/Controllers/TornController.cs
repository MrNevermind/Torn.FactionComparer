using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Torn.FactionComparer.Services;

namespace Torn.FactionComparer.Controllers
{
    public class TornController : Controller
    {
        private readonly ICompareDataRetriever _compareDataRetriever;
        private readonly IViewRenderer _viewRenderer;
        private readonly IImageGenerator _imageGenerator;
        private readonly IDbService _dbService;
        private readonly Fixture _fixture;

        public TornController(ICompareDataRetriever compareDataRetriever, IViewRenderer viewRenderer, IImageGenerator imageGenerator, IDbService dbService)
        {
            _compareDataRetriever = compareDataRetriever;
            _viewRenderer = viewRenderer;
            _imageGenerator = imageGenerator;
            _dbService = dbService;
            _fixture = new Fixture();
        }
        public async Task<IActionResult> Cache(int factionId)
        {
            if (factionId == 0)
                return Ok(null);

            return Ok(await _dbService.GetFactionCacheDateTime(factionId));
        }
        public async Task<IActionResult> Clear(int factionId)
        {
            if (factionId == 0)
                return Ok();

            await _dbService.ClearFactionCache(factionId);

            return Ok();
        }
        public async Task<IActionResult> Factions(string apiKey, int firstFaction, int seccondFaction)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || firstFaction == 0 || seccondFaction == 0)
                return Ok(Array.Empty<byte>());

            var compareData = await _compareDataRetriever.GetFactionCompareImageData(apiKey, firstFaction, seccondFaction);
            //var compareData = _fixture.Create<FactionCompareImageData>();

            var htmlContent = await _viewRenderer.RenderViewAsync(this, "Factions", compareData);

            var bytes = await _imageGenerator.GenerateImage(htmlContent);

            return Ok(bytes);
        }
    }
}
