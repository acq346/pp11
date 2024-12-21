using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        
        public CategoryPage()
        {
            InitializeComponent();

            LoadDate();
        }


        public void LoadDate()
        {
            CategListView.ItemsSource = База_данныхEntities1.GetContext().categ.ToList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditCategoryPage(this));
            CategListView.ItemsSource = База_данныхEntities1.GetContext().categ.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedCateg = CategListView.SelectedItems.Cast<categ>().FirstOrDefault();
            if (selectedCateg != null)
            {
                NavigationService.Navigate(new EditCategoryPage(this));
                CategListView.ItemsSource = База_данныхEntities1.GetContext().categ.ToList();
            }
            else
            {
                MessageBox.Show("Выберите продукт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
