using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace APITesting
{
    public class API : BaseTestAPI
    {
        public static async Task<bool> VerifyDefinitionExists(string endpoint, Dictionary<string, string> parameters, Dictionary<string, object> definitions, bool assert = true)
        {
            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            var fullUrl = baseUrl + endpoint;

            if (parameters != null)
            {
                fullUrl = fullUrl + "?" + string.Join("&", parameters.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            }

            int maxAttempts = 3;

            string errorMessage = "";
            string statusCode = "Cannot Get Status Code.";

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    statusCode = $"{(int)response.StatusCode}, ({response.StatusCode}).";

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(responseContent);

                        foreach (var definition in definitions)
                        {
                            if (!DefinitionExistsInJson(jsonResponse, definition.Key, definition.Value))
                            {
                                errorMessage = $"Definition with key '{definition.Key}' and value '{definition.Value}' not found in the JSON response.";
                                throw new Exception(errorMessage);
                            }
                        }

                        if (assert)
                        {
                            Assert.IsTrue(true, "All definitions exist in the JSON response.");
                            Console.WriteLine(responseContent);
                        }
                        return true;
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        errorMessage = response.StatusCode.ToString();

                        try
                        {
                            var errorJson = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
                            errorMessage = (string)errorJson["message"] ?? errorMessage;
                        }
                        catch
                        {
                            // Handle JSON parse failure if needed
                        }
                    }

                    await Task.Delay(1000); // delay 1 second before retrying
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt + 1} failed with exception: {ex}");
                }
            }

            if (assert)
            {
                Assert.Fail($"Expected definitions not found in Json. Status code: {statusCode}. {errorMessage}");
            }
            return false;
        }

        private static bool DefinitionExistsInJson(Newtonsoft.Json.Linq.JToken token, string key, object value)
        {
            if (token is Newtonsoft.Json.Linq.JObject jsonObject)
            {
                if (jsonObject.TryGetValue(key, out var jTokenValue))
                {
                    if (value == null && jTokenValue.Type == Newtonsoft.Json.Linq.JTokenType.Null)
                    {
                        return true;
                    }
                    else if (value != null && jTokenValue.ToString() == value.ToString())
                    {
                        return true;
                    }
                }

                // Also search nested objects:
                foreach (var childToken in jsonObject.Values())
                {
                    if (DefinitionExistsInJson(childToken, key, value))
                        return true;
                }
            }
            else if (token is Newtonsoft.Json.Linq.JArray jArray)
            {
                foreach (var childToken in jArray)
                {
                    if (DefinitionExistsInJson(childToken, key, value))
                        return true;
                }
            }

            return false;
        }




        public static async Task PostToEndpoint(string endpoint, Dictionary<string, object> data)
        {
            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            var url = baseUrl + endpoint;

            var jsonData = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"POST request to '{url}' failed with status code {(int)response.StatusCode}, ({response.StatusCode}). Server says: {responseContent}";

                try
                {
                    var errorJson = JObject.Parse(responseContent);
                    errorMessage = $"{response.StatusCode}: {(string)errorJson["message"] ?? errorMessage}";
                }
                catch
                {
                    // Handle JSON parse failure if needed
                }

                Assert.Fail(errorMessage);
                throw new HttpRequestException(errorMessage);
            }

            Assert.IsTrue(response.IsSuccessStatusCode, "POST request failed.");
        }

        public static async Task PostToEndpointWithParams(string endpoint, Dictionary<string, string> parameters)
        {
            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            var url = baseUrl + endpoint;

            var query = string.Join("&", parameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            if (!string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }

            var response = await client.PostAsync(url, null); // No content in the body

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"POST request to '{url}' failed with status code {(int)response.StatusCode}, ({response.StatusCode}). Server says: {responseContent}";

                try
                {
                    var errorJson = JObject.Parse(responseContent);
                    errorMessage = $"{response.StatusCode}: {(string)errorJson["message"] ?? errorMessage}";
                }
                catch
                {
                    // Handle JSON parse failure if needed
                }

                throw new HttpRequestException(errorMessage);
            }

            Assert.IsTrue(response.IsSuccessStatusCode, "POST request failed.");
        }

        public static async Task<HttpResponseMessage> PutToEndpoint(string endpoint, Dictionary<string, object> data)
        {
            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            var fullUrl = baseUrl + endpoint;

            string jsonPayload = JsonConvert.SerializeObject(data);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(fullUrl, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseContent);
                string errorMessage = jsonResponse["ErrorMessage"]?.ToString() ?? $"PUT request to {endpoint} failed with status code {(int)response.StatusCode}, ({response.StatusCode}).";
                Assert.IsTrue(response.IsSuccessStatusCode, errorMessage);
            }

            return response;
        }

        public static async Task<HttpResponseMessage> DeleteFromEndpoint(string endpoint, Dictionary<string, string> parameters)
        {
            try
            {
                var baseUrl = _configuration.GetSection("API")["BaseUrl"];
                var url = baseUrl + endpoint;

                if (parameters != null)
                {
                    url = url + "?" + string.Join("&", parameters.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                }

                HttpResponseMessage response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    string errorMessage = $"Delete request to '{url}' failed with status code {(int)response.StatusCode}, ({response.StatusCode}).";

                    try
                    {
                        var errorJson = JObject.Parse(responseContent);
                        errorMessage = $"{response.StatusCode}: {(string)errorJson["message"] ?? errorMessage}";
                    }
                    catch
                    {
                        // Handle JSON parse failure if needed
                    }

                    throw new HttpRequestException(errorMessage);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting from endpoint: {ex.Message}", ex);
            }
        }

        public static async Task<HttpResponseMessage> DeleteFromEndpointWithBody(string endpoint, object body)
        {
            try
            {
                var baseUrl = _configuration.GetSection("API")["BaseUrl"];
                var url = baseUrl + endpoint;

                string jsonPayload = body != null ? JsonConvert.SerializeObject(body) : null;
                HttpContent httpContent = jsonPayload != null ? new StringContent(jsonPayload, Encoding.UTF8, "application/json") : null;

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = httpContent
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    string errorMessage = $"Delete request to '{url}' with body failed with status code {(int)response.StatusCode}, ({response.StatusCode}).";

                    try
                    {
                        var errorJson = JObject.Parse(responseContent);
                        errorMessage = $"Status code: {(int)response.StatusCode}, ({response.StatusCode}). {(string)errorJson["message"] ?? errorMessage}";
                    }
                    catch
                    {
                        // Handle JSON parse failure if needed
                    }

                    throw new HttpRequestException(errorMessage);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting from endpoint with body: {ex.Message}", ex);
            }
        }


        public static async Task<string> FindNestedValue(string endpoint, Dictionary<string, string> parameters, string searchKey, string searchValue, string targetKey, bool assert = true)
        {
            try
            {
                string targetValue = null;
                using (var client = new HttpClient())
                {
                    var baseUrl = _configuration.GetSection("API")["BaseUrl"];

                    if (parameters != null && parameters.Count > 0)
                    {
                        var queryString = string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
                        endpoint += $"?{queryString}";
                    }

                    HttpResponseMessage response = await client.GetAsync(baseUrl + endpoint);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Request to {endpoint} failed with status code {(int)response.StatusCode}, ({response.StatusCode}).";

                        var responseContent = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var errorJson = JObject.Parse(responseContent);
                            errorMessage = $"Status code: {(int)response.StatusCode}, ({response.StatusCode}). {(string)errorJson["message"] ?? errorMessage}";
                        }
                        catch
                        {
                            // Handle JSON parse failure if needed
                        }

                        throw new HttpRequestException(errorMessage);
                    }


                    var content = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(content);
                    JArray jsonArray = (JArray)jsonObject["content"];

                    bool keyFound = false;

                    foreach (JObject contentObject in jsonArray.Children<JObject>())
                    {
                        foreach (JToken token in contentObject.DescendantsAndSelf())
                        {
                            if (token is JProperty property && property.Name == searchKey && property.Value.ToString() == searchValue)
                            {
                                JObject parentObject = property.Parent as JObject;
                                JToken targetToken = parentObject.GetValue(targetKey, StringComparison.OrdinalIgnoreCase);

                                if (targetToken != null)
                                {
                                    targetValue = targetToken.ToString();
                                    keyFound = true;
                                    break;
                                }
                            }
                        }
                        if (keyFound)
                            break;
                    }

                    if (!keyFound)
                    {
                        throw new ArgumentException($"No object with key {searchKey} and value {searchValue} was found. Status code: {(int)response.StatusCode}, ({response.StatusCode}).");
                    }

                    if (targetValue == null)
                    {
                        throw new ArgumentException($"No value was found for target key {targetKey}. Status code: {(int)response.StatusCode}, ({response.StatusCode}).");
                    }

                    return targetValue;
                }
            }
            catch (ArgumentException ex)
            {
                if (assert)
                {
                    Assert.Fail(ex.Message);
                }
                return null;
            }
        }

        public static async Task<string> GetJsonFromEndpoint(string endpoint, Dictionary<string, string> parameters)
        {
            var query = new StringBuilder();

            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            foreach (var parameter in parameters)
            {
                if (query.Length > 0) query.Append('&');
                query.AppendFormat("{0}={1}", WebUtility.UrlEncode(parameter.Key), WebUtility.UrlEncode(parameter.Value));
            }

            var response = await client.GetAsync(baseUrl + endpoint + "?" + query);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Failed to get data from endpoint. Status code: {(int)response.StatusCode}, ({response.StatusCode}). Server says: {responseContent}";

                try
                {
                    var errorJson = JObject.Parse(responseContent);
                    errorMessage = $"Status code: {(int)response.StatusCode}, ({response.StatusCode}). {(string)errorJson["message"] ?? errorMessage}";
                }
                catch
                {
                    // Handle JSON parse failure if needed
                }

                throw new Exception(errorMessage);
            }

        }

        public static async Task AssertKeysNotNull(string endpoint, Dictionary<string, string> parameters, List<string> keys)
        {
            var baseUrl = _configuration.GetSection("API")["BaseUrl"];
            var builder = new UriBuilder(baseUrl + endpoint);

            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var parameter in parameters)
            {
                query[parameter.Key] = parameter.Value;
            }
            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject jsonResponse;
                try
                {
                    jsonResponse = JObject.Parse(content);
                }
                catch
                {
                    Assert.Fail($"Failed to parse JSON content. Status code: {(int)response.StatusCode}, ({response.StatusCode}).");
                    return;
                }

                foreach (var key in keys)
                {
                    var tokens = jsonResponse.SelectTokens($"$..{key}");
                    Assert.IsNotEmpty(tokens, $"Key '{key}' does not exist in the JSON. Status code: {(int)response.StatusCode}, ({response.StatusCode}).");

                    foreach (var token in tokens)
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(token.ToString()), $"Value for key '{key}' is null or empty. Status code: {(int)response.StatusCode}, ({response.StatusCode}).");
                    }
                }
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Response was not successful. Status code: {(int)response.StatusCode}, ({response.StatusCode}).";

                try
                {
                    var errorJson = JObject.Parse(responseContent);
                    errorMessage = $"{response.StatusCode}: {(string)errorJson["message"] ?? errorMessage}";
                }
                catch
                {
                    // Handle JSON parse failure if needed
                }

                Assert.Fail(errorMessage);
            }
        }

    }
}
