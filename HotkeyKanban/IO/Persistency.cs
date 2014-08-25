using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace KCT.HotkeyKanban.IO
{
    public class Persistency : IPersist
    {
        public void ToFile(object obj, FileInfo file)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(file.FullName, json, Encoding.UTF8);
        }

        public T FromFile<T>(FileInfo file)
        {
            if (!file.Exists)
                return default(T);
            string json = File.ReadAllText(file.FullName, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public IEnumerable<T> FromDirectory<T>(DirectoryInfo folder, string filter = "*.txt")
        {
            FileInfo[] files = folder.GetFiles(filter);
            foreach (FileInfo file in files)
            {
                yield return FromFile<T>(file);
            }
        }
    }
}