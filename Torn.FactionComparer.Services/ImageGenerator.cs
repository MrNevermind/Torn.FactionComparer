using System.Diagnostics;

namespace Torn.FactionComparer.Services
{
    public interface IImageGenerator
    {
        byte[] GenerateImage(string html);
    }

    public class ImageGenerator : IImageGenerator
    {
        public byte[] GenerateImage(string html)
        {
            var guid = Guid.NewGuid();

            var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            var inputFile = Path.Combine(fileDir, $"{guid}.html");
            var outputFile = Path.Combine(fileDir, $"{guid}.jpeg");
            File.WriteAllText(inputFile, html);

            var pProcess = new Process();
            pProcess.StartInfo.FileName = "wkhtmltoimage";
            pProcess.StartInfo.Arguments = $"--format jpeg {inputFile} {outputFile}";
            pProcess.Start();
            pProcess.WaitForExit();
            pProcess.Close();

            return File.ReadAllBytes(outputFile);
        }
    }
}
