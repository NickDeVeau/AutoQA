using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using log4net;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using UITesting.PageModels;

namespace UITesting.Pages
{
	public class GooglePage : BaseTestUI
	{
        public void VerifyImages()
        {
            UI.WaitForElementToBeVisible(Driver.Instance, By.XPath(GooglePageModel.imgGoogle), WaitTime);
        }
    }
}

