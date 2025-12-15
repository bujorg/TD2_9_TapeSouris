using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TapeSouris
{
    public partial class Jeu : Window
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        Random apparais = new Random();

        private int score = 0;
        private int tempsRestant = 0;
        private int niveau;

        private DispatcherTimer minuterie;
        private bool jeuEnCours = true;
        private bool estEnPause = false;

        private MediaPlayer player;
        private MediaPlayer sonTouche;

        // 🔹 CONSTRUCTEUR MODIFIÉ
        public Jeu(int tempsNiveau, int niveauChoisi)
        {
            InitializeComponent();
            tempsRestant = tempsNiveau;
            niveau = niveauChoisi;
            Loaded += Jeu_Loaded;
        }

        private void Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            ChangerBackground();
            Demarrage();
            InitializeTimer();
            Musique();
            PreviewKeyDown += Jeu_PreviewKeyDown;
        }

        // 🔹 BACKGROUND SELON NIVEAU
        private void ChangerBackground()
        {
            if (niveau == 1)
            {
                imgBackground.Source = new BitmapImage(
                    new Uri("pack://application:,,,/images/sol.jpg"));
            }
            else if (niveau == 2)
            {
                imgBackground.Source = new BitmapImage(
                    new Uri("pack://application:,,,/images/sol_mc.jpg"));
            }
        }

        private void Musique()
        {
            player = new MediaPlayer();

            if (niveau == 1)
                player.Open(new Uri("Musiques/MusiqueNiveau1.mp3", UriKind.Relative));
            if (niveau == 2)
                player.Open(new Uri("Musiques/MusiqueNiveau2.mp3", UriKind.Relative));

            player.MediaEnded += (s, e) =>
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            };

            player.Volume = 0.5;
            player.Play();
        }

        private async void Demarrage()
        {
            var buttons = MainGrid.Children.OfType<Button>().ToList();

            foreach (var btn in buttons)
            {
                btn.Visibility = Visibility.Collapsed;
                btn.IsEnabled = false;
            }

            await Task.Delay(1000);

            while (jeuEnCours)
            {
                if (estEnPause)
                {
                    await Task.Delay(100);
                    continue;
                }

                cts = new CancellationTokenSource();
                var btn = buttons[apparais.Next(buttons.Count)];

                if (!btn.IsEnabled)
                {
                    btn.Visibility = Visibility.Visible;
                    btn.IsEnabled = true;

                    btn.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/images/souris.png")),
                        Stretch = Stretch.Uniform
                    };

                    try
                    {
                        await Task.Delay(apparais.Next(800, 2000), cts.Token);
                    }
                    catch { }

                    btn.IsEnabled = false;
                    btn.Content = null;
                    btn.Visibility = Visibility.Collapsed;
                }

                await Task.Delay(apparais.Next(300, 1000));
            }
        }

        private async void btnSouris_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;

            btn.Content = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/images/souris_etourdie.png")),
                Stretch = Stretch.Uniform
            };

            sonTouche = new MediaPlayer();
            sonTouche.Open(new Uri("Musiques/frappe.mp3", UriKind.Relative));
            sonTouche.Volume = 0.5;
            sonTouche.Play();

            await Task.Delay(200);
            btn.Content = null;

            cts.Cancel();

            score++;
            txtScore.Text = $"Score : {score}";
        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            minuterie.Tick += TempsJeu;
            minuterie.Start();
        }

        private void TempsJeu(object sender, EventArgs e)
        {
            tempsRestant--;
            txtTimer.Text = $"Temps : {tempsRestant}";

            if (tempsRestant <= 0)
            {
                minuterie.Stop();
                TerminerJeu();
            }
        }

        private void Jeu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !estEnPause)
                MettreEnPause();
        }

        private void PauseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MettreEnPause();
        }

        private void MettreEnPause()
        {
            minuterie.Stop();
            cts.Cancel();
            estEnPause = true;

            PauseMenu pause = new PauseMenu { Owner = this };
            bool? result = pause.ShowDialog();

            if (result == true)
                ReprendreJeu();
            else
                jeuEnCours = false;
        }

        private void ReprendreJeu()
        {
            estEnPause = false;
            minuterie.Start();
            jeuEnCours = true;
        }

        private void TerminerJeu()
        {
            jeuEnCours = false;
            cts.Cancel();

            foreach (var btn in MainGrid.Children.OfType<Button>())
                btn.IsEnabled = false;

            var result = MessageBox.Show(
                $"Temps écoulé ! Score : {score}\nRejouer ?",
                "Fin",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                score = 0;
                txtScore.Text = "Score : 0";
                tempsRestant = niveau == 1 ? 30 : 15;
                jeuEnCours = true;
                Demarrage();
                InitializeTimer();
            }
            else
            {
                Close();
            }
        }
    }
}
