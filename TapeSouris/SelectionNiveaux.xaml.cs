using System.Windows;

namespace TapeSouris
{
    public partial class SelectionNiveaux : Window
    {
        //LA PROPRIÉTÉ POUR DONNER UN TEMPS DE NIVEAU, TOUT LE MONDE PEUT LA LIRE (get) MAIS ELLE N'EST MODIFIABLE QUE ICI (private set)
        public int TempsPourNiveau { get; private set; }
        //PROPRIÉTÉ POUR SAVOIR QUEL NIVEAU A ÉTÉ CHOISI POUR LANCER LE BON DANS JEU
        public int NiveauChoisi { get; private set; }
        //SCORES REQUIS POUR DÉBLOQUER UN NIVEAU SUPÉRIEUR.
        private readonly int[] scoreRequis = { 0, 15, 8 };
        //LANCE TOUTES CES MÉTHODES AU LANCEMENT DE LA FENÊTRE
        public SelectionNiveaux()
        {
            InitializeComponent();
            ChargerScores();
            DeblocageNiveau();
        }
        //QUAND LE BOUTON DU NIVEAU 1/2/3 EST CLIQUÉ, RENVOIE LE TEMPS POUR LE NIVEAU ET LE NIVEAU
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
        //APPELLE LE PROGRAMME POUR SAUVEGARDER LE MEILLEUR SCORE ET BLOQUE LE BOUTON DANS LE CAS OÙ LE SCORE EST RESET
        private void ChargerScores()
        {
            scoreNiveau1.Text = $"Meilleur score : {ScoreManager.MeilleurScore(1)}";
            scoreNiveau2.Text = $"Meilleur score : {ScoreManager.MeilleurScore(2)}";
            scoreNiveau3.Text = $"Meilleur score : {ScoreManager.MeilleurScore(3)}";

            btnNiveau2.IsEnabled = ScoreManager.MeilleurScore(2) > 0;
            btnNiveau3.IsEnabled = ScoreManager.MeilleurScore(3) > 0;
        }
        //QUAND LE BOUTON POUR RESET LES SCORES EST CLIQUÉ, DEMANDE UNE CONFIRMATION, PUIS RESET, PUIS DÉSACTIVE LES BOUTONS DES NIVEAUX DÉBLOQUABLES
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
                ChargerScores();
            }
        }
        //SI LE SCORE EST ASSEZ HAUT, DÉBLOQUE LE NIVEAU SUIVANT
        private void DeblocageNiveau()
        {
            int scoreN1 = ScoreManager.MeilleurScore(1);
            int scoreN2 = ScoreManager.MeilleurScore(2);

            btnNiveau2.IsEnabled = scoreN1 >= scoreRequis[1];
            btnNiveau2.Opacity = btnNiveau2.IsEnabled ? 1 : 0.4;

            btnNiveau3.IsEnabled = scoreN2 >= scoreRequis[2];
            btnNiveau3.Opacity = btnNiveau3.IsEnabled ? 1 : 0.4;
        }

    }
}
