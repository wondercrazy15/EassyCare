using System;
namespace EasyCare.B2C
{
    public class B2CConstants
    {
        // Azure AD B2C Coordinates
        public static string Tenant = "easycareadb2c.onmicrosoft.com";
        public static string AzureADB2CHostname = "easycareadb2c.b2clogin.com";
        public static string ClientID = "79b4fa12-bec8-4298-887f-6a39ede2ed01";
        public static string PolicySignUpSignIn = "B2C_1_b2c_easycare_susi";
        public static string PolicyEditProfile = "b2c_easycare_pe";
        public static string PolicyResetPassword = "b2c_easycare_reset";

        public static string[] Scopes = {
                         "https://easycareadb2c.onmicrosoft.com/api/demo.read",
                         "https://easycareadb2c.onmicrosoft.com/api/demo.write"
                                        };

        public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
        public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";
        public static string IOSKeyChainGroup = "com.microsoft.adalcache";

    }
}
