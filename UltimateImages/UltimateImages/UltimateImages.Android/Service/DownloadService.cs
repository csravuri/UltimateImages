using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UltimateImages.Droid.Service;
using UltimateImages.Service;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(DownloadService))]
namespace UltimateImages.Droid.Service
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public async Task DownloadFile(string url, string folderName)
        {
            

            string folderPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, folderName);
                        
            if(await PermissionsExists())
            {
                try
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    WebClient webClient = new WebClient();
                    string fileName = Path.GetFileName(url);

                    if (fileName.Length > 10)
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileName).Substring(0, 10) + Path.GetExtension(fileName);
                    }

                    string filePath = Path.Combine(folderPath, fileName);

                    webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(
                        (sender, e) => OnDownloadComplete(e, filePath));

                    webClient.DownloadDataAsync(new Uri(url), filePath);

                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(
                    //    (sender, e) => OnFileDownloaded?.Invoke(this, new DownloadEventArgs(e.Error == null, filePath)));

                    //webClient.DownloadFileAsync(new Uri(url), filePath);
                }
                catch (Exception ex)
                {
                    OnFileDownloaded?.Invoke(this, new DownloadEventArgs(false));
                }
            }            
        }

        private async Task<bool> PermissionsExists()
        {            
            var readStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (readStatus == PermissionStatus.Denied)
            {
                readStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            var writeStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (writeStatus == PermissionStatus.Denied)
            {
                writeStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            return (writeStatus == PermissionStatus.Granted) && (readStatus == PermissionStatus.Granted);
        }

        private void OnDownloadComplete(DownloadDataCompletedEventArgs e, string filePath)
        {
            File.WriteAllBytes(filePath, e.Result);
            OnFileDownloaded?.Invoke(this, new DownloadEventArgs(e.Error == null, filePath));
        }
    }
}