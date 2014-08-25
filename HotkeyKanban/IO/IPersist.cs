using System.Collections.Generic;
using System.IO;

namespace KCT.HotkeyKanban.IO
{
    public interface IPersist
    {
        void ToFile(object obj, FileInfo file);
        T FromFile<T>(FileInfo file);
        IEnumerable<T> FromDirectory<T>(DirectoryInfo folder, string filter = "*.txt");
    }
}