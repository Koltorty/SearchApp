using System;
using System.Windows;

using SearchApp.Constants;
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
            try
            {
                if (string.IsNullOrEmpty(CatalogPathTextBlock.Text))
                {
                    ErrorTextBox.Text = StringConstants.EmptyDirectoryPathError;
                    return;
                }

                if (string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    ErrorTextBox.Text = StringConstants.EmptySearchQueryError;
                    return;
                }

                var result = searchService.Search(
                    CatalogPathTextBlock.Text,
                    SearchTextBox.Text,
                    SizeTextBox.Text,
                    DatePicker.SelectedDate,
                    SubCatalogCheckbox.IsChecked);

                ResultsGrid.ItemsSource = result;
            }
            catch (Exception exception)
            {
                ErrorTextBox.Text = exception.InnerException != null
                    ? exception.InnerException.Message
                    : exception.Message;
            }
        }
    }
}
