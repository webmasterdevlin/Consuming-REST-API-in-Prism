using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RestApiPrism.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApiPrism.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private bool canDo;
        private readonly IPageDialogService _pageDialogService;
        private const string Url = "http://jsonplaceholder.typicode.com/posts";

        private readonly HttpClient _client = new HttpClient();

        private ObservableCollection<Post> _posts;

        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            set { SetProperty(ref _posts, value); }
        }

        public MainPageViewModel(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
        }

        public DelegateCommand AddCommand => new DelegateCommand(Add);

        private void Add()
        {
            var post = new Post { Title = $"Title {DateTime.UtcNow.Ticks}" };
            var content = JsonConvert.SerializeObject(post);
            Posts.Insert(0, post);
            _client.PostAsync(Url, new StringContent(content));
        }

        public DelegateCommand UpdateCommand => new DelegateCommand(Update);

        private void Update()
        {
            var post = Posts[0];
            post.Title += " [updated]";
            var content = JsonConvert.SerializeObject(post);
            _client.PutAsync(Url + "/" + post.Id, new StringContent(content));
        }

        public DelegateCommand DeleteCommand => new DelegateCommand(Delete);

        private async void Delete()
        {
            var confirmation = await _pageDialogService.DisplayAlertAsync("Alert", "You sure?","Delete","Cancel");
            if (!confirmation) return;
            var post = Posts[0];
            Posts.Remove(post);
            await _client.DeleteAsync(Url + "/" + post.Id);
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            var content = await _client.GetStringAsync(Url);
            var posts = JsonConvert.DeserializeObject<List<Post>>(content);
            Posts = new ObservableCollection<Post>(posts);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }
    }
}