using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Business.ViewModel;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.Helpers
{
    public   class HelperUrlApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HelperUrlApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];

        }
        //public async Task<T> GetDataFromApi<T>(string url)
        //{
        //    try
        //    {
        //        _httpClient.BaseAddress = new Uri(_baseUrl);
        //        _httpClient.DefaultRequestHeaders.Clear();
        //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        //        var response = await _httpClient.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonData = await response.Content.ReadAsStringAsync();
        //            var res= Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
        //            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Error fetching data from API: {url}, Status Code: {response.StatusCode}");
        //            return default;
        //        }
        //    }
        //    catch (Exception ex)
            
        //    {
        //        Console.WriteLine($"Exception while calling API: {url}, Exception: {ex.Message}");
        //        return default;
        //    }
        //}


        public async Task<T> GetDataFromApi<T>(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_baseUrl), url));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
                }
                else
                {
                    Console.WriteLine($"Error fetching data from API: {url}, Status Code: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while calling API: {url}, Exception: {ex.Message}");
                return default;
            }
        }
        public async Task<T> GetDataFromApiNewHttpClient<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Set the base address for HttpClient
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching data from API: {url}, Status Code: {response.StatusCode}");
                        return default;
                    }
                }
            }
            catch (Exception ex)

            {
                Console.WriteLine($"Exception while calling API: {url}, Exception: {ex.Message}");
                return default;
            }
        }
        public async Task<(T1, T2)> GetMultipleDataFromApi<T1, T2>(string url1, string url2)
        {
            try
            {
                // Initialize HttpClient only once
                using (var client = new HttpClient())
                {
                    // Set the base address for HttpClient
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Create tasks for both API calls
                    var request1 = client.GetAsync(url1);
                    var request2 = client.GetAsync(url2);

                    // Wait for both requests to complete concurrently
                    await Task.WhenAll(request1, request2);

                    // Handle the responses for both URLs
                    var response1 = await request1.Result.Content.ReadAsStringAsync();
                    var response2 = await request2.Result.Content.ReadAsStringAsync();

                    // Deserialize the responses into the respective types
                    var result1 = JsonConvert.DeserializeObject<T1>(response1);
                    var result2 = JsonConvert.DeserializeObject<T2>(response2);

                    // Return the results as a tuple
                    return (result1, result2);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions and return default values for the types
                Console.WriteLine($"Exception while calling multiple APIs: {ex.Message}");
                return (default, default);
            }
        }

        public async Task<(List<SelectListItem>, List<FileUploadConfigVM>)> GetMultipleDataFromApiWithSelectListHandling(string url1, string url2)
        {
            try
            {
                // Initialize HttpClient only once
                using (var client = new HttpClient())
                {
                    // Set the base address for HttpClient
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Create tasks for both API calls
                    var request1 = client.GetAsync(url1);
                    var request2 = client.GetAsync(url2);

                    // Wait for both requests to complete concurrently
                    await Task.WhenAll(request1, request2);

                    // Handle the responses for both URLs
                    var response1 = await request1.Result.Content.ReadAsStringAsync();
                    var response2 = await request2.Result.Content.ReadAsStringAsync();

                    // Deserialize the response for activities (SelectListItem)
                    var activitiesList = new List<SelectListItem>();
                    var activitiesResponse = JsonConvert.DeserializeObject<JObject>(response1);

                    if (activitiesResponse != null)
                    {
                        var values = activitiesResponse["$values"];
                        if (values != null)
                        {
                            // Add default 'Select Activity' option at the start of the list
                            activitiesList.Add(new SelectListItem
                            {
                                Value = "0", // Set this value to 0 or any default value
                                Text = "إختر نشاط",
                                Selected = true,  // Make it selected by default
                                Disabled = true   // Option is disabled to prevent selection
                            });

                            // Create SelectListItems from the response
                            activitiesList.AddRange(values
                                .Select(item => new SelectListItem
                                {
                                    Value = item["id"].ToString(),
                                    Text = item["nameAr"].ToString(),

                                })
                                .ToList());
                        }
                        else
                        {
                            // Handle invalid data structure
                            activitiesList.Add(new SelectListItem { Text = "Invalid Data", Value = "0" });
                        }
                    }

                    // Deserialize the response for file upload configurations
                    var fileUploadConfigs = JsonConvert.DeserializeObject<List<FileUploadConfigVM>>(response2);

                    // Return both activities and file upload configurations as a tuple
                    return (activitiesList, fileUploadConfigs);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions and return empty lists for the types
                Console.WriteLine($"Exception while calling multiple APIs: {ex.Message}");
                return (new List<SelectListItem>(), new List<FileUploadConfigVM>());
            }
        }


        // Retry logic helper method for a single API call
        private async Task<T> GetDataWithRetry<T>(string url, int maxRetries = 3, int delayMilliseconds = 2000)
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    var result = await GetDataFromApi<T>(url); // Using your existing method to fetch data
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Console.WriteLine($"Retry {attempt} failed for URL: {url}, Error: {ex.Message}");

                    if (attempt < maxRetries)
                    {
                        await Task.Delay(delayMilliseconds); // Delay before retrying
                    }
                }
            }

            // After max retries, throw the last exception encountered
            throw new Exception($"Failed to retrieve data after {maxRetries} attempts.", lastException);
        }

        public async Task<TResponse> PostDataToApi<TRequest, TResponse>(string url, TRequest requestData, string? actionType=null)
        {
            try
            {
                //_httpClient.BaseAddress = new Uri(_baseUrl);
                //_httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add action type to the request body
                var requestPayload = new
                {
                    ActionType = actionType,
                    Data = requestData
                };
                // Serialize the payload with settings to ignore null values
                string serializedData = actionType == null
                    ? Newtonsoft.Json.JsonConvert.SerializeObject(requestData, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })
                    : Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                // Prepare the content
                var jsonContent = new StringContent(serializedData, Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync(url, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(jsonData);
                }
                else
                {
                    Console.WriteLine($"Error in API request: {url}, Status Code: {response.StatusCode}");
                    return default;
                    //var errorResponse = await response.Content.ReadAsStringAsync();
                    //return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while calling API: {url}, Exception: {ex.Message}");
                return default;
            }
        }
        public async Task<bool> DeleteDataFromApi<T>(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting data from API: {ex.Message}");
                return false;
            }
        }
    }
}
