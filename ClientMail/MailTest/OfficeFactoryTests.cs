using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMail.Tests
{
    [TestClass()]
    public class OfficeFactoryTests
    {
        [TestMethod()]
        public void CreateContentTest()
        {
            var stream = new OfficeFactory("smtp.gmail.com", 587, "sygnion", "sygnion01!")
                .CreateContent(new DateTime(2015, 7, 7), new DateTime(2015, 7, 7), null);

            
            //Assert.Fail();
        }
    }
}