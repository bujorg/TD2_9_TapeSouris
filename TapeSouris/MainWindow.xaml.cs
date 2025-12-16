using System.Windows;

namespace TapeSouris
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Ouvre la sélection des niveaux
            SelectionNiveaux selectionNiveaux = new SelectionNiveaux();
            selectionNiveaux.Show();

            // Ferme le menu principal
            Close();
        }
    }
}
