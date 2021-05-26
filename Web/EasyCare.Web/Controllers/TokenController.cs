using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EasyCare.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Token")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<ActionResult<TokenDto>> Token([Required] LoginDto item)
        {
            try
            {
                TokenDto token = null;
                string clientId = _configuration.GetValue<string>("AzureAdB2C:ClientId");

                var formVars = new Dictionary<string, string>();
                formVars.Add("username", item.Email);
                formVars.Add("password", item.Password);
                formVars.Add("grant_type", "password");
                formVars.Add("scope", "openid " + clientId + " offline_access");
                formVars.Add("client_id", clientId);
                formVars.Add("response_type", "token id_token");
                var content = new FormUrlEncodedContent(formVars);

                var tokenEndPoint = string.Format("https://easycareadb2c.b2clogin.com/easycareadb2c.onmicrosoft.com/B2C_1A_ROPC_Auth/oauth2/v2.0/token");

                var client = new HttpClient();
                var uri = new Uri(tokenEndPoint);
                var response = client.PostAsync(uri, content).Result;

                var result = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                if (response.IsSuccessStatusCode)
                {
                    token = new TokenDto();

                    token.AccessToken = jObj["access_token"];
                    token.TokenType = jObj["token_type"];
                    token.ExpiresIn = jObj["expires_in"];
                    token.RefreshToken = jObj["refresh_token"];
                    token.IdToken = jObj["id_token"];

                    return Ok(token);
                }
                else
                {
                    return BadRequest(new { Status = jObj["error"], Message = jObj["error_description"] });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "server_error", Message = ex.Message });
            }
        }

    }
}