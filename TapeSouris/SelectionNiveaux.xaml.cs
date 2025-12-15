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
        }

        private void btnNiveau1_Click(object sender, RoutedEventArgs e)
        {
            TempsPourNiveau = 30;
            NiveauChoisi = 1;
            DialogResult = true;
            Close();
        }

        private void btnNiveau2_Click(object sender, RoutedEventArgs e)
        {
            TempsPourNiveau = 15;
            NiveauChoisi = 2;
            DialogResult = true;
            Close();
        }
    }
}
