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
        static int errorCount = 0;

        // The following console program exercises the basic UI for creating three types of repeating invoices:
        // Draft
        // Approved
        // Approved for sending
        static void Main(string[] args)
        {
            System.Console.WriteLine("Repeating invoice test running...\n\n");

            // Create a new instance of the Firefox driver.
            IWebDriver driver = new FirefoxDriver();

            //Navigate to Xero login page
            driver.Navigate().GoToUrl(Settings.LoginUrl);

            // Find the login input element by its id
            IWebElement query = driver.FindElement(By.Id(Settings.LoginEmailFieldId));

            // Enter email login
            query.SendKeys(Settings.LoginEmail);

            // Find the password input element by its id
            query = driver.FindElement(By.Id(Settings.LoginPasswordFieldId));

            // Enter password
            query.SendKeys(Settings.LoginPassword);

            // Submit the login form
            query.Submit();

            System.Console.WriteLine("Attempting creation of draft repeating invoice..");
            // Create draft repeating invoice
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.Draft);

            // The following section of code verifies that the invoices were created by locating the invoices by their unique names on the
            // "Repeating" tab of the Invoices page.
            
            // Check existence of draft repeating invoice
            try
            {
                // test that new invoice exists and if found output message
                Assert.IsNotNull(driver.FindElement(By.LinkText(draftInvoiceRef)));
                System.Console.WriteLine("Draft repeating invoice creation - PASS");
            }
            catch (Exception e)
            {
                // if newly created invoice can not be found, exception will be caught and details output to console
                System.Console.WriteLine("Draft repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
                errorCount++;
            }

            System.Console.WriteLine("Attempting creation of approved repeating invoice..");
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.Approved);

            // Check existence of approved repeating invoice
            try
            {
                // test that new invoice exists and if found output message
                Assert.IsNotNull(driver.FindElement(By.LinkText(approvedInvoiceRef)));
                System.Console.WriteLine("Approved repeating invoice creation - PASS");
            }
            catch (Exception e)
            {
                // if newly created invoice can not be found, exception will be caught and details output to console
                System.Console.WriteLine("Approved repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
                errorCount++;
            }

            System.Console.WriteLine("Attempting creation of approved for sending repeating invoice..");
            query = CreateRepeatingInvoice(driver, query, RepeatingInvoiceType.ApprovedForSending);

            // Check existence of approved for sending repeating invoice
            try
            {
                // test that new invoice exists and if found output message
                Assert.IsNotNull(driver.FindElement(By.LinkText(approvedForSendingInvoiceRef)));
                System.Console.WriteLine("Approved for sending repeating invoice creation - PASS");
            }
            catch (Exception e)
            {
                // if newly created invoice can not be found, exception will be caught and details output to console
                System.Console.WriteLine("Approved for sending repeating invoice creation - FAILED");
                System.Console.WriteLine(e.Message);
                errorCount++;
            }

            System.Console.WriteLine("\n\nRepeating invoice test complete.");

            // Check our error count that is incremented when a test fails to know whether test passes or fails.
            if (errorCount == 0)
            {
                System.Console.WriteLine(String.Format("\nTest status: PASS."));
            }
            else            
            {
                System.Console.WriteLine(String.Format("\nTest status: FAILED. See above for {0} error(s).", errorCount.ToString()));
            }

            System.Console.ReadLine();
        }

        // This method handles the logic for creating the repeating invoice. Whether invoice is draft, approved for approved for sending is sent in via parameter.
        private static IWebElement CreateRepeatingInvoice(IWebDriver driver, IWebElement query, RepeatingInvoiceType repeatingInvoiceType)
        {
            //Navigate to Xero repeating invoices page
            driver.Navigate().GoToUrl(Settings.CreateRepeatingInvoiceUrl);

            // Ensure required elements are availble before entering data
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            IWebElement element = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.Id(Settings.DueDateDayFieldId));
            });

            // Find the repeating invoice start date input element by its html id
            query = driver.FindElement(By.Id(Settings.DueDateDayFieldId));

            // Enter value
            query.SendKeys("1");

            // Find the repeating invoice start date input element by its html id
            query = driver.FindElement(By.Id(Settings.StartDateFieldId));

            // Enter something to search for
            query.SendKeys("12/12/2014");

            // Logic is seperated below depended on what type of repeating invoice we wish to create
            if (repeatingInvoiceType == RepeatingInvoiceType.Draft)
            {
                // Find the draft radio button by its html id and select
                query = driver.FindElement(By.Id(Settings.saveAsDraftFieldId));
                query.Click();

                // Find Invoice To field and enter customer name by XPath
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // Give invoice name that is unique to this test execution and record it in variable for verification later
                draftInvoiceRef = string.Format("Draft repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(draftInvoiceRef);

                // Enter basic description. Text area is initially hidden so we need to ensure it is displayed by first clicking the row div
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Draft repeating invoice description");
            }

            if(repeatingInvoiceType == RepeatingInvoiceType.Approved)
            {
                // Find the approved radio button by html id and select
                query = driver.FindElement(By.Id(Settings.saveAsAutoApprovedFieldId));
                query.Click();

                //Find Invoice To field by XPath and enter customer name
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // Give invoice name that is unique to this test execution and record it in variable for verification later
                approvedInvoiceRef = string.Format("Aproved repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(approvedInvoiceRef);

                // Enter basic description. Text area is initially hidden so we need to ensure it is displayed by first clicking the row div
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Approved repeating invoice description");
            }

            if (repeatingInvoiceType == RepeatingInvoiceType.ApprovedForSending)
            {
                // Find the approved for sending radio button by id and select
                query = driver.FindElement(By.Id(Settings.saveAsAutoApprovedAndEmailFieldId));
                query.Click();

                // Edit message popup screen is displayed. Wait until elements are present before writing value
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(Settings.EditMessageToAddress)));

                // Find the email to address by XPath
                query = driver.FindElement(By.XPath(Settings.EditMessageToAddress));
                // Enter email
                query.SendKeys(Settings.ApprovedForSendingEmailTo);

                // Find the approved for sending edit message save button by XPath and click
                query = driver.FindElement(By.XPath(Settings.ApprovedForSendingSaveButtonXPath));
                query.Click();

                //Find Invoice To field and enter customer name
                query = driver.FindElement(By.XPath(Settings.CustomerFieldXPath));

                // Give invoice a name that is unique to this test execution and record it in variable for verification later
                approvedForSendingInvoiceRef = string.Format("Aproved for sending repeating invoice - {0}", System.DateTime.Now.ToString());
                query.SendKeys(approvedForSendingInvoiceRef);

                // Enter basic description. Text area is initially hidden so we need to ensure it is displayed by first clicking the row div
                query = driver.FindElement(By.XPath(Settings.DescriptionXPath));
                query.Click();
                query = driver.FindElement(By.XPath(Settings.DescriptionPopupTextboxXPath));
                query.SendKeys("Approved for sending repeating invoice description");
            }

            // Find the save button by its XPath
            query = driver.FindElement(By.XPath(Settings.SaveButtonXPath));
            query.Click();
            
            // Wait for the page to load by checking the page title is updated, timeout after 10 seconds
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            wait.Until((d) => 
            { 
                return d.Title.StartsWith("Xero | Invoices | "); 
            });
            
            return query;
        }
    }
}
