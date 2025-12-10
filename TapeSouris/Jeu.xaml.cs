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
        Random apparais = new Random();
        public Jeu()
        {
            InitializeComponent();
            Demarrage();
        }
        /*private async void apparition(Button clickedButton)
        {
            
            int attenteApparition = apparais.Next(1000, 5000);
            clickedButton.IsEnabled = false;
            await Task.Delay(attenteApparition);
            clickedButton.IsEnabled = true;
        }*/
       private async void btnSouris_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
        }
        /*private async void demarage()
        {
            foreach (var btn in MainGrid.Children.OfType<Button>())
            {
                btn.IsEnabled = false;      
            }
            await Task.Delay(1000);
            foreach (var btn in MainGrid.Children.OfType<Button>())
            {
                btn.IsEnabled = true;
                await Task.Delay(apparais.Next(800, 2000)); // Temps d'apparition
                btn.IsEnabled = false;
                await Task.Delay(apparais.Next(500, 1500)); // Pause
            }
        }*/
        private async void Demarrage()
        {
            var buttons = MainGrid.Children.OfType<Button>().ToList();

            // Désactiver tous les boutons
            foreach (var btn in buttons)
                btn.IsEnabled = false;

            await Task.Delay(1000);

            // Boucle infinie pour réactiver aléatoirement les boutons
            while (true)
            {
                // Choisir un bouton aléatoire
                var btn = buttons[apparais.Next(buttons.Count)];

                // Si le bouton est déjà actif, sauter
                if (!btn.IsEnabled)
                {
                    btn.IsEnabled = true;

                    // Le garder actif pendant un temps aléatoire
                    int activeTime = apparais.Next(800, 2000);
                    await Task.Delay(activeTime);

                    // Désactiver à nouveau
                    btn.IsEnabled = false;
                }

                // Pause aléatoire avant de réactiver un autre bouton
                await Task.Delay(apparais.Next(300, 1000));
            }
        }

    }
}
