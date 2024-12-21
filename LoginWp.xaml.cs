using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pp11
{
    /// <summary>
    /// Логика взаимодействия для LoginWp.xaml
    /// </summary>
    public partial class LoginWp : Window
    {
        private string _currentCaptcha;
        private int _failedAttempts = 0;
        private DateTime _blockEndTime;



        public LoginWp()
        {
            InitializeComponent();
            CheckBlock();
        }
        private void GenerateCaptcha()
        {
            _currentCaptcha = GenerateRandomCaptcha(10); // Генерация капчи при запуске и обновлении
            CaptchaValueTextBlock.Text = _currentCaptcha; // Отображение капчи
            CaptchaGrid.Visibility = Visibility.Visible;
            CaptchaTextBox.Visibility = Visibility.Visible;
            RefreshCaptchaButton.Visibility = Visibility.Visible;
        }
        private void CheckBlock()
        {
            // Проверяем, не заблокирован ли пользователь
            if (_blockEndTime > DateTime.Now)
            {
                var remainingTime = _blockEndTime - DateTime.Now;
                ErrorTextBlock.Text = $"Вы заблокированы. Осталось {remainingTime.TotalSeconds:F0} секунд";
                LoginTextBox.IsEnabled = false;
                PasswordBox.IsEnabled = false;
                CaptchaTextBox.IsEnabled = false;
                RefreshCaptchaButton.IsEnabled = false;
                return;
            }
            else
            {
                ErrorTextBlock.Text = "";
                LoginTextBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                CaptchaTextBox.IsEnabled = true;
                RefreshCaptchaButton.IsEnabled = true;

            }

        }

        private string GenerateRandomCaptcha(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder();
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                crypto.GetBytes(data);
                foreach (byte b in data)
                {
                    sb.Append(chars[b % chars.Length]);
                }
            }
            return sb.ToString();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";
            AttemptsTextBlock.Text = "";
            CheckBlock();
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string enteredCaptcha = CaptchaTextBox.Text;
            string hashedPassword = HashPassword(password);
            _failedAttempts = 0;

            var user = База_данныхEntities1.GetContext().users.FirstOrDefault(u => u.login == login);
            if (LoginTextBox.IsEnabled == false)
                return;



            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Заполните все поля";
                return;
            }
            else if (user == null)
            {
                ErrorTextBlock.Text = "Неверный логин или пароль";
                _failedAttempts++;
                AttemptsTextBlock.Text = $"Неверный ввод, попыток до блокировки: {3 - _failedAttempts}";
                if (_failedAttempts == 1)
                    GenerateCaptcha();
                if (_failedAttempts >= 3)
                    StartBlock();

                return;
            }
            else if (user.password != hashedPassword)
            {
                ErrorTextBlock.Text = "Неверный логин или пароль";
                _failedAttempts++;
                AttemptsTextBlock.Text = $"Неверный ввод, попыток до блокировки: {3 - _failedAttempts}";
                if (_failedAttempts == 1)
                    GenerateCaptcha();
                if (_failedAttempts >= 3)
                    StartBlock();

                return;
            }
            else if (_failedAttempts > 0 && enteredCaptcha != _currentCaptcha)
            {
                ErrorTextBlock.Text = "Неверная капча";
                _failedAttempts++;
                AttemptsTextBlock.Text = $"Неверный ввод, попыток до блокировки: {3 - _failedAttempts}";
                if (_failedAttempts >= 3)
                    StartBlock();
                return;
            }
            else
            {
                MessageBox.Show("Авторизация прошла успешно!", "Успешно");
                Window loginWp = Window.GetWindow(this);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                loginWp.Close();

            }



        }
        private void RefreshCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
        private void StartBlock()
        {
            _blockEndTime = DateTime.Now.AddSeconds(30); // Блокировка на 30 секунд
            ErrorTextBlock.Text = "Вы заблокированы на 30 секунд";
            LoginTextBox.IsEnabled = false;
            PasswordBox.IsEnabled = false;
            CaptchaTextBox.IsEnabled = false;
            RefreshCaptchaButton.IsEnabled = false;
            _failedAttempts = 0;
            new Thread(() =>
            {
                Thread.Sleep(30000);
                Dispatcher.Invoke(() =>
                {
                    CheckBlock();
                    _failedAttempts = 0;
                });

            }).Start();
        }
    }
}
