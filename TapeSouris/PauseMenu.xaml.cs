using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TapeSouris
{
    public partial class PauseMenu : Window
    {
        //Volume transmis au jeu
        public double Volume { get; set; } = 0.5;

        //INITIALISE LA FENETRE PAUSE
        public PauseMenu()
        {
            InitializeComponent();
        }
        //ACTIONS A FAIRE LORSQUE LA FENETRE EST CHARGEE 
        private void PauseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            VolumeSlider.Value = Volume;
        }
        //MET LE BON VOLUME GRACE AU SLIDER
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = VolumeSlider.Value;
        }
        //FERME LE MENU PAUSE SI LE BOUTON RESUME EST PRESSE ET REPREND LE JEU 
        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        //FERME LE MENU PAUSE SI ECHAPE EST PRESSE ET REPREND LE JEU
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = true;
                Close();
            }
        }
        //FERME LE JEU ENTIEREMENT SI LE BOUTON QUITTER EST APPUYE
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
