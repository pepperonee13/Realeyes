using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealeyesHomework.UI.Services.Adapters;
using System.Reflection;
using System.Xml.Linq;

namespace RealeyesHomework.UnitTest
{
    [TestClass]
    public class ExchangeDataAdapterTests
    {
        private const string SampleXmlName = "RealeyesHomework.UnitTest.TestData.eurofxref-hist-90d.xml";
        private XDocument _xml;

        [TestInitialize]
        public void Initialize()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(SampleXmlName);
            _xml = XDocument.Load(stream);
        }

        [TestMethod]
        public void Test_All_Dates_Are_Adapted()
        {
            var result = ExchangeDataAdapter.Adapt(_xml);

            Assert.AreEqual(64, result.Count);
        }
    }
}
