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
            SelectionNiveaux choixNiveau = new SelectionNiveaux();
            bool? rep = choixNiveau.ShowDialog();

            if (rep == true)
            {
                Jeu jeuFenetre = new Jeu(
                    choixNiveau.TempsPourNiveau,
                    choixNiveau.NiveauChoisi
                );
                jeuFenetre.Show();
                this.Close();
            }
        }
    }
}
