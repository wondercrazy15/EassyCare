using System;
using System.Collections.Generic;
using Twilio.Jwt.AccessToken;

namespace EasyCare.Services
{
    public class TwilioToken
    {
        public string twilioAccountSid = "AC1cf545dd9abaa20cec5aefbbc9a092c4";
        public string twilioApiKey = "SKcf54685dc05c54a9e15d5b970a1af798";
        public string twilioApiSecret = "ec4gh4GESBr3gGHO5ndD8OAq4oC0Fn7Y";
        public string authToken = "d2bf7989dfe45ab77aad5e75ece79f43";
        // These are specific to Chat
        public string serviceSid = "IS9613cfe0039243df84535e7b48cf5344";
        

        public string getTwilioToken(string identity)
        {
            var grant = new ChatGrant
            {
                ServiceSid = serviceSid
            };

            var grants = new HashSet<IGrant>
                {
                    { grant }
                };
            var token = new Token(
           twilioAccountSid,
           twilioApiKey,
           twilioApiSecret,
           identity,
           grants: grants);
            return token.ToJwt();
        }
    }
}
