using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TapeSouris
{
    /// <summary>
    /// Logique d'interaction pour Jeu.xaml
    /// </summary>
    public partial class Jeu : Window
    {       
        //Crée cts qui pourra déclencher une annulation de await
        private CancellationTokenSource cts = new CancellationTokenSource();
        //Crée apparais qui nous permet d'avoir des timings et boutons aléatoires
        Random apparais = new Random();
        //Calcul su caore
        private int score = 0;
        //Temps du niveau 
        private int tempsRestant = 0;
        //Créer un timer du nom de minuterie
        private DispatcherTimer minuterie;
        private bool jeuEnCours = true;
        private bool estEnPause = false;
        private MediaPlayer player;
        private MediaPlayer sonTouche;

        //Permet de lancer les methodes lorsque la fenetre est ouverte
        public Jeu(int tempsNiveau)
        {
            InitializeComponent();
            tempsRestant = tempsNiveau;
            Loaded += Jeu_Loaded;
        }

        private void Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            Demarrage();
            InitializeTimer();
            Musique();
            this.PreviewKeyDown += Jeu_PreviewKeyDown;
        }
        private void Musique()
        {
            player = new MediaPlayer();
            player.Open(new Uri("Musiques/MusiqueNiveau1.mp3", UriKind.Relative));
            player.MediaEnded += (s, e) => { player.Position = TimeSpan.Zero; player.Play(); };
            player.Volume = 0.5;
            player.Play();  
        }

        //A pour but de démarer avec tout les boutons inactifs puis d'en activer un aléatoirement
        private async void Demarrage()
        {
            //stoque tout les Buttons qui se trouvent dans la grid "MainGrid" dans button sous forme de liste (dans le cas ou on rajoute des boutons au lieu de les taper 1 par 1 a la mains)
            var buttons = MainGrid.Children.OfType<Button>().ToList();
            //Pour chaque bouton
            foreach (var btn in buttons)
            {
                btn.Visibility = Visibility.Collapsed;
                //Désactive les boutons
                btn.IsEnabled = false;
            }

            //Fait une pause

            await Task.Delay(1000);
            //Boucle d'apparition des boutons

            while (jeuEnCours == true)
            {
                if (estEnPause)
                {
                    await Task.Delay(100); // Attend que la pause soit levée
                    continue;
                }
                //Création du jeton a chaque boucle
                cts = new CancellationTokenSource();
                //Chosi un bouton aléatoire
                var btn = buttons[apparais.Next(buttons.Count)];
                //Verifie si le bouton est pas déjà actif (si le niveau en fait apparaitre plusieurs a la fois)
                if (!btn.IsEnabled)
                {
                    btn.Visibility = Visibility.Visible;
                    //Active le bouton
                    btn.IsEnabled = true;
                    var sourisImage = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/images/souris.png")),
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };  
                    btn.Content = sourisImage;
                    //Tant que le bouton n'est pas cliqué fait le temps d'attente
                    try
                    {
                        //Fait le await sauf si le jeton cts demande une annulation (quand cts.Cancel(); s'active (quand le bouton est cliqué))
                        await Task.Delay(apparais.Next(800, 2000), cts.Token);
                    }
                    //Permet d'annuler l'annulation et de reprendre la boucle si non elle se relance plus et le je est figé
                    catch (TaskCanceledException)
                    {
                        
                    }
                    //Désactive le bouton au bout d'un certain temps si il a pas déjà été cliqué
                    btn.IsEnabled = false;
                    btn.Content = null;         
                    btn.Visibility = Visibility.Collapsed;
                }
                //Attente avant l'apparition du prochain bouton
                await Task.Delay(apparais.Next(300, 1000));
            }
        }


        //Pour detecter un bouton cliqué et faire des modification en conséquence
        private async void btnSouris_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            //Désactive le bouton cliqué
            btn.IsEnabled = false;            
            var toucheImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/images/souris_etourdie.png")),
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            // Mettre l'image sur le bouton
            btn.Content = toucheImage;
            sonTouche = new MediaPlayer();
            sonTouche.Open(new Uri("Musiques/frappe.mp3", UriKind.Relative));
            sonTouche.Volume = 0.5;
            sonTouche.Play();
            await Task.Delay(200);
            btn.Content = null;

            //Permet d'arreter le temps d'arêt (await) du bouton lorsqu'il est cencé entre actif
            cts.Cancel();
            
            
            
            //Permet d'augmenter le score de 1 et de le mettre a jours sur le ¨PF
            score++;
            txtScore.Text = $"Score : {score}";
        }
        private void InitializeTimer()
        {
            //Initialise le timer
            minuterie = new DispatcherTimer();
            //Configure l'intervalle du Timer
            minuterie.Interval = TimeSpan.FromMilliseconds(1000);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += temspJeu;
            // lancement du timer
            minuterie.Start();
        }
        private void temspJeu(object sender, EventArgs e)
        {
            //Reduit le temps restant sur le timer
            tempsRestant--;
            //Actualiser la timer visuel
            txtTimer.Text = $"Temps : {tempsRestant}";
            //Quant le timer descend a 0, lancer le stop du jeu
            if (tempsRestant <= 0)
            {
                minuterie.Stop();
                TerminerJeu();
            }
        }
        private void Jeu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !estEnPause)
            {
                MettreEnPause();
            }
        }
        private void PauseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MettreEnPause();
        }
        private void MettreEnPause()
        {
            // Stop le jeu
            minuterie.Stop();
            cts.Cancel();
            estEnPause = true   ;


            // Ouvre le menu pause
            PauseMenu pause = new PauseMenu();
            pause.Owner = this;

            bool? result = pause.ShowDialog();

            // ➜ Reprendre
            if (result == true)
            {
                ReprendreJeu();

            }
            else
            {
                // Quitter → la fenêtre Jeu sera fermée par PauseMenu
                jeuEnCours = false;
            }
        }
        private void ReprendreJeu()
        {
            estEnPause = false;

            // Redémarre le timer
            minuterie.Start();

            // Relance l’apparition des boutons
            jeuEnCours = true;
        }
        private void TerminerJeu()
        {
            jeuEnCours = false;
            cts.Cancel();
            // Désactiver tous les boutons
            foreach (var btn in MainGrid.Children.OfType<Button>())
                btn.IsEnabled = false;

            // Afficher un message avec choix
            var result = MessageBox.Show(
                $"Temps écoulé ! Votre score : {score}\n\nVoulez-vous rejouer ?",
                "Fin du jeu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                score = 0;
                txtScore.Text = $"Score : {score}";
                tempsRestant = 30;
                jeuEnCours = true;
                Demarrage();
                InitializeTimer();
            }
            else
            {
                this.Close();
            }
        }
    }
}
