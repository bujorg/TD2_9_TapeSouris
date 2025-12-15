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
    /// Logique d'interaction pour SelectionNiveaux.xaml
    /// </summary>
    public partial class SelectionNiveaux : Window
    {
        public int TempsPourNiveau { get; private set; } = 30; // valeur par dé
        public SelectionNiveaux()
        {
            InitializeComponent();
        }
        private void btnNiveau1_Click(object sender, RoutedEventArgs e)
        {
            TempsPourNiveau = 30; // Niveau facile : 30 secondes
            DialogResult = true;
            this.Close();
        }

        private void btnNiveau2_Click(object sender, RoutedEventArgs e)
        {
            TempsPourNiveau = 15; // Niveau difficile : 15 secondes
            DialogResult = true;
            this.Close();
        }
    }
}
