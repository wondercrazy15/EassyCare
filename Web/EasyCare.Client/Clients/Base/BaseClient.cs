using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EasyCare.Client.Exceptions;
using EasyCare.Core.Constants;
using Newtonsoft.Json;

namespace EasyCare.Client.Clients.Base
{
    public abstract class BaseClient
    {
        private Options _options;
        private HttpClient _httpClient;
        private string _service;
        private string _baseAddress;

        protected BaseClient(Options options, string service)
        {
            _options = options;
            _httpClient = new HttpClient();
            _service = service;

            var protocol = options.UseHttps ? "https" : "http";
            _baseAddress = $"{protocol}://{_options.UrlRoot}";

            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();

          
          
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        

        protected async virtual Task<TResponse> Post<TRequest, TResponse>(string methodName, TRequest request)
            where TResponse : class
        {
            var url = BuildUrl(methodName);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync(url, content);
            return await ServeResposne<TResponse>(responseMessage);
        }
        
        protected async virtual Task<TResponse> Put<TRequest, TResponse>(string methodName, string parameters, TRequest request)
            where TResponse : class
        {
            var url = BuildUrl(methodName, parameters);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PutAsync(url, content);
            return await ServeResposne<TResponse>(responseMessage);
        }
        
        protected async virtual Task<TResponse> Delete<TResponse>(string methodName = null, string parameters = null)
            where TResponse : class
        {
            var url = BuildUrl(methodName, parameters);
            var responseMessage = await _httpClient.DeleteAsync(url);
            return await ServeResposne<TResponse>(responseMessage);
        }
        
        protected async virtual Task Delete(string methodName = null, string parameters = null)
        {
            var url = BuildUrl(methodName, parameters);
            var responseMessage = await _httpClient.DeleteAsync(url);
            
            CheckStatusCode(responseMessage);
        }
        
        protected async virtual Task<TResponse> Get<TResponse>(string methodName = null, string parameters = null)
            where TResponse : class
        {
            var url = BuildUrl(methodName, parameters);

            if (GlobalConstant.AccessToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalConstant.AccessToken);
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalConstant.AccessToken);
            }

            var responseMessage = await _httpClient.GetAsync(url);
            return await ServeResposne<TResponse>(responseMessage);
        }

        private string BuildUrl(string methodName = null, string parameters = null)
        {
            var builder = new StringBuilder($"{_baseAddress}/{_service}");

            if (!string.IsNullOrEmpty(methodName))
            {
                builder.Append($"/{methodName}");
            }

            if (!string.IsNullOrEmpty(parameters))
            {
                builder.Append($"?{parameters}");
            }

            return builder.ToString();
        }

        private void CheckStatusCode(HttpResponseMessage message)
        {
            if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizeException();
            }

            if (message.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException();
            }
        }
        
        private async Task<TResponse> ServeResposne<TResponse>(HttpResponseMessage message) 
            where TResponse : class
        {
            CheckStatusCode(message);

            if (message.StatusCode == HttpStatusCode.Accepted)
            {
                return null;
            }
            
            var json = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(json);
        }
    }
}