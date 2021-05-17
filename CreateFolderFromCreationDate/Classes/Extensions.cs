using System.Collections.Generic;
using System.Linq;

namespace CreateFolderFromCreationDate
{
    /// <summary>
    /// Helper methods for the lists.
    /// </summary>
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }

    public static class ExtensionDeleteToRecycleBin
    {
        /// <summary>  
        /// Delete File To Recycle Bin  
        /// WARMING: NETWORK FILES DON'T GO TO RECYCLE BIN  
        /// </summary>  
        /// <param name="file"></param>  
        public static void FileRecycle(this string file)
            =>
        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file,
            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

        /// <summary>  
        /// Delete Path To Recycle Bin  
        /// WARMING: NETWORK PATHS DON'T GO TO RECYCLE BIN  
        /// </summary>  
        /// <param name="path"></param>  
        public static void DirectoryRecycle(this string path)
            =>
        Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path,
            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
    }
}
