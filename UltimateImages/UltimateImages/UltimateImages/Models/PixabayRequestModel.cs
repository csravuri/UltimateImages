using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;

namespace UltimateImages.Models
{
    public class PixabayRequestModel
    {
        private static string apiKey = "key";
        private static string baseAddress = "https://pixabay.com/api/";

        private string querry = null;        
        public string Querry 
        {
            get => querry;
            set => querry = GetURLEncoded(value); 
        }

        public string Category { get; set; }
        public int PageNo { get; set; }
        public int PerPage { get; set; }

        public string GetRequestURI()
        {
            StringBuilder requestURI = new StringBuilder(baseAddress);

            requestURI.Append("?key=");
            requestURI.Append(apiKey);
            
            requestURI.Append(GetParameter("q", Querry));
            requestURI.Append(GetParameter("category", Category));            
            requestURI.Append(GetParameter("page", PageNo));            
            requestURI.Append(GetParameter("per_page", PerPage));            

            return requestURI.ToString();
        }

        private string GetURLEncoded(string normal)
        {
            if (string.IsNullOrWhiteSpace(normal))
                return string.Empty;

            return HttpUtility.UrlEncode(normal);
        }

        private string GetParameter(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return $"&{name}={value}";
            }

            return string.Empty;
        }

        private string GetParameter(string name, int value)
        {
            if (value != 0)
            {
                return $"&{name}={value}";
            }

            return string.Empty;
        }

    }
}
