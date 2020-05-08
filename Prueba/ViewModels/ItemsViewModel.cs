using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Prueba.Models;
using Prueba.Views;
using Prism.Navigation;
using System.Linq;

namespace Prueba.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<UserInfo> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItem { get; set; }

        public ItemsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Users";
            Items = new ObservableCollection<UserInfo>();
            LoadItemsCommand = new Command(async () => { await LoadItemsExecute(); });
            AddItem = new Command(async () => { await AddItemExecute(); });
        }

        private async Task AddItemExecute()
        {
            await NavigationService.NavigateAsync(nameof(NewItemPage));
        }

        private async Task LoadItemsExecute()
        {
            await GetInfoData();
        }

        private async Task GetInfoData()
        {
            IsBusy = true;
            var userResponse = await ApiService.Get<User>("", "?results=50");
            
            Items.Clear();
            foreach (var userInfo in userResponse.Results)
            {
                Items.Add(new UserInfo
                {
                    FullName = $"{userInfo.Name.Title.ToString()} {userInfo.Name.First} {userInfo.Name.Last}",
                    City = userInfo.Location.City,
                    Email = userInfo.Email,
                    ProfileImage = userInfo.Picture.Thumbnail
                });
            }
            var localData = await DataBaseService.GetItemsAsync<UserInfo>();
            if (localData.Any())
            {
                foreach (var userInfo in localData)
                {
                    Items.Add(userInfo);
                }
            }

           
            IsBusy = false;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await GetInfoData();
        }
    }
}