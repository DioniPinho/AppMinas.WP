using System;
using System.Net.NetworkInformation;

using AppStudio.Services;
using AppStudio.ViewModels;

using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Views
{
    public sealed partial class FlickrDetail : Page
    {
        private NavigationHelper _navigationHelper;

        private DataTransferManager _dataTransferManager;

        private DisplayOrientations _currentOrientations;

        public FlickrDetail()
        {
            this.InitializeComponent();
            _navigationHelper = new NavigationHelper(this);

            FlickrModel = new FlickrViewModel();
        }

        public FlickrViewModel FlickrModel { get; private set; }

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            _navigationHelper.OnNavigatedTo(e);

            if (FlickrModel != null)
            {
                await FlickrModel.LoadItemsAsync();
                FlickrModel.SelectItem(e.Parameter);

                FlickrModel.ViewType = ViewTypes.Detail;
            }
            DataContext = this;

            // Allow this page to rotate
            _currentOrientations = DisplayInformation.AutoRotationPreferences;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait
                                                        | DisplayOrientations.Landscape
                                                        | DisplayOrientations.LandscapeFlipped
                                                        | DisplayOrientations.PortraitFlipped;

            // Handle orientation changes
            DisplayInformation.GetForCurrentView().OrientationChanged += this.OnOrientationChanged;
            this.TransitionStoryboardState();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
            _dataTransferManager.DataRequested -= OnDataRequested;

            // Restore previous rotation preferences
            DisplayInformation.AutoRotationPreferences = _currentOrientations;

            // Handle orientation changes
            DisplayInformation.GetForCurrentView().OrientationChanged -= this.OnOrientationChanged;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (FlickrModel != null)
            {
                FlickrModel.GetShareContent(args.Request);
            }
        }

        private void OnOrientationChanged(DisplayInformation sender, object args)
        {
            this.TransitionStoryboardState();
        }

        private void TransitionStoryboardState()
        {
            string displayOrientation;

            switch (DisplayInformation.GetForCurrentView().CurrentOrientation)
            {
                case DisplayOrientations.Portrait:
                case DisplayOrientations.PortraitFlipped:
                    displayOrientation = "Portrait";
                    break;

                case DisplayOrientations.Landscape:
                case DisplayOrientations.LandscapeFlipped:
                default:
                    displayOrientation = "Landscape";
                    break;
            }

            VisualStateManager.GoToState(this, displayOrientation, false);
        }
    }
}
