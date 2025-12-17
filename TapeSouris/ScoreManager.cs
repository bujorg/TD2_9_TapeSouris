using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TapeSouris
{
    public static class ScoreManager
    {
        //CHEMIN VERS LE FICHIER QUI STOCKE TOUS LES SCORES
        private static readonly string filePath = "scores.json";

        //CHARGE LES SCORES À PARTIR DU FICHIER SCORES.JSON
        public static Dictionary<int, int> ChargerScores()
        {
            if (!File.Exists(filePath))
                return new Dictionary<int, int>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<int, int>>(json)
                   ?? new Dictionary<int, int>();
        }
        //PERMET DE SAUVEGARDER LES SCORES ET TESTE SI LE NOUVEAU SCORE EST SUPÉRIEUR
        public static void SauvegarderScore(int niveau, int score)
        {
            var scores = ChargerScores();

            if (!scores.ContainsKey(niveau) || score > scores[niveau])
            {
                scores[niveau] = score;
                string json = JsonSerializer.Serialize(
                    scores,
                    new JsonSerializerOptions { WriteIndented = true }
                );
                File.WriteAllText(filePath, json);
            }
        }

        //PERMET DE DONNER LE MEILLEUR SCORE POUR CHAQUE NIVEAU, QUI VA ENSUITE ÊTRE RÉCUPÉRÉ PAR SelectionNiveau
        public static int MeilleurScore(int niveau)
        {
            var scores = ChargerScores();
            return scores.ContainsKey(niveau) ? scores[niveau] : 0;
        }

        //PERMET DE RESETTER LES SCORES QUAND APPELÉ DE SELECTIONNIVEAU
        public static void ResetScores()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
