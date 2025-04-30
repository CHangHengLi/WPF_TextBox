using System.Windows;
using System.Windows.Controls;
using TextBoxDemo.Pages;

namespace TextBoxDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 默认显示基本属性页面
            MainContent.Navigate(new BasicPropertiesPage());
        }

        private void BasicProperties_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new BasicPropertiesPage());
        }

        private void InputControl_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new InputControlPage());
        }

        private void TextSelection_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new TextSelectionPage());
        }

        private void ScrollSupport_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new ScrollSupportPage());
        }

        private void DataValidation_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new DataValidationPage());
        }

        private void UndoRedo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new UndoRedoPage());
        }

        private void DataBinding_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new DataBindingPage());
        }

        private void StylesTemplates_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new StylesTemplatesPage());
        }

        private void Applications_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new ApplicationsPage());
        }
        
        private void SearchDemo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new SearchDemoPage());
        }
        
        private void SpecialTextBox_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new SpecialTextBoxPage());
        }
    }
} 