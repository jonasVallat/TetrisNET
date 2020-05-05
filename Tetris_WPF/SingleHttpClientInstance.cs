using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WpfApp1
{
    class SingleHttpClientInstance
    {

        private const string URI = "https://localhost:44395/";
        private static readonly HttpClient httpClient;

        static SingleHttpClientInstance()
        {
            httpClient = new HttpClient();
        }

        // This method uses the shared instance of HttpClient for every call to GetProductAsync.
        static public Tuple<bool, string> Login(string email, string passwod)
        {

            var values = new Dictionary<string, string>
                            {
                                { "grant_type", "password" },
                                { "username", email },
                                {"password" ,  passwod}
                            };

            var content = new FormUrlEncodedContent(values);
            var requestURI = $"{URI}/Token";
            var response = httpClient.PostAsync(requestURI, content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(responseString);


            dynamic json = JsonConvert.DeserializeObject(responseString);

            if(json.error_description != null)
            {
                string error = json.error_description;
                return new Tuple<bool, string>(true, error);
            }

            string token = json.access_token;
            return new Tuple<bool, string>(false, token);

        }

        static public void AddScore(int score)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{URI}/scores"));
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {App.Current.Properties["token"]}");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                            {
                                { "value", score.ToString() }
                            });

            var response = httpClient.SendAsync(request).Result;
        }

        static public List<string> GetScore()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{URI}/scores"));
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {App.Current.Properties["token"]}");

            var response = httpClient.SendAsync(request).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<string>>(responseString);
        }

        static public Tuple<bool, string> Register(string email, string password, string confirmPassword)
        {
            string json = $"{{\"Email\": \"{email}\",\"Password\": \"{password}\",\"ConfirmPassword\": \"{confirmPassword}\"}}";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{URI}/scores"));
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {App.Current.Properties["token"]}");

            var response = httpClient.PostAsync($"{URI}/api/Account/Register", new StringContent(json, Encoding.UTF8, "application/json")).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            if(!response.IsSuccessStatusCode)
            {
                var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                // Deserialize:
                var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(responseString, anonymousErrorObject);

                // Check if there are actually model errors
                if (deserializedErrorObject.ModelState != null)
                {
                    var errors =
                        deserializedErrorObject.ModelState
                                                .Select(kvp => string.Join("\n ", kvp.Value));


                    return new Tuple<bool, string>(true, errors.FirstOrDefault());

                }
                // Othertimes, there may not be Model Errors:
                else
                {
                    var error =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                    return new Tuple<bool, string>(true, "salut");
                }
            }
            return new Tuple<bool, string>(false, "");
        }
    }
}
