using System;
using NUnit.Framework;
using UITesting.Pages;

namespace UITesting.TestSuites
{
	public class Google : BaseTestUI
	{
        GooglePage googlePage = new GooglePage();

        [Test]
        public void GoogleImage()
        {
            googlePage.VerifyImages();
        }
    }
}

