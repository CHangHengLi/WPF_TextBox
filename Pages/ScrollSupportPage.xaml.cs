using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TextBoxDemo.Pages
{
    /// <summary>
    /// ScrollSupportPage.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollSupportPage : Page
    {
        private int _lineCounter = 0;
        
        public ScrollSupportPage()
        {
            InitializeComponent();
            
            // 初始化滚动控制示例文本框
            InitializeScrollControlTextBox();
        }
        
        private void InitializeScrollControlTextBox()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 20; i++)
            {
                _lineCounter++;
                sb.AppendLine($"这是第 {_lineCounter} 行文本内容");
            }
            txtScrollControl.Text = sb.ToString();
        }

        #region 垂直滚动条设置
        private void VerticalScrollBar_Changed(object sender, RoutedEventArgs e)
        {
            if (txtVerticalScroll == null) return;
            
            if (rbVerticalAuto.IsChecked == true)
                txtVerticalScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            else if (rbVerticalVisible.IsChecked == true)
                txtVerticalScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            else if (rbVerticalHidden.IsChecked == true)
                txtVerticalScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            else if (rbVerticalDisabled.IsChecked == true)
                txtVerticalScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }
        #endregion

        #region 水平滚动条设置
        private void HorizontalScrollBar_Changed(object sender, RoutedEventArgs e)
        {
            if (txtHorizontalScroll == null) return;
            
            if (rbHorizontalAuto.IsChecked == true)
                txtHorizontalScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            else if (rbHorizontalVisible.IsChecked == true)
                txtHorizontalScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            else if (rbHorizontalHidden.IsChecked == true)
                txtHorizontalScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            else if (rbHorizontalDisabled.IsChecked == true)
                txtHorizontalScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }
        #endregion

        #region 文本换行设置
        private void TextWrapping_Changed(object sender, RoutedEventArgs e)
        {
            if (txtWrapping == null) return;
            
            if (rbWrapWrap.IsChecked == true)
                txtWrapping.TextWrapping = TextWrapping.Wrap;
            else if (rbWrapNoWrap.IsChecked == true)
                txtWrapping.TextWrapping = TextWrapping.NoWrap;
            else if (rbWrapWrapWithOverflow.IsChecked == true)
                txtWrapping.TextWrapping = TextWrapping.WrapWithOverflow;
        }
        #endregion

        #region 编程控制滚动
        private void AddLines_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder(txtScrollControl.Text);
            sb.AppendLine();
            
            // 添加10行新文本
            for (int i = 1; i <= 10; i++)
            {
                _lineCounter++;
                sb.AppendLine($"这是第 {_lineCounter} 行文本内容");
            }
            
            txtScrollControl.Text = sb.ToString();
            txtScrollControl.ScrollToEnd();
        }

        private void ScrollToHome_Click(object sender, RoutedEventArgs e)
        {
            txtScrollControl.ScrollToHome();
        }

        private void ScrollToEnd_Click(object sender, RoutedEventArgs e)
        {
            txtScrollControl.ScrollToEnd();
        }

        private void ScrollToLine5_Click(object sender, RoutedEventArgs e)
        {
            // 获取文本行
            string[] lines = txtScrollControl.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            
            // 确保至少有5行
            if (lines.Length >= 5)
            {
                // 计算到第5行的字符偏移量
                int offset = 0;
                for (int i = 0; i < 4; i++) // 计算前4行的长度(到第5行的开始)
                {
                    offset += lines[i].Length + Environment.NewLine.Length;
                }
                
                // 设置插入点位置并滚动到那里
                txtScrollControl.Focus();
                txtScrollControl.Select(offset, 0);
                txtScrollControl.ScrollToLine(4); // 索引从0开始，所以第5行是索引4
            }
        }
        #endregion
    }
} 