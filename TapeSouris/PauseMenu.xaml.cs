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
    /// Logique d'interaction pour PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : Window
    {
        public PauseMenu()
        {
            InitializeComponent();
        }
        private void PauseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }
        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            // Ferme le menu pause
            this.DialogResult = true;
            this.Close();
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                // ferme le menu pause avec touche échape
                this.DialogResult = true;
                this.Close();
            }
        }
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            // Ferme complètement le jeu
            Application.Current.Shutdown();
        }
    }
}
