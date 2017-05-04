using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRentalDsed_WPF.XamlConverters;

namespace UnitTests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void TestXamlConverter()
        {
            var xamlConv = new DateMinToString();
            var result = xamlConv.Convert(DateTime.MinValue, null, null, CultureInfo.CurrentUICulture);

            Assert.AreSame("Not returned", result);
        }
    }
}
