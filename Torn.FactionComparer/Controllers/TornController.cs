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
        private readonly ILogger<TornController> _logger;

        public TornController(ICompareDataRetriever compareDataRetriever, IViewRenderer viewRenderer, IImageGenerator imageGenerator, IDbService dbService, ILogger<TornController> logger)
        {
            _compareDataRetriever = compareDataRetriever;
            _viewRenderer = viewRenderer;
            _imageGenerator = imageGenerator;
            _dbService = dbService;
            _logger = logger;
        }
        public async Task<IActionResult> Cache(int factionId)
        {
            if (factionId == 0)
                return Ok(null);

            var date = await _dbService.GetFactionCacheDateTime(factionId);

            return Ok(date.HasValue ? date.Value.ToShortDateString() + " " + date.Value.ToShortTimeString() : null);
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
            _logger.LogInformation("Stats generation started");
            if (string.IsNullOrWhiteSpace(apiKey) || firstFaction == 0 || seccondFaction == 0)
            {
                _logger.LogWarning("One of the input fields was empty.");
                return Ok(Array.Empty<byte>());
            }

            _logger.LogInformation("Retrieving compare data started");
            var compareData = await _compareDataRetriever.GetFactionCompareImageData(apiKey, firstFaction, seccondFaction);
            _logger.LogInformation("Retrieving compare data finished");

            _logger.LogInformation("Rendering view started");
            var htmlContent = await _viewRenderer.RenderViewAsync(this, "Factions", compareData);
            _logger.LogInformation("Rendering view finished");

            _logger.LogInformation("Generating image started");
            var bytes = await _imageGenerator.GenerateImage(htmlContent);
            _logger.LogInformation("Generating image finished");

            _logger.LogInformation("Stats generation finished");

            return Ok(bytes);
        }
    }
}
