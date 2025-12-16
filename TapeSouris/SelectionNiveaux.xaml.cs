using System.Windows;

namespace TapeSouris
{
    public partial class SelectionNiveaux : Window
    {
        public int TempsPourNiveau { get; private set; }
        public int NiveauChoisi { get; private set; }

        public SelectionNiveaux()
        {
            InitializeComponent();
            ChargerScores();
        }
        private void btnNiveau1_Click(object sender, RoutedEventArgs e)
        {
            Jeu jeu = new Jeu(30, 1);
            jeu.Show();
            Close();
        }

        private void btnNiveau2_Click(object sender, RoutedEventArgs e)
        {
            Jeu jeu = new Jeu(15, 2);
            jeu.Show();
            Close();
        }

        private void ChargerScores()
        {
            scoreNiveau1.Text = $"Meilleur score : {ScoreManager.MeilleurScore(1)}";
            scoreNiveau2.Text = $"Meilleur score : {ScoreManager.MeilleurScore(2)}";
        }
    }
}
