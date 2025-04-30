using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TextBoxDemo.Pages
{
    /// <summary>
    /// DataValidationPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataValidationPage : Page, INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        private string _age;
        private string _password;
        private string _email;
        private string _quantity;
        private string _username;
        private string _phoneNumber;
        private string _creditCardNumber;
        private string _expiryDate;
        private string _postalCode;
        
        // 用于存储属性验证错误
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();

        // 用于跟踪哪些字段已经被编辑过
        private readonly Dictionary<string, bool> _editedFields = new Dictionary<string, bool>();

        public DataValidationPage()
        {
            InitializeComponent();
            DataContext = this;
            
            // 添加示例TextBox的事件处理
            Loaded += (s, e) => 
            {
                // 查找并添加示例验证控件
                AddSampleValidationControls();
            };
        }

        private void AddSampleValidationControls()
        {
            // 这个方法可以用来添加额外的验证示例控件，如果需要的话
        }

        #region INotifyPropertyChanged 实现
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IDataErrorInfo 实现
        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                string error = null;
                
                // 只验证已经编辑过的字段
                if (!_editedFields.ContainsKey(propertyName) || !_editedFields[propertyName])
                    return null;
                
                switch (propertyName)
                {
                    case nameof(Username):
                        if (string.IsNullOrEmpty(Username))
                            error = "用户名不能为空";
                        else if (Username.Length < 3)
                            error = "用户名长度不能少于3个字符";
                        break;
                        
                    case nameof(PhoneNumber):
                        if (!string.IsNullOrEmpty(PhoneNumber) && !Regex.IsMatch(PhoneNumber, @"^1[3-9]\d{9}$"))
                            error = "请输入有效的手机号码";
                        break;
                }
                
                return error;
            }
        }
        #endregion

        #region INotifyDataErrorInfo 实现
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
                return null;
                
            return _validationErrors[propertyName];
        }
        
        private void SetError(string propertyName, string error)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors[propertyName] = new List<string>();
                
            if (!_validationErrors[propertyName].Contains(error))
            {
                _validationErrors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        
        private void ClearErrors(string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName) && _validationErrors[propertyName].Any())
            {
                _validationErrors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion

        #region 属性
        public string Age
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

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
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

        public string Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    // 标记字段已被编辑
                    _editedFields[nameof(Username)] = true;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    // 标记字段已被编辑
                    _editedFields[nameof(PhoneNumber)] = true;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string CreditCardNumber
        {
            get => _creditCardNumber;
            set
            {
                if (_creditCardNumber != value)
                {
                    _creditCardNumber = value;
                    OnPropertyChanged(nameof(CreditCardNumber));
                    ValidateCreditCard();
                }
            }
        }

        public string ExpiryDate
        {
            get => _expiryDate;
            set
            {
                if (_expiryDate != value)
                {
                    _expiryDate = value;
                    OnPropertyChanged(nameof(ExpiryDate));
                    ValidateCreditCard();
                }
            }
        }

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode != value)
                {
                    _postalCode = value;
                    OnPropertyChanged(nameof(PostalCode));
                }
            }
        }
        #endregion

        #region 验证逻辑
        private void ValidateCreditCard()
        {
            // 清除之前的错误
            ClearErrors(nameof(CreditCardNumber));
            ClearErrors(nameof(ExpiryDate));
            
            // 信用卡号验证
            if (!string.IsNullOrEmpty(CreditCardNumber))
            {
                // 移除空格和破折号
                string cleanedNumber = CreditCardNumber.Replace(" ", "").Replace("-", "");
                
                // 基本验证: 只允许数字且长度在13-19之间
                if (!Regex.IsMatch(cleanedNumber, @"^\d{13,19}$"))
                {
                    SetError(nameof(CreditCardNumber), "信用卡号必须是13-19位数字");
                }
                else
                {
                    // Luhn算法验证信用卡号 (这是一个简化版本)
                    int sum = 0;
                    bool alternate = false;
                    
                    for (int i = cleanedNumber.Length - 1; i >= 0; i--)
                    {
                        int n = int.Parse(cleanedNumber[i].ToString());
                        
                        if (alternate)
                        {
                            n *= 2;
                            if (n > 9) n -= 9;
                        }
                        
                        sum += n;
                        alternate = !alternate;
                    }
                    
                    if (sum % 10 != 0)
                    {
                        SetError(nameof(CreditCardNumber), "无效的信用卡号");
                    }
                }
            }
            
            // 有效期验证
            if (!string.IsNullOrEmpty(ExpiryDate))
            {
                // 预期格式: MM/YY 或 MM/YYYY
                if (!Regex.IsMatch(ExpiryDate, @"^(0[1-9]|1[0-2])\/(\d{2}|\d{4})$"))
                {
                    SetError(nameof(ExpiryDate), "请输入有效的日期格式 (MM/YY 或 MM/YYYY)");
                }
                else
                {
                    // 检查是否过期
                    try
                    {
                        string[] parts = ExpiryDate.Split('/');
                        int month = int.Parse(parts[0]);
                        int year = int.Parse(parts[1]);
                        
                        // 如果是两位数年份，添加2000
                        if (year < 100) year += 2000;
                        
                        DateTime expiryDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1); // 月底
                        if (expiryDate < DateTime.Today)
                        {
                            SetError(nameof(ExpiryDate), "信用卡已过期");
                        }
                    }
                    catch
                    {
                        SetError(nameof(ExpiryDate), "无效的日期");
                    }
                }
            }
        }
        #endregion

        #region 界面事件处理
        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // 触发信用卡信息的验证
            ValidateCreditCard();
            
            // 显示验证结果
            if (!HasErrors)
            {
                MessageBox.Show("信用卡信息验证通过", "验证成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string errorMessage = "验证失败:\n";
                
                if (_validationErrors.ContainsKey(nameof(CreditCardNumber)))
                    errorMessage += string.Join("\n", _validationErrors[nameof(CreditCardNumber)]) + "\n";
                    
                if (_validationErrors.ContainsKey(nameof(ExpiryDate)))
                    errorMessage += string.Join("\n", _validationErrors[nameof(ExpiryDate)]);
                    
                MessageBox.Show(errorMessage, "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #region 基本验证
        private void TxtBasicValidation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // 进行验证
                bool isValid = !string.IsNullOrEmpty(textBox.Text) && textBox.Text.Length >= 3;
                
                // 根据验证结果更新界面
                textBox.Background = isValid 
                    ? System.Windows.Media.Brushes.White 
                    : System.Windows.Media.Brushes.MistyRose;
            }
        }
        #endregion

        #region 正则表达式验证
        private void TxtNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 使用正则表达式判断输入是否为数字
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtNumeric_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 阻止空格输入
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // 简单的邮箱格式验证
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                bool isValid = string.IsNullOrEmpty(textBox.Text) || Regex.IsMatch(textBox.Text, pattern);
                
                textBox.Background = isValid 
                    ? System.Windows.Media.Brushes.White 
                    : System.Windows.Media.Brushes.MistyRose;
            }
        }
        #endregion
        #endregion
    }

    #region 表单视图模型
    public class FormViewModel
    {
        public int Age { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
    #endregion

    #region 验证规则
    // 年龄验证规则
    public class AgeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int age;
            
            try
            {
                if (string.IsNullOrEmpty((value ?? "").ToString()))
                    return new ValidationResult(false, "年龄不能为空");
                    
                age = int.Parse((string)value, cultureInfo);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "请输入有效的数字");
            }
            
            if (age < Min || age > Max)
            {
                return new ValidationResult(false, $"年龄必须在 {Min} 到 {Max} 之间");
            }
            
            return ValidationResult.ValidResult;
        }
    }
    
    // 密码验证规则
    public class PasswordValidationRule : ValidationRule
    {
        public int MinLength { get; set; }
        public string ErrorMessage { get; set; }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = (value ?? "").ToString();
            
            if (string.IsNullOrEmpty(password))
                return new ValidationResult(false, "密码不能为空");
                
            if (password.Length < MinLength)
                return new ValidationResult(false, ErrorMessage ?? $"密码长度不能少于 {MinLength} 个字符");
                
            return ValidationResult.ValidResult;
        }
    }
    
    // 自定义验证规则 - 范围验证
    public class RangeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return new ValidationResult(false, "值不能为空");

                int intValue = int.Parse(value.ToString(), cultureInfo);
                if (intValue < Min || intValue > Max)
                    return new ValidationResult(false, ErrorMessage ?? $"值必须在 {Min} 到 {Max} 之间");

                return ValidationResult.ValidResult;
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, $"无效的输入: {ex.Message}");
            }
        }
    }

    // 自定义验证规则 - 电子邮件验证
    public class EmailValidationRule : ValidationRule
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "电子邮件地址不能为空");

            string email = value.ToString();
            if (!EmailRegex.IsMatch(email))
                return new ValidationResult(false, ErrorMessage ?? "请输入有效的电子邮件地址");

            return ValidationResult.ValidResult;
        }
    }

    // 必填字段验证规则
    public class RequiredFieldValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((value ?? "").ToString()))
                return new ValidationResult(false, ErrorMessage ?? "此字段为必填项");
                
            return ValidationResult.ValidResult;
        }
    }

    // 自定义验证规则 - 邮政编码验证
    public class PostalCodeValidationRule : ValidationRule
    {
        private static readonly Regex PostalCodeRegex = new Regex(@"^\d{6}$", RegexOptions.Compiled);
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "邮政编码不能为空");

            string postalCode = value.ToString();
            if (!PostalCodeRegex.IsMatch(postalCode))
                return new ValidationResult(false, ErrorMessage ?? "邮政编码必须是6位数字");

            return ValidationResult.ValidResult;
        }
    }
    #endregion
} 