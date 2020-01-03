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
            var imageOcr = await ResourceLoader.GetFileFromAssembly("*basic_captcha.jpg").ConvertToPix()
                                               .GetTextAndEnsureData().CleanAndFlattenString();
            
            Assert.AreEqual(imageOcr, "m85W8I");
        }
    }
}