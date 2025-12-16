using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TapeSouris
{
    public partial class PauseMenu : Window
    {
        // 🔊 Volume transmis au jeu
        public double Volume { get; set; } = 0.5;

        public PauseMenu()
        {
            InitializeComponent();
        }

        private void PauseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            VolumeSlider.Value = Volume;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = VolumeSlider.Value;
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = true;
                Close();
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
