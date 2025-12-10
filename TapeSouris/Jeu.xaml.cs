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
using System.Windows.Shapes;

namespace TapeSouris
{
    /// <summary>
    /// Logique d'interaction pour Jeu.xaml
    /// </summary>
    public partial class Jeu : Window
    {
        public Jeu()
        {
            InitializeComponent();
        }
        private async void btnSouris1(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris2(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris3(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris4(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris5(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris6(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris7(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris8(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris9(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris10(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void btnSouris11(object sender, RoutedEventArgs e)
        {
            apparition();
        }
        private async void apparition()
        {
            Random apparition = new Random();
            int attenteApparition = apparition.Next(1000, 5000);
            Souris1.IsEnabled = false;
            await Task.Delay(attenteApparition);
            Souris1.IsEnabled = true;
        }
    }
}
