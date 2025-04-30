using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace TextBoxDemo.Pages
{
    /// <summary>
    /// DataBindingPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataBindingPage : Page
    {
        private PersonViewModel _person;

        public DataBindingPage()
        {
            InitializeComponent();
            
            // 创建视图模型并设置为DataContext
            _person = new PersonViewModel
            {
                Name = "张三",
                Email = "zhangsan@example.com",
                Age = 25,
                IsActive = true,
                Price = 1299.99,
                Address = null,//可以修改此处的Address值来通过热重载改变显示的地址
                BirthDate = new DateTime(1998, 5, 15)
            };
            
            this.DataContext = _person;
        }

        private void UpdateEmail_Click(object sender, RoutedEventArgs e)
        {
            // 更新Email属性，用于演示不同绑定模式的效果
            _person.Email = $"updated_{_person.Email}";
            MessageBox.Show($"Email已更新为: {_person.Email}\n\n请观察不同绑定模式的TextBox显示效果。", "更新完成");
        }

        private void UpdateExplicit_Click(object sender, RoutedEventArgs e)
        {
            // 获取显式绑定表达式并更新源
            BindingExpression binding = txtExplicit.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
            MessageBox.Show($"已手动更新源数据，当前Age值: {_person.Age}", "显式更新");
        }
    }

    /// <summary>
    /// 人员信息视图模型
    /// </summary>
    public class PersonViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _email;
        private int _age;
        private bool _isActive;
        private double _price;
        private string _address;
        private DateTime _birthDate;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 布尔值转换为"是/否"文本的转换器
    /// </summary>
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "是" : "否";
            }
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue == "是";
            }
            return false;
        }
    }

    /// <summary>
    /// 字符串转换为大写的转换器
    /// </summary>
    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue.ToUpper();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 不支持反向转换
            return value;
        }
    }

    /// <summary>
    /// 价格转换器，用于处理带有货币符号的价格格式
    /// </summary>
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double price)
            {
                return string.Format(culture, "￥{0:N2}", price);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                // 移除非数字字符（保留小数点和数字）
                strValue = Regex.Replace(strValue, @"[^0-9\.]", "");
                
                if (double.TryParse(strValue, NumberStyles.Any, culture, out double result))
                {
                    return result;
                }
            }
            // 转换失败返回默认值
            return 0.0;
        }
    }

    /// <summary>
    /// 日期转换器，用于处理中文格式的日期
    /// </summary>
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("yyyy年MM月dd日");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                // 尝试解析标准格式
                if (DateTime.TryParse(strValue, culture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                
                // 尝试解析中文格式（yyyy年MM月dd日）
                var match = Regex.Match(strValue, @"(\d{4})年(\d{1,2})月(\d{1,2})日");
                if (match.Success && match.Groups.Count >= 4)
                {
                    int year = int.Parse(match.Groups[1].Value);
                    int month = int.Parse(match.Groups[2].Value);
                    int day = int.Parse(match.Groups[3].Value);
                    
                    try
                    {
                        return new DateTime(year, month, day);
                    }
                    catch
                    {
                        // 日期无效
                    }
                }
            }
            
            // 转换失败返回当前日期
            return DateTime.Now;
        }
    }
} 