using System.Configuration;

namespace RepeatingInvoiceTests
{
    static class Settings
    {
        public static string LoginUrl
        {
            get { return ConfigurationManager.AppSettings["LoginUrl"]; }
        }

        public static string CreateRepeatingInvoiceUrl
        {
            get { return ConfigurationManager.AppSettings["CreateRepeatingInvoiceUrl"]; }
        }

        public static string LoginEmail
        {
            get { return ConfigurationManager.AppSettings["LoginEmail"]; }
        }

        public static string LoginPassword
        {
            get { return ConfigurationManager.AppSettings["LoginPassword"]; }
        }

        public static string LoginEmailFieldId
        {
            get { return ConfigurationManager.AppSettings["LoginEmailFieldId"]; }
        }
        public static string LoginPasswordFieldId
        {
            get { return ConfigurationManager.AppSettings["LoginPasswordFieldId"]; }
        }

        public static string CustomerFieldXPath
        {
            get { return ConfigurationManager.AppSettings["CustomerFieldXPath"]; }
        }

        public static string DescriptionXPath
        {
            get { return ConfigurationManager.AppSettings["DescriptionXPath"]; }
        }

        public static string DescriptionPopupTextboxXPath
        {
            get { return ConfigurationManager.AppSettings["DescriptionPopupTextboxXPath"]; }
        }

        public static string DueDateDayFieldId
        {
            get { return ConfigurationManager.AppSettings["DueDateDayFieldId"]; }
        }

        public static string StartDateFieldId
        {
            get { return ConfigurationManager.AppSettings["StartDateFieldId"]; }
        }

        public static string SaveButtonXPath
        {
            get { return ConfigurationManager.AppSettings["SaveButtonXPath"]; }
        }

        public static string EditMessageToAddress
        {
            get { return ConfigurationManager.AppSettings["EditMessageToAddress"]; }
        }

        public static string ApprovedForSendingEmailTo
        {
            get { return ConfigurationManager.AppSettings["ApprovedForSendingEmailTo"]; }
        }

        public static string ApprovedForSendingSaveButtonXPath
        {
            get { return ConfigurationManager.AppSettings["ApprovedForSendingSaveButtonXPath"]; }
        }

        public static string saveAsDraftFieldId
        {
            get { return ConfigurationManager.AppSettings["saveAsDraftFieldId"]; }
        }

        public static string saveAsAutoApprovedFieldId
        {
            get { return ConfigurationManager.AppSettings["saveAsAutoApprovedFieldId"]; }
        }

        public static string saveAsAutoApprovedAndEmailFieldId
        {
            get { return ConfigurationManager.AppSettings["saveAsAutoApprovedAndEmailFieldId"]; }
        }
    }
}
