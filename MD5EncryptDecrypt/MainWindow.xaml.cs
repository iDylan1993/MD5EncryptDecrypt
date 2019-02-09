using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MD5EncryptDecrypt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string hash;

        private void Randomletters()
        {
            hash = string.Empty;
            Char[] letters = "qwertyuiopasdfghjklzxcvbnm".ToCharArray();
            Random R = new Random();
            for (int i = 0; i < 18; i++)
            {
                hash += letters[R.Next(0, 25)].ToString();
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ValueBox.Text))
            {
                System.Windows.MessageBox.Show("Value must not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Randomletters();
                byte[] data = UTF8Encoding.UTF8.GetBytes(ValueBox.Text);
                using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = MD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider()
                    { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform Transform = TripleDes.CreateEncryptor();
                        byte[] Results = Transform.TransformFinalBlock(data, 0, data.Length);
                        EncryptBox.Text = Convert.ToBase64String(Results, 0, Results.Length);
                    }
                }
            } 
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(EncryptBox.Text))
            {
                System.Windows.MessageBox.Show("Encrypt must not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            { 
            byte[] data = Convert.FromBase64String(EncryptBox.Text);
                using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = MD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider()
                    { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform Transform = TripleDes.CreateDecryptor();
                        byte[] Results = Transform.TransformFinalBlock(data, 0, data.Length);
                        DecryptBox.Text = UTF8Encoding.UTF8.GetString(Results);
                    }
                }
            }
        }
    }
 }

