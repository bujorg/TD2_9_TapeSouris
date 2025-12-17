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
        private double volumeJeu = 1;
        private bool scoreSauvegarde = false;

        // FONCTION D'INITIALISATION. ELLE INITIALISE LA FENETRE, LE TEMPS ET LE NIVEAU
        public Jeu(int tempsNiveau, int niveauChoisi)
        {
            InitializeComponent();
            tempsRestant = tempsNiveau;
            niveau = niveauChoisi;
            Closing += Jeu_Closing;
            Loaded += Jeu_Loaded;
        }

        // DEBUT DU JEU. CETTE FONCTION MET LE BON BACKGROUND ET DEMARRE LE JEU
        private void Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            ChangerBackground();
            Demarrage();
            if (niveau != 3)
            {
                InitializeTimer();
            }
            else
            {
                txtTimer.Text = "Temps : ∞";
            }
            Musique();
            PreviewKeyDown += Jeu_PreviewKeyDown;
        }
        //SAUVEGARDE LE SCORE
        private void SauvegarderScoreUneFois()
   
        {
            if (!scoreSauvegarde)
            {
                ScoreManager.SauvegarderScore(niveau, score);
                scoreSauvegarde = true;
            }
        }
        // SAUVEGARDE LORSQU'ON FERME LE JEU
        private void Jeu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SauvegarderScoreUneFois();
        }

        //CHARGE LE BON BACKGROUND SELON NIVEAU
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
                new Uri("pack://application:,,,/images/sol_dehors.jpg"));
            }
            else if (niveau == 3)
            {
                imgBackground.Source = new BitmapImage(
                new Uri("pack://application:,,,/images/sol_mc.png"));
            }
        }
        //MET LA BONNE MUSIQUE SELON LE NIVEAU AU VOLUME SPECIFIE
        private void Musique()
        {
            player = new MediaPlayer();
            if (niveau == 1 || niveau == 2)
                player.Open(new Uri("Musiques/MusiqueNiveau1.mp3", UriKind.Relative));
            if (niveau == 3)
                player.Open(new Uri("Musiques/MusiqueNiveau2.mp3", UriKind.Relative));
            player.MediaEnded += (s, e) =>
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            };
            player.Volume = volumeJeu;
            player.Play();
        }

        //DEMARRE LE JEU EN LANCANT L'ACTIVATION DES BOUTONS ET GERE L'ACTIVATION DU MENU PAUSE ET DU TYPE DE SOURIS AFFICHE
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
                    if (niveau == 1 || niveau == 2)
                    {
                        btn.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/images/souris.png")),
                            Stretch = Stretch.Uniform
                        };
                    }
                    if (niveau == 3)
                    {
                        btn.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/images/souris_mc.png")),
                            Stretch = Stretch.Uniform
                        };
                    }
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
        //CETTE FONCTION VERIFIE SI NOUS AVONS PRESSE LE BOUTON ET CHANGE LA SOURIS EN SOURIS ETOURDI SELON LE NIVEAU EN JOUANT LE SON DE FRAPPE
        private async void btnSouris_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;
            if (niveau == 1 || niveau == 2)
            {
                btn.Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/images/souris_etourdie.png")),
                    Stretch = Stretch.Uniform
                };
            }
            if (niveau == 3)
            {
                btn.Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/images/souris_etourdie_mc.png")),
                    Stretch = Stretch.Uniform
                };
            }

            sonTouche = new MediaPlayer();
            if (niveau==1 || niveau==2)
                sonTouche.Open(new Uri("Musiques/frappe.mp3", UriKind.Relative));
            if (niveau==3)
                sonTouche.Open(new Uri("Musiques/frappe_mc.mp3", UriKind.Relative));
            sonTouche.Volume = volumeJeu;
            sonTouche.Play();

            await Task.Delay(200);
            btn.Content = null;

            cts.Cancel();

            score++;
            txtScore.Text = $"Score : {score}";
        }
        //LANCE LE TIMER
        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            minuterie.Tick += TempsJeu;
            minuterie.Start();
        }
        //GERE LE TEMPS RESTANT EN JEU ET ARRETE LORSQU'IL EST FINI
        private void TempsJeu(object sender, EventArgs e)
        {
            if (niveau == 3)
                return;

            tempsRestant--;
            txtTimer.Text = $"Temps : {tempsRestant}";

            if (tempsRestant <= 0)
            {
                minuterie?.Stop();
                TerminerJeu();
            }
        }
        // VERIFIE SI ECHAPE EST APPUYE ET LANCE LA FONCTION DE PAUSE SI OUI
        private void Jeu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !estEnPause)
                MettreEnPause();
        }
        //LANCE LA FONCTION DE PAUSE SI LE BOUTON PAUSE EST APPUYE
        private void PauseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MettreEnPause();
        }
        //MET EN PAUSE LE JEU (TIMER,APPARITION SOURIS)
        private void MettreEnPause()
        {
            minuterie?.Stop();
            cts.Cancel();
            estEnPause = true;
            PauseMenu pause = new PauseMenu
            {
                Owner = this,
                Volume = volumeJeu
            };
            bool? result = pause.ShowDialog();
            volumeJeu = pause.Volume;
            if (player != null)
                player.Volume = volumeJeu;

            if (result == true)
                ReprendreJeu();
            else
                jeuEnCours = false;
        }
        //RELANCE LE JEU
        private void ReprendreJeu()
        {
            estEnPause = false;
            minuterie?.Start();
            jeuEnCours = true;
        }
        //SE LANCE APRES QUE LE TEMPS S'ECOULE ET ENVOIE UN MESSAGE DE FIN PERMETTANT DE RECOMMENCER, D'ARRETER OU DE FERMER LE JEU
        private void TerminerJeu()
        {
            jeuEnCours = false;
            cts.Cancel();
            SauvegarderScoreUneFois();

            foreach (var btn in MainGrid.Children.OfType<Button>())
                btn.IsEnabled = false;

            var result = MessageBox.Show(
                $"Temps écoulé ! Score : {score}\n\n" +
                "Oui = Rejouer\n" +
                "Non = Retour au menu\n" +
                "Annuler = Quitter le jeu",
                "Fin de partie",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                score = 0;
                txtScore.Text = "Score : 0";
                tempsRestant = niveau == 1 ? 30 : 15;
                jeuEnCours = true;
                Demarrage();
                InitializeTimer();
            }
            else if (result == MessageBoxResult.No)
            {
                QuitterJeu(retournerMenu: true);
            }
            else
            {
                QuitterJeu(retournerMenu: false);
            }
        }
        //REVIENS AU MENU DE SELECTION DE NIVEAUX
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            QuitterJeu(retournerMenu: true);
        }
        //FERME LE JEU ET NOUS ENVOIE AU MENU DE SELECTION DE NIVEAUX SI DEMANDE
        private void QuitterJeu(bool retournerMenu)
        {
            minuterie?.Stop();
            cts.Cancel();
            player?.Stop();
            SauvegarderScoreUneFois();
            if (retournerMenu)
            {
                SelectionNiveaux menu = new SelectionNiveaux();
                menu.Show();
            }
            Close();
        }
    }
}
