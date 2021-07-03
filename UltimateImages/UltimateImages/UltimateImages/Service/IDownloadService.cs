using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UltimateImages.Service
{
    public interface IDownloadService
    {
        Task DownloadFile(string url, string folderName);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }

    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;
        public string FilePath;
        public DownloadEventArgs(bool fileSaved, string filePath = "")
        {
            FileSaved = fileSaved;
            FilePath = filePath;
        }
    }
}
