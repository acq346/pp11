using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ProductEditPage.xaml
    /// </summary>
    public partial class ProductEditPage : Page
    {
        private База_данныхEntities1 _context;
        private prodact _product;
        public ProductEditPage(prodact product = null)
        {
            InitializeComponent();
            var categories = База_данныхEntities1.GetContext().categ.ToList();
            categories.Insert(0, new categ
            {
                id = 0,
                category_name = "Все категории"
            });
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0;
            _context = База_данныхEntities1.GetContext();
            _product = product != null ? product : new prodact();
            if (product != null)
            {
                ProductNameTextBox.Text = product.name_prod;
                CategoryComboBox.SelectedValue = product.id_cat;
            }

        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || CategoryComboBox.SelectedValue == null)
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }

            _product.name_prod = ProductNameTextBox.Text.Trim();
                _product.id_cat = (int)CategoryComboBox.SelectedValue;

                int max = 0;
                if(_product.id == 0)
                {
                    if (_context.prodact.Count() != 0)
                        max = _context.prodact.Max(pr => pr.id);
                    _product.id = max + 1;
                    _context.prodact.Add(_product);
                }
                _context.SaveChanges();
                MessageBox.Show("Добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            

        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}








