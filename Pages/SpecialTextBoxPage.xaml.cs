using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TextBoxDemo.Pages
{
    public partial class SpecialTextBoxPage : Page
    {
        public SpecialTextBoxPage()
        {
            InitializeComponent();
            
            // 添加对控件的null检查
            if (pwdBox != null && passwordLength != null)
            {
                UpdatePasswordStrengthIndicator(EvaluatePasswordStrength(pwdBox.Password));
                passwordLength.Text = pwdBox.Password.Length.ToString();
            }
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            
            // 添加null检查
            if (passwordBox == null || passwordLength == null || strengthIndicator == null || strengthText == null)
            {
                return;
            }
            
            // 显示密码长度
            passwordLength.Text = $"{passwordBox.Password.Length}";
            
            // 评估密码强度
            PasswordStrength strength = EvaluatePasswordStrength(passwordBox.Password);
            UpdatePasswordStrengthIndicator(strength);
        }
        
        private enum PasswordStrength { Weak, Medium, Strong }
        
        private PasswordStrength EvaluatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return PasswordStrength.Weak;
                
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));
            
            if (password.Length >= 8 && hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar)
                return PasswordStrength.Strong;
                
            return PasswordStrength.Medium;
        }
        
        private void UpdatePasswordStrengthIndicator(PasswordStrength strength)
        {
            // 添加null检查
            if (strengthIndicator == null || strengthText == null)
            {
                return;
            }
            
            switch (strength)
            {
                case PasswordStrength.Weak:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Red);
                    strengthText.Text = "弱";
                    break;
                case PasswordStrength.Medium:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Yellow);
                    strengthText.Text = "中";
                    break;
                case PasswordStrength.Strong:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Green);
                    strengthText.Text = "强";
                    break;
            }
        }
        
        private void MaskChar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pwdBox != null && cmbMaskChar.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbMaskChar.SelectedItem;
                if (selectedItem.Content is string content && !string.IsNullOrEmpty(content))
                {
                    pwdBox.PasswordChar = content[0];
                }
            }
        }
        
        // RichTextBox方法
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, 
                richTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold) ? 
                FontWeights.Normal : FontWeights.Bold);
        }
        
        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, 
                richTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic) ? 
                FontStyles.Normal : FontStyles.Italic);
        }
        
        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            TextRange selection = new TextRange(richTextBox.Selection.Start, richTextBox.Selection.End);
            
            // 获取当前下划线状态
            TextDecorationCollection textDecorations = selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection;
            bool hasUnderline = textDecorations != null && textDecorations.Count > 0;
            
            // 设置或移除下划线
            if (hasUnderline)
            {
                selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            else
            {
                selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }
        
        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (richTextBox != null && richTextBox.Selection != null && cmbFontFamily.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbFontFamily.SelectedItem;
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(selectedItem.Content.ToString()));
            }
        }
        
        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (richTextBox != null && richTextBox.Selection != null && cmbFontSize.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbFontSize.SelectedItem;
                if (double.TryParse(selectedItem.Content.ToString(), out double size))
                {
                    richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
                }
            }
        }
        
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
        }
        
        private void Green_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
        }
        
        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
        }
        
        private void Black_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null || richTextBox.Selection == null)
                return;
                
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
        }
    }
} 