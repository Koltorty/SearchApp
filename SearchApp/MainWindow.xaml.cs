using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SearchApp.Models;
using SearchApp.Services;

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SearchService searchService;

        public MainWindow()
        {
            InitializeComponent();
            searchService = SearchService.GetSearchService();
        }

        private void CatalogPathButton_Click(object sender, RoutedEventArgs e)
        {
            var catalogPath = searchService.GetCatalogPath();
            CatalogPathTextBlock.Text = catalogPath;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var result = searchService.Search(
                CatalogPathTextBlock.Text,
                SearchTextBox.Text,
                SizeTextBox.Text,
                DatePicker.DisplayDate,
                SubCatalogCheckbox.IsChecked);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
