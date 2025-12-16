using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TapeSouris
{
    public static class ScoreManager
    {
        private static readonly string filePath = "scores.json";

        public static Dictionary<int, int> ChargerScores()
        {
            if (!File.Exists(filePath))
                return new Dictionary<int, int>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<int, int>>(json)
                   ?? new Dictionary<int, int>();
        }

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

        public static int MeilleurScore(int niveau)
        {
            var scores = ChargerScores();
            return scores.ContainsKey(niveau) ? scores[niveau] : 0;
        }

        // 🔥 NOUVEAU
        public static void ResetScores()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
