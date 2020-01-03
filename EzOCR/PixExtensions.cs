using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace EzOCR
{
    public static class PixExtensions
    {
        /// <summary>
        /// Converts byte array to a Pix image, by using a temporary file.
        /// </summary>
        /// <param name="data">The byte array containing the image data.</param>
        /// <returns></returns>
        public static Pix ConvertToPix(this byte[] data)
        {
            using var temporaryFile = new TemporaryFile();
            using var writeStream = temporaryFile.OpenWriteStream();
            writeStream.Write(data, 0, data.Length);
            writeStream.Close();

            return Pix.LoadFromFile(temporaryFile.FullPath);
        }

        /// <summary>
        /// Converts stream to a Pix image by using the file on a file stream or by using a temporary file.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Pix ConvertToPix(this Stream stream)
        {
            if (stream is FileStream fileStream && File.Exists(fileStream.Name))
                return Pix.LoadFromFile(fileStream.Name);

            using var temporaryFile = new TemporaryFile();
            using var writeStream = temporaryFile.OpenWriteStream();
            stream.CopyTo(writeStream);
            stream.Close();
            writeStream.Close();

            return Pix.LoadFromFile(temporaryFile.FullPath);
        }

        /// <summary>
        /// Ensures data folder exists, and run an OCR.
        /// </summary>
        /// <param name="pixImage">The image.</param>
        /// <param name="dataPath">The directory holding the tesseract data.</param>
        /// <param name="language">The language to use during the OCR process.</param>
        /// <param name="disposeImage">If true, the image will be disposed after the OCR process is done.</param>
        /// <returns></returns>
        public static async Task<string> GetTextAndEnsureData(this Pix pixImage,
                                                              string dataPath = "./tessdata",
                                                              string language = "eng",
                                                              bool disposeImage = true)
        {
            var tessdataDownloader = new TessdataDownloader();
            await tessdataDownloader.EnsureDataFolder(language, dataFolder: dataPath);

            using var engine = new TesseractEngine(dataPath, language, EngineMode.Default);
            using var process = engine.Process(pixImage);
            var text = process.GetText();

            if (disposeImage) pixImage.Dispose();

            return text;
        }

        /// <summary>
        /// Runs a basic OCR on an image.
        /// </summary>
        /// <param name="pixImage">The image.</param>
        /// <param name="dataPath">The directory holding the tesseract data.</param>
        /// <param name="language">The language to use during the OCR process.</param>
        /// <param name="disposeImage">If true, the image will be disposed after the OCR process is done.</param>
        /// <returns></returns>
        public static string GetText(this Pix pixImage,
                                     string dataPath = "./tessdata",
                                     string language = "eng",
                                     bool disposeImage = true)
        {
            using var engine = new TesseractEngine(dataPath, language, EngineMode.Default);
            using var process = engine.Process(pixImage);
            var text = process.GetText();

            if (disposeImage) pixImage.Dispose();

            return text;
        }
    }
}