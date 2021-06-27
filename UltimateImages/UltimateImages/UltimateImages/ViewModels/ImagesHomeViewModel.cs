using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UltimateImages.Models;
using UltimateImages.Service;
using UltimateImages.Views;
using Xamarin.Forms;

namespace UltimateImages.ViewModels
{
    public class ImagesHomeViewModel : BaseViewModel
    {
        public enum ScreenState
        {
            None,
            CalledAPI,
            EmptyResponseReceived,
            CorrectResponseReceived,
            Error
        }

        public ICommand LoadImagesCommand { get; set; }
        public ICommand SearchClickedCommand { get; set; }
        public ICommand ImageSelectedCommand { get; set; }
        public ICommand PreviousClickedCommand { get; private set; }
        public ICommand NextClickedCommand { get; private set; }


        private string searchText = string.Empty;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private ScreenState state;
        public ScreenState State
        {
            get => state;
            set => SetProperty(ref state, value, onChanged: OnStateChange);
        }

        private bool isStatusVisible = true;
        public bool IsStatusVisible
        {
            get => isStatusVisible;
            set 
            {
                IsDataVisible = !value;
                SetProperty(ref isStatusVisible, value); 
            }
        }
        private bool isDataVisible;
        public bool IsDataVisible
        {
            get => isDataVisible;
            set => SetProperty(ref isDataVisible, value);
        }
        private string statusMessage;
        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }
        
        private int currentPageNo;
        public int CurrentPageNo
        {
            get => currentPageNo;
            set => SetProperty(ref currentPageNo, value);
        }

        private int totalPageCount;
        public int TotalPageCount
        {
            get => totalPageCount;
            set => SetProperty(ref totalPageCount, value);
        }
        private int perPage = 5;
        public int PerPage
        {
            get => perPage;
            set => SetProperty(ref perPage, value);
        }

        public ObservableCollection<Hit> Images { get; set; }

        public ImagesHomeViewModel(INavigation navigation) : base(navigation)
        {
            LoadImagesCommand = new Command(async () => await ExecuteLoadImagesCommand());
            SearchClickedCommand = new Command(async () => await ExecuteSearchClickedCommand());
            ImageSelectedCommand = new Command<Hit>(async (selectedImage) => await ExecuteImageSelectedCommand(selectedImage));
            Images = new ObservableCollection<Hit>();

            PreviousClickedCommand = new Command(() => ExecutePreviousClickedCommand());
            NextClickedCommand = new Command(() => ExecuteNextClickedCommand());

        }

        private async Task ExecuteImageSelectedCommand(Hit selectedImage)
        {
            if (selectedImage != null)
            {
                await Navigation.PushAsync(new ImagesDisplay(new ImagesDisplayViewModel(Navigation, Images, selectedImage)));
            }
        }

        private async Task ExecuteLoadImagesCommand()
        {
            if (Images.Count == 0)
            {
                State = ScreenState.None;
                OnStateChange();
            }
        }

        private async Task ExecuteSearchClickedCommand()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                CurrentPageNo = 1;
                ReadImageDetails();
            }
        }

        private async Task ReadImageDetails()
        {
            try
            {
                State = ScreenState.CalledAPI;
                PixabayService service = new PixabayService();

                var pixabayResponse = await service.GetImages(new PixabayRequestModel()
                {
                    Querry = SearchText,
                    PageNo = CurrentPageNo,
                    PerPage = PerPage
                });

                if (pixabayResponse == null || pixabayResponse.hits.Length == 0)
                {
                    State = ScreenState.EmptyResponseReceived;
                }
                else
                {
                    TotalPageCount = (int)Math.Ceiling(1.0 * (pixabayResponse.totalHits ?? 0) / PerPage);
                    Images.Clear();

                    AddRange(Images, pixabayResponse.hits);
                    State = ScreenState.CorrectResponseReceived;
                }

            }
            catch
            {
                State = ScreenState.Error;
            }
        }

        private void AddRange<T>(ObservableCollection<T> colection, IEnumerable<T> elements)
        {
            foreach (T item in elements)
            {
                colection.Add(item);
            }
        }

        private void OnStateChange()
        {
            switch (State)
            {
                case ScreenState.None:
                    StatusMessage = "Enter something and search. Try Nature.";
                    IsStatusVisible = true;
                    break;
                case ScreenState.CalledAPI:
                    StatusMessage = "Please wait images are loading";
                    IsStatusVisible = true;
                    break;
                case ScreenState.EmptyResponseReceived:
                    StatusMessage = "No result try somthing else.";
                    IsStatusVisible = true;
                    break;
                case ScreenState.CorrectResponseReceived:
                    StatusMessage = string.Empty;
                    IsStatusVisible = false;
                    break;
                case ScreenState.Error:
                    StatusMessage = "Something went wrong. Please try again later.";
                    IsStatusVisible = true;
                    break;
            }
        }

        private void ExecuteNextClickedCommand()
        {
            if (CurrentPageNo < TotalPageCount)
            {
                CurrentPageNo++;
                ReadImageDetails();
            }
        }

        private void ExecutePreviousClickedCommand()
        {
            if (CurrentPageNo > 1)
            {
                CurrentPageNo--;
                ReadImageDetails();
            }
        }
    }
}
