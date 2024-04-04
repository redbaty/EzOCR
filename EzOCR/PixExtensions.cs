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
            return Pix.LoadFromMemory(data);
        }

        /// <summary>
        /// Converts stream to a Pix image.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="useFile">If true, the stream will be written to a temporary file and then loaded as a Pix image.</param>
        /// <returns></returns>
        public static Pix ConvertToPix(this Stream stream, bool useFile = false)
        {
            if (stream is FileStream fileStream && File.Exists(fileStream.Name))
                return Pix.LoadFromFile(fileStream.Name);
            
            if(stream is MemoryStream memoryStream)
                return memoryStream.ToArray().ConvertToPix();

            if (useFile)
            {
                using var temporaryFile = new TemporaryFile();
                using var writeStream = temporaryFile.OpenWriteStream();
                stream.CopyTo(writeStream);
                stream.Close();
                writeStream.Close();

                return Pix.LoadFromFile(temporaryFile.FullPath);
            }
            
            using var convertedStream = new MemoryStream();
            stream.CopyTo(convertedStream);
            return convertedStream.ToArray().ConvertToPix();
        }

        /// <summary>
        /// Ensures data folder exists, and run an OCR.
        /// </summary>
        /// <param name="pixImage">The image.</param>
        /// <param name="dataPath">The directory holding the tesseract data.</param>
        /// <param name="language">The language to use during the OCR process.</param>
        /// <param name="disposeImage">If true, the image will be disposed after the OCR process is done.</param>
        /// <param name="engineMode">The engine mode to use during the OCR process.</param>
        /// <returns></returns>
        public static async Task<string> GetTextAndEnsureData(this Pix pixImage,
                                                              string dataPath = "./tessdata",
                                                              string language = "eng",
                                                              bool disposeImage = true,
                                                              EngineMode engineMode = EngineMode.TesseractOnly)
        {
            var tessdataDownloader = new TessdataDownloader();
            await tessdataDownloader.EnsureDataFolder(language, dataFolder: dataPath);

            using var engine = new TesseractEngine(dataPath, language, engineMode);
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
        /// <param name="engineMode">The engine mode to use during the OCR process.</param> 
        /// <returns></returns>
        public static string GetText(this Pix pixImage,
                                     string dataPath = "./tessdata",
                                     string language = "eng",
                                     bool disposeImage = true,
                                     EngineMode engineMode = EngineMode.TesseractOnly)
        {
            using var engine = new TesseractEngine(dataPath, language, engineMode);
            using var process = engine.Process(pixImage);
            var text = process.GetText();

            if (disposeImage) pixImage.Dispose();

            return text;
        }
    }
}