using System;
using System.Windows;
using System.Windows.Controls;

namespace TextBoxDemo.Pages
{
    /// <summary>
    /// TextSelectionPage.xaml 的交互逻辑
    /// </summary>
    public partial class TextSelectionPage : Page
    {
        public TextSelectionPage()
        {
            InitializeComponent();
        }

        #region 选择操作
        private void TxtSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && txtSelectionStart != null && txtSelectionLength != null)
            {
                txtSelectionStart.Text = textBox.SelectionStart.ToString();
                txtSelectionLength.Text = textBox.SelectionLength.ToString();
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            txtSelection.SelectAll();
            txtSelection.Focus();
        }

        private void SelectFirst5_Click(object sender, RoutedEventArgs e)
        {
            if (txtSelection.Text.Length >= 5)
            {
                txtSelection.Select(0, 5);
                txtSelection.Focus();
            }
        }

        private void SelectLast5_Click(object sender, RoutedEventArgs e)
        {
            if (txtSelection.Text.Length >= 5)
            {
                int start = txtSelection.Text.Length - 5;
                txtSelection.Select(start, 5);
                txtSelection.Focus();
            }
        }
        #endregion

        #region 选中文本操作
        private void TxtOriginal_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && txtSelectedText != null)
            {
                txtSelectedText.Text = textBox.SelectedText;
            }
        }

        private void ReplaceSelected_Click(object sender, RoutedEventArgs e)
        {
            if (txtOriginal.SelectionLength > 0)
            {
                txtOriginal.SelectedText = "[替换的文本]";
                txtOriginal.Focus();
            }
            else
            {
                MessageBox.Show("请先在文本框中选择一些文本", "提示");
            }
        }
        #endregion

        #region 光标位置操作
        private void TxtCaret_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && txtCaretIndex != null)
            {
                txtCaretIndex.Text = textBox.CaretIndex.ToString();
            }
        }

        private void MoveToStart_Click(object sender, RoutedEventArgs e)
        {
            txtCaret.CaretIndex = 0;
            txtCaret.Focus();
        }

        private void MoveToEnd_Click(object sender, RoutedEventArgs e)
        {
            txtCaret.CaretIndex = txtCaret.Text.Length;
            txtCaret.Focus();
        }

        private void MoveTo10_Click(object sender, RoutedEventArgs e)
        {
            if (txtCaret.Text.Length >= 10)
            {
                txtCaret.CaretIndex = 10;
                txtCaret.Focus();
            }
        }
        #endregion

        #region 文本统计
        private void TxtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTextStats();
        }

        private void UpdateTextStats()
        {
            if (txtContent == null || txtCharCount == null || txtLineCount == null || txtSelectedStats == null)
                return;
                
            // 计算文本统计信息
            int charCount = txtContent.Text.Length;
            string[] lines = txtContent.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int lineCount = lines.Length;
            
            // 计算选中部分的统计信息
            int selectedCharCount = txtContent.SelectedText.Length;
            
            // 更新统计信息显示
            txtCharCount.Text = $"字符数: {charCount}";
            txtLineCount.Text = $"行数: {lineCount}";
            txtSelectedStats.Text = $"选中: {selectedCharCount}个字符";
        }
        #endregion
    }
} 