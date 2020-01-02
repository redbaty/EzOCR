using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EzOCR.Tests
{
    [TestClass]
    public class OCRTests
    {
        [TestMethod]
        public async Task TestCaptcha()
        {
            var tessdataDownloader = new TessdataDownloader();
            await tessdataDownloader.EnsureDataFolder();
            
            var imageOcr = ResourceLoader.GetFileFromAssembly("*basic_captcha.jpg").ConvertToPix()
                                      .GetText().Replace("\n", string.Empty);
            
            Assert.AreEqual(imageOcr, "m85W8I");
        }
    }
}