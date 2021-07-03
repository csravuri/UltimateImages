using Android.App;
using Android.Content;
using System;
using System.IO;
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
            if(await PermissionsExists())
            {
                try
                {
                    string fileName = Path.GetFileName(url);

                    if (fileName.Length > 10)
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileName).Substring(0, 10) + Path.GetExtension(fileName);
                    }

                    var source = Android.Net.Uri.Parse(url);
                    DownloadManager.Request request = new DownloadManager.Request(source);
                    request.SetAllowedOverRoaming(false);
                    request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                    request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, Path.Combine(folderName, fileName));
                    var manager = (DownloadManager)Application.Context.GetSystemService(Context.DownloadService);
                     
                    manager.Enqueue(request);
                }
                catch
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
    }
}