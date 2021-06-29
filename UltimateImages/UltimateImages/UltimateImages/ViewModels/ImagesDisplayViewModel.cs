﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using UltimateImages.Models;
using UltimateImages.Service;
using Xamarin.Forms;

namespace UltimateImages.ViewModels
{
    public class ImagesDisplayViewModel : BaseViewModel
    {
        private List<Hit> images;

        private Hit selectedImage;
        public Hit SelectedImage
        {
            get => selectedImage;
            set => SetProperty(ref selectedImage, value);
        }

        private int currentImageIndex;
        public int CurrentImageIndex
        {
            get => currentImageIndex;
            set => SetProperty(ref currentImageIndex, value);
        }

        private int totalImageCount;
        public int TotalImageCount
        {
            get => totalImageCount;
            set => SetProperty(ref totalImageCount, value);
        }

        public ICommand PreviousClickedCommand { get; private set; }
        public ICommand DownloadClickedCommand { get; private set; }
        public ICommand NextClickedCommand { get; private set; }       

        public ImagesDisplayViewModel(INavigation navigation, IEnumerable<Hit> images, Hit selectedImage) : base(navigation)
        {
            this.images = images.ToList();
            SelectedImage = selectedImage;

            PreviousClickedCommand = new Command(() => ExecutePreviousClickedCommand());
            DownloadClickedCommand = new Command(() => ExecuteDownloadClickedCommand());
            NextClickedCommand = new Command(() => ExecuteNextClickedCommand());

            CurrentImageIndex = this.images.IndexOf(SelectedImage) + 1;
            TotalImageCount = this.images.Count;
        }

        private void ExecuteNextClickedCommand()
        {
            if (CurrentImageIndex < TotalImageCount)
            {
                SelectedImage = images[++CurrentImageIndex - 1];
            }
        }

        private void ExecuteDownloadClickedCommand()
        {
            IDownloadService downloadService = DependencyService.Get<IDownloadService>(DependencyFetchTarget.NewInstance);

            downloadService.DownloadFile(SelectedImage.largeImageURL, "UltimateImages");
            downloadService.OnFileDownloaded += OnFileDownloaded;
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                ToastService.ShowLongAlert("Downloaded successfully");
            }
            else
            {
                ToastService.ShowLongAlert("Download failed");
            }
        }

        private void ExecutePreviousClickedCommand()
        {
            if (CurrentImageIndex - 1 > 0)
            {
                SelectedImage = images[--CurrentImageIndex - 1];
            }
        }
    }
}
