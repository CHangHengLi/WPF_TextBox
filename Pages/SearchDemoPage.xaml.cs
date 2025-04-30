using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace TextBoxDemo.Pages
{
    public partial class SearchDemoPage : Page
    {
        public SearchDemoPage()
        {
            InitializeComponent();
            DataContext = new SearchViewModel();
        }
    }
    
    public class SearchViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _allItems;
        private ObservableCollection<string> _filteredItems;
        private string _searchTerm;
        
        public SearchViewModel()
        {
            // 初始化数据
            _allItems = new ObservableCollection<string>
            {
                "苹果", "香蕉", "橙子", "草莓", "葡萄", 
                "西瓜", "菠萝", "芒果", "樱桃", "蓝莓"
            };
            
            _filteredItems = new ObservableCollection<string>(_allItems);
        }
        
        public ObservableCollection<string> FilteredItems
        {
            get { return _filteredItems; }
            set 
            { 
                _filteredItems = value; 
                OnPropertyChanged(nameof(FilteredItems));
            }
        }
        
        public string SearchTerm
        {
            get { return _searchTerm; }
            set 
            { 
                _searchTerm = value; 
                OnPropertyChanged(nameof(SearchTerm));
                
                // 根据搜索词过滤项目
                FilterItems();
            }
        }
        
        private void FilterItems()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                // 如果搜索词为空，显示所有项目
                FilteredItems = new ObservableCollection<string>(_allItems);
            }
            else
            {
                // 根据搜索词过滤项目
                var filtered = _allItems.Where(i => i.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                FilteredItems = new ObservableCollection<string>(filtered);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 