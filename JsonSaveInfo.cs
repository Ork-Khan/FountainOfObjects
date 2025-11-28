using FountainOfObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FountainOfObjects
{
    public class JsonInfo : ISaveInfo, IReadInfo
    {
        private string _path;
        public JsonInfo(string path)
        {
            _path = path;
        }
        public void SaveScore(Score score)
        {
            var scores = GetScores();
            if(scores != null)
                scores.Add(score);
            else
            {
                scores = new List<Score>();
                scores.Add(score);
            }

            string jsonText = JsonSerializer.Serialize(scores);
            File.Delete(_path);
            File.WriteAllText(_path, jsonText);
        }

        public List<Score>? GetScores()
        {
            if(File.Exists(_path))
            {
                string jsonScore = File.ReadAllText(_path);
                if(!string.IsNullOrWhiteSpace(jsonScore))
                {
                    var scores = JsonSerializer.Deserialize<List<Score>>(jsonScore);
                    return scores;
                }
            }
            return null;
        }

    }
}
