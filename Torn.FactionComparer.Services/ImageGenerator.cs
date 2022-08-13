using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Torn.FactionComparer.Services
{
    public interface IImageGenerator
    {
        Task<byte[]> GenerateImage(string html);
    }

    public class ImageGenerator : IImageGenerator
    {
        private readonly ILogger<ImageGenerator> _logger;

        public ImageGenerator(ILogger<ImageGenerator> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> GenerateImage(string html)
        {
            var guid = Guid.NewGuid();

            var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            var inputFile = Path.Combine(fileDir, $"{guid}.html");
            var outputFile = Path.Combine(fileDir, $"{guid}.jpeg");
            await File.WriteAllTextAsync(inputFile, html);

            var pProcess = new Process();
            pProcess.StartInfo.FileName = "wkhtmltoimage";
            pProcess.StartInfo.Arguments = $"--format jpeg --crop-w 1000 {inputFile} {outputFile}";
            pProcess.Start();
            await pProcess.WaitForExitAsync();
            pProcess.Close();

            var bytes = await File.ReadAllBytesAsync(outputFile);

            SafeFileDelete(inputFile);
            SafeFileDelete(outputFile);

            return bytes;
        }

        private void SafeFileDelete(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Couln't delete {FileName}, retrying in 100ms. Reason: {Reason}", fileName, ex.Message);
                Thread.Sleep(100);
                SafeFileDelete(fileName);
            }
        }
    }
}
