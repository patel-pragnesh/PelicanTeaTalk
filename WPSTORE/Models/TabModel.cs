using MvvmHelpers;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using WPSTORE.Models.WooCommerce;

namespace WPSTORE.Models
{
    public class TabModel: BindableBase
    {
        public TabModel()
        {
            CurrentPage = 1;
            ItemThreshold = -1;
            PageSize = GlobalSettings.AppConst.PageSize;
            Products = new ObservableRangeCollection<ProductModel>();
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }
        public int PageSize { get; private set; }

        private int _itemThreshold;
        public int ItemThreshold
        {
            get { return _itemThreshold; }
            set { SetProperty(ref _itemThreshold, value); }
        }
        
        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private int _categoryId;
        public int CategoryId
        {
            get => _categoryId;
            set => SetProperty(ref _categoryId, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private bool _alreadyRefreshed;
        public bool AlreadyRefreshed
        {
            get => _alreadyRefreshed;
            set => SetProperty(ref _alreadyRefreshed, value);
        }

        ObservableRangeCollection<ProductModel> _products;
        public ObservableRangeCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
    }
}
