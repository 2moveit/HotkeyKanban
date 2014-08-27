using System.Collections.Generic;
using System.IO;
using KCT.HotkeyKanban.IO;

namespace HotkeyKanban.tests
{
    public class PersistencyMockup : IPersist
    {
        public void ToFile(object obj, FileInfo file)
        {
            
        }

        public T FromFile<T>(FileInfo file)
        {
            return default(T);
        }

        public IEnumerable<T> FromDirectory<T>(DirectoryInfo folder, string filter = "*.txt")
        {
            return new List<T>();
        }
    }
}