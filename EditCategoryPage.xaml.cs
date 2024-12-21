using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для EditCategoryPage.xaml
    /// </summary>
    public partial class EditCategoryPage : Page
    {
        private База_данныхEntities1 _context;
        private categ _categ;
        private CategoryPage _categoryPage;
      
        public EditCategoryPage(CategoryPage categoryPage, categ categs = null)
        {
            InitializeComponent();
            _context = База_данныхEntities1.GetContext();
            _categ = categs != null ? categs : new categ();
            _categoryPage = categoryPage;
            if (categs != null)
            {
                ProductNameTextBox.Text = categs.category_name;
            } 
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }
            using(var context = new База_данныхEntities1())
            {
                _categ.category_name = ProductNameTextBox.Text.Trim();

                int max = 0;
                if (_categ.id == 0)
                {
                    if (_context.prodact.Count() != 0)
                        max = _context.categ.Max(pr => pr.id);
                    _categ.id = max + 1;
                    _context.categ.Add(_categ);
                }
                _context.SaveChanges();
            }

            _categoryPage.LoadDate();
            MessageBox.Show("Добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
