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

namespace pp11
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            var categories = База_данныхEntities1.GetContext().categ.ToList();
            categories.Insert(0, new categ { id = 0, category_name = "все категории" });
            CategotyFilterComboBox.ItemsSource = categories;
            CategotyFilterComboBox.SelectedIndex = 0;
            ProductsListView.ItemsSource = База_данныхEntities1.GetContext().prodact.ToList();
        }

        private void ApplyFilter()
        {
            var selectedCategory = CategotyFilterComboBox.SelectedItem as categ;
            var query = База_данныхEntities1.GetContext().prodact.AsQueryable();
            if(selectedCategory != null && selectedCategory.id != 0)
             {
             query = query.Where(p => p.id_cat == selectedCategory.id);
            }
            if(!string.IsNullOrWhiteSpace(SearchTextBox.Text)){
                query = query.Where(p => p.name_prod.StartsWith(SearchTextBox.Text));
            }
            ProductsListView.ItemsSource = query.ToList();
        }

        private void CategotyFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEditPage());
            ProductsListView.ItemsSource = База_данныхEntities1.GetContext().prodact.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItems.Cast<prodact>().FirstOrDefault();
            if(selectedProduct != null)
            {
                NavigationService.Navigate(new ProductEditPage(selectedProduct));
                ProductsListView.ItemsSource = База_данныхEntities1.GetContext().prodact.ToList();
            }
            else
            {
                MessageBox.Show("Выберите продукт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
