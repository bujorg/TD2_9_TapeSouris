using System;
using System.Collections.Generic;
using System.Linq;
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
        private int tempsRestant = 30;
        //Créer un timer du nom de minuterie
        private DispatcherTimer minuterie;


        //Permet de lancer les methodes lorsque la fenetre est ouverte
        public Jeu()
        {
            InitializeComponent();
            Demarrage();
            InitializeTimer();
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

            while (true)
            {
                //Création du jeton a chaque boucle
                cts = new CancellationTokenSource();
                //Chosi un bouton aléatoire
                var btn = buttons[apparais.Next(buttons.Count)];
                //Verifie si le bouton est pas déjà actif (si le niveau en fait apparaitre plusieurs a la fois)
                if (!btn.IsEnabled)
                {
                    btn.IsEnabled = true;
                    //Active le bouton
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
                }
                //Attente avant l'apparition du prochain bouton
                await Task.Delay(apparais.Next(300, 1000));
            }
        }


        //Pour detecter un bouton cliqué et faire des modification en conséquence
        private void btnSouris_Click(object sender, RoutedEventArgs e)
        {
            //Permet d'arreter le temps d'arêt (await) du bouton lorsqu'il est cencé entre actif
            cts.Cancel();
            //Désactive le bouton cliqué
            ((Button)sender).IsEnabled = false;
            //Permet d'augmenter le score de 1 et de le mettre a jours sur le ¨PF
            score++;
            txtScore.Text = $"Score : {score}";
        }

                    // Le garder actif pendant un temps aléatoire
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/images/souris.png")),
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    btn.Content = sourisImage;

                    // Le garder actif pendant un temps aléatoire
                    int activeTime = apparais.Next(800, 2000);
                    await Task.Delay(activeTime);

                    // Désactiver à nouveau
        //Permet de faire défiler le temps dans le jeu
        private void InitializeTimer()
        {
            //Initialise le timer
            minuterie = new DispatcherTimer();
            //Configure l'intervalle du Timer
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += temspJeu;
            // lancement du timer
            minuterie.Start();
        }
        private void temspJeu(object sender, EventArgs e)
        {
            tempsRestant--;
            txtTimer.Text = $"Temps : {tempsRestant}";
            if (tempsRestant <= 0)
            {
                minuterie.Stop();
                TerminerJeu();
            }
        }
        private void TerminerJeu()
        {
            // Désactiver tous les boutons
            foreach (var btn in MainGrid.Children.OfType<Button>())
                btn.IsEnabled = false;

            MessageBox.Show($"Temps écoulé ! Votre score : {score}");

            // Si tu veux, tu peux fermer la fenêtre ou proposer de rejouer
            this.Close();
        }
    }
}
