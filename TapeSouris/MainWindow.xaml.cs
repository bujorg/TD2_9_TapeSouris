using System.Windows;

namespace TapeSouris
{
    public partial class MainWindow : Window
    {
        //INITIALISE LE WPF
        public MainWindow()
        {
            InitializeComponent();
        }
        //QUAND LE BOUTON JEU EST CLIQUÉ, LANCE LA SÉLECTION DE NIVEAUX ET FERME LA FENÊTRE
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            SelectionNiveaux selectionNiveaux = new SelectionNiveaux();
            selectionNiveaux.Show();
            Close();
        }
    }
}
