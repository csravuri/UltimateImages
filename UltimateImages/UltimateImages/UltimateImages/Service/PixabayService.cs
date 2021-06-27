using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UltimateImages.Database;
using UltimateImages.Models;

namespace UltimateImages.Service
{
    public class PixabayService
    {
        public PixabayService(int cacheDurationDays = 1)
        {
            this.cacheDurationDays = cacheDurationDays;
        }
        
        private readonly int cacheDurationDays;
        public async Task<PixabayResponseModel> GetImages(PixabayRequestModel request)
        {
            string requestUri = request.GetRequestURI();

            #region Try from cache
            
            PixabayResponseModel cachedResponse = await GetCachedResponse(requestUri);

            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            #endregion

            #region Try from api

            using (HttpClient client = new HttpClient())
            {
                var responseTask = client.GetAsync(requestUri);

                try
                {
                    responseTask.Wait(-1);
                }
                catch (AggregateException)
                {

                }
                catch (Exception)
                {

                }

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var uploadResponse = readTask.Result;

                    await CacheResponse(requestUri, uploadResponse);

                    PixabayResponseModel response = JsonConvert.DeserializeObject<PixabayResponseModel>(uploadResponse);

                    return response;
                }
            }

            #endregion

            return null;
        }

        private async Task<PixabayResponseModel> GetCachedResponse(string requestUri)
        {
            DBConnect dBConnect = DBConnect.GetDBConnect();

            APIResponseCache aPICachedResponse = (await dBConnect.GetAPIResponses()).Where(x => string.Compare(requestUri, x.RequestURI, true) == 0).FirstOrDefault();

            if (aPICachedResponse != null && aPICachedResponse.ExpiryDate > DateTime.Now)
            {
                PixabayResponseModel response = JsonConvert.DeserializeObject<PixabayResponseModel>(aPICachedResponse.Response);

                return response;
            }

            return null;
        }

        private async Task CacheResponse(string requestUri, string response)
        {
            DBConnect dBConnect = DBConnect.GetDBConnect();

            APIResponseCache aPICachedResponse = (await dBConnect.GetAPIResponses()).Where(x => string.Compare(requestUri, x.RequestURI, true) == 0).FirstOrDefault();

            if (aPICachedResponse != null)
            {
                aPICachedResponse.Response = response;
                aPICachedResponse.ModifiedDate = DateTime.Now;
                aPICachedResponse.ExpiryDate = DateTime.Now.AddDays(cacheDurationDays);

                await dBConnect.UpdateRecord(aPICachedResponse);
            }
            else
            {
                APIResponseCache aPIResponse = new APIResponseCache()
                {
                    CreatedDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(cacheDurationDays),
                    RequestURI = requestUri,
                    Response = response                    
                };

                await dBConnect.InsertRecord(aPIResponse);
            }
        }
    }
}
