using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private FlickrViewModel _flickrModel;
       private LinksUteisViewModel _linksUteisModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = FlickrModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public FlickrViewModel FlickrModel
        {
            get { return _flickrModel ?? (_flickrModel = new FlickrViewModel()); }
        }
 
        public LinksUteisViewModel LinksUteisModel
        {
            get { return _linksUteisModel ?? (_linksUteisModel = new LinksUteisViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            FlickrModel.ViewType = viewType;
            LinksUteisModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

      get { return Visibility.Collapsed; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadDataAsync(bool forceRefresh = false)
        {
            var loadTasks = new Task[]
            { 
                FlickrModel.LoadItemsAsync(forceRefresh),
                LinksUteisModel.LoadItemsAsync(forceRefresh),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}
