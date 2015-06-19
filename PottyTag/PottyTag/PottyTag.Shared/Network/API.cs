using Newtonsoft.Json;
using PottyTag.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace PottyTag.Network
{
    class API
    {
        public static API Current = new API();
        private static string BASE = "http://spiralpower.net/pottytag/api/?";

        private API()
        {
        }

        public async Task<BathroomStatus> GetStatus()
        {
            BathroomStatus bathroomStatus = null;                        
            string url = BASE + "r=status";
            var response = await Get(url);

            Debug.WriteLine("GetStatus() response: " + response);

            if (!string.IsNullOrEmpty(response))
            {
                bathroomStatus = JsonConvert.DeserializeObject<BathroomStatus>(response);
            }

            return bathroomStatus;
        }

        public async Task<bool> CheckIn()
        {
            string gender = Settings.Current.Gender;
            string id = Settings.Current.Id;
            bool success = false;

            string url = BASE + "r=action&action=checkin&gender=" + gender;

            if (!string.IsNullOrEmpty(id))
            {
                url += "&last_checkin=" + id;
            }

            var response = await Get(url);

            if (!string.IsNullOrEmpty(response))
            {
                CheckInResponse checkInResponse = JsonConvert.DeserializeObject<CheckInResponse>(response);
                
                if (checkInResponse.Success)
                {
                    success = true;
                    Settings.Current.Id = checkInResponse.Id.ToString();
                    Debug.WriteLine("[API] CheckIn() id: " + Settings.Current.Id);
                }
            }

            return success;
        }

        public async Task<bool> CheckOut()
        {
            string gender = Settings.Current.Gender;
            string id = Settings.Current.Id;
            bool success = false;
            
            if (!string.IsNullOrEmpty(id))
            {
                string url = BASE + "r=action&action=checkout&last_checkin=" + id;
                var response = await Get(url);

                if (!string.IsNullOrEmpty(response) && response.Contains("true"))
                {
                    success = true;
                    Settings.Current.Id = null;
                }
            }

            return success;
        }

        public async Task<bool> DisableToilet(bool isToiletOne)
        {
            bool success = false;

            string url = BASE + "r=action&action=addflag&toilet_id=" + (isToiletOne ? "0" : "1");
            var response = await Get(url);

            if (!string.IsNullOrEmpty(response) && response.Contains("true"))
            {
                success = true;
            }

            return success;
        }

        public async Task<bool> EnableToilet(bool isToiletOne)
        {
            bool success = false;

            string url = BASE + "r=action&action=removeflag&toilet_id=" + (isToiletOne ? "0" : "1");
            var response = await Get(url);

            if (!string.IsNullOrEmpty(response) && response.Contains("true"))
            {
                success = true;
            }

            return success;
        }

        private async Task<string> Get(string url)
        {
            string responseString = null;
            HttpClient httpClient = null;

            try
            {
                httpClient = new HttpClient();
                
                if (!url.Contains("?")) {
                    url += "?";
                }

                url += "&random=" + DateTime.Now.Ticks;

                Debug.WriteLine("[API] Get(): " + url);

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                var response = await httpClient.SendRequestAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Debug.WriteLine("[API] GetStatus() failed. Response code: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("[API] Exception in GetStatus(): " + e.StackTrace);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }

            return responseString;
        }
    }
}
