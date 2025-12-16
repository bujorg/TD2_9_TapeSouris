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
        private void btnNiveau3_Click(object sender, RoutedEventArgs e)
        {
            Jeu jeu = new Jeu(0, 3);
            jeu.Show();
            Close();
        }
        private void ChargerScores()
        {
            scoreNiveau1.Text = $"Meilleur score : {ScoreManager.MeilleurScore(1)}";
            scoreNiveau2.Text = $"Meilleur score : {ScoreManager.MeilleurScore(2)}";
            scoreNiveau3.Text = $"Meilleur score : {ScoreManager.MeilleurScore(3)}";
        }
        private void ResetScores_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Voulez-vous vraiment réinitialiser tous les scores ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ScoreManager.ResetScores();
                ChargerScores(); // 🔄 Rafraîchit l'affichage
            }
        }

    }
}
