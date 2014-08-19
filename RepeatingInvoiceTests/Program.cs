using System;
using System.Configuration;
using Xero.Api.Core;
using Xero.Api.Serialization;
using Xero.Api.Example.TokenStores;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Example.Applications;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

namespace RepeatingInvoiceTests
{
    class Program
    {
        enum RepeatingInvoiceType
        {
            Draft = 1, Approved = 2, ApprovedForSending = 3
        }

        static string draftInvoiceRef;
        static string approvedInvoiceRef;
        static string approvedForSendingInvoiceRef;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Repeating invoice test running...\n\n");

            // Create a new instance of the Firefox driver.
            IWebDriver driver = new FirefoxDriver();

            //Navigate to Xero login page
            driver.Navigate().GoToUrl(Settings.LoginUrl);

            // Find the login input element by its id
            IWebElement query = driver.FindElement(By.Id("email"));

            // Enter email login
            query.SendKeys(Settings.LoginEmail);

            // Find the password input element by its id
            query = driver.FindElement(By.Id("password"));

            // Enter password
            query.SendKeys(Settings.LoginPassword);

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            System.Console.WriteLine("Attempting creation of draft repeating invoice..");
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.Draft);

            try
            {
                Assert.IsNotNull(driver.FindElement(By.LinkText(draftInvoiceRef)));
                System.Console.WriteLine("Draft repeating invoice creation - SUCCESS");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Draft repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
            }

            System.Console.WriteLine("Attempting creation of approved repeating invoice..");
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.Approved);

            try
            {
                Assert.IsNotNull(driver.FindElement(By.LinkText(approvedInvoiceRef)));
                System.Console.WriteLine("Approved repeating invoice creation - SUCCESS");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Approved repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
            }

            System.Console.WriteLine("Attempting creation of approved for sending repeating invoice..");
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.ApprovedForSending);

            try
            {
                Assert.IsNotNull(driver.FindElement(By.LinkText(approvedForSendingInvoiceRef)));
                System.Console.WriteLine("Approved for sending repeating invoice creation - SUCCESS");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Approved for sending repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
            }

            System.Console.WriteLine("\n\nRepeating invoice test complete.");
            System.Console.ReadLine();
            //Close the browser
            driver.Quit();
        }

        private static IWebElement CreateRepeatingInvoice(IWebDriver driver, IWebElement query, RepeatingInvoiceType repeatingInvoiceType)
        {
            //Navigate to Xero repeating invoices page
            driver.Navigate().GoToUrl(Settings.CreateRepeatingInvoiceUrl);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IWebElement element = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.Id("DueDateDay"));
            });

            // Find the repeating invoice start date input element by its id
            query = driver.FindElement(By.Id("DueDateDay"));

            // Enter something to search for
            query.SendKeys("1");

            // Find the repeating invoice start date input element by its id
            query = driver.FindElement(By.Id("StartDate"));

            // Enter something to search for
            query.SendKeys("12/12/2014");

            if (repeatingInvoiceType == RepeatingInvoiceType.Draft)
            {
                // Find the draft radio button and select
                query = driver.FindElement(By.Id("saveAsDraft"));
                query.Click();

                // Find Invoice To field and enter customer name
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // give invoice name that is unique to this test
                draftInvoiceRef = string.Format("Draft repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(draftInvoiceRef);

                //Enter description
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Draft repeating invoice description");
            }

            if(repeatingInvoiceType == RepeatingInvoiceType.Approved)
            {
                // Find the approved radio button and select
                query = driver.FindElement(By.Id("saveAsAutoApproved"));
                query.Click();

                //Find Invoice To field and enter customer name
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // give invoice name that is unique to this test
                approvedInvoiceRef = string.Format("Aproved repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(approvedInvoiceRef);

                //Enter description
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Approved repeating invoice description");
            }

            if (repeatingInvoiceType == RepeatingInvoiceType.ApprovedForSending)
            {
                // Find the approved for sending radio button and select
                query = driver.FindElement(By.Id("saveAsAutoApprovedAndEmail"));
                query.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                element = wait.Until<IWebElement>((d) =>
                {
                    return d.FindElement(By.XPath(Settings.EditMessageToAddress));
                });

                // Find the repeating invoice start date input element by its id
                query = driver.FindElement(By.XPath(Settings.EditMessageToAddress));
                // Enter email
                query.SendKeys(Settings.ApprovedForSendingEmailTo);

                // Find the approved for sending edit message save button and click
                query = driver.FindElement(By.XPath(Settings.ApprovedForSendingSaveButtonXPath));
                query.Click();

                //Find Invoice To field and enter customer name
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // give invoice name that is unique to this test
                approvedForSendingInvoiceRef = string.Format("Aproved for sending repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(approvedForSendingInvoiceRef);

                //Enter description
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Approved for sending repeating invoice description");
            }

            // Find the save button by its XPath
            query = driver.FindElement(By.XPath(Settings.SaveButtonXPath));
            query.Click();
            
            // Wait for the page to load, timeout after 10 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));

            string test = driver.Title;
            wait.Until((d) => 
            { 
                return d.Title.StartsWith("Xero | Invoices | "); 
            });
            
            return query;
        }
    }
}
