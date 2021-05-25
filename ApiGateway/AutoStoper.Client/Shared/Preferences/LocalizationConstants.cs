using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Preferences
{
    public static class LocalizationConstants
    {
        public static readonly LanguageCode[] SupportedLanguages = {
            new LanguageCode
            {
                Code = "hr-HR",
                DisplayName = "Croatian"
            },
            new LanguageCode
            {
                Code = "en-US",
                DisplayName= "English"
            }
        };
    }
}
