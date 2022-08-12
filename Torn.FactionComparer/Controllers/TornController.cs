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
        private readonly Fixture _fixture;

        public TornController(ICompareDataRetriever compareDataRetriever, IViewRenderer viewRenderer, IImageGenerator imageGenerator)
        {
            _compareDataRetriever = compareDataRetriever;
            _viewRenderer = viewRenderer;
            _imageGenerator = imageGenerator;
            _fixture = new Fixture();
        }

        public async Task<IActionResult> Factions(string apiKey, int firstFaction, int seccondFaction)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || firstFaction == 0 || seccondFaction == 0)
                return Ok(Array.Empty<byte>());

            var compareData = await _compareDataRetriever.GetFactionCompareImageData(apiKey, firstFaction, seccondFaction);
            //var compareData = _fixture.Create<FactionCompareImageData>();

            var htmlContent = await _viewRenderer.RenderViewAsync(this, "Factions", compareData);

            var bytes = _imageGenerator.GenerateImage(htmlContent);

            return Ok(bytes);
        }
    }
}
