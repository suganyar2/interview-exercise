using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Interview_Exercise
{
    public interface IStorageHandler
    {
        Dictionary<string, string> ReadAll();

        void Write(string key, string value);

        void Update(string key, string value);

        void Remove(string key);

        void RemoveAll();
    }

    public class CSVStorageHandler : IStorageHandler
    {
        string _storagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Interview-Exercise");

        public Dictionary<string, string> ReadAll()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(_storagePath).Where(file => file.EndsWith(".csv"));
            var dict = new Dictionary<string, string>();
            foreach (var file in files)
            {
                string[] allLines = File.ReadAllLines(file);
                foreach (string line in allLines)
                {
                    string[] keyValue = line.Split(',');
                    dict.Add(keyValue[0], keyValue[1]);
                }
            }
            return dict;
        }

        public void Write(string key, string value)
        {
            string filePath = $@"{_storagePath}\{key[0]}.csv";
            File.AppendAllText(filePath, $"{key},{value}\n");
        }

        public void Update(string key, string value)
        {
            string filePath = $@"{_storagePath}\{key[0]}.csv";
            var lines = File.ReadAllLines(filePath).Where(line => !line.StartsWith(key));
            File.WriteAllLines(filePath, lines);
            File.AppendAllText(filePath, $"{key},{value}\n");
        }

        public void Remove(string key)
        {
            string filePath = $@"{_storagePath}\{key[0]}.csv";
            var lines = File.ReadAllLines(filePath).Where(line => !line.StartsWith(key));
            File.WriteAllLines(filePath, lines);
        }

        public void RemoveAll()
        {
            Array.ForEach(Directory.GetFiles(_storagePath), delegate (string path) { File.Delete(path); });
        }
    }
}
