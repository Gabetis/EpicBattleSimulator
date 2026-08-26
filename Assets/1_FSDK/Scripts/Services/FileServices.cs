namespace FSDK
{
    using System.IO;

    public static class FileServices
    {
        public static void WriteContentToTextFile(string path, string content, bool append)
        {
            using (var sw = new StreamWriter(path, append))
            {
                sw.WriteLine(content);
                sw.Dispose();
                sw.Close();
            }
        }
        
        public static string ReadContentFromTextFile(string path)
        {
            if (File.Exists(path))
            {
                string content = string.Empty;
                using (var sr = new StreamReader(path))
                {
                    content = sr.ReadToEnd();
                    sr.Dispose();
                    sr.Close();
                }
                return content;
            }

            return string.Empty;
        }

        public static string ReadContentFromTextFile(string path, string nextLineSeparator)
        {
            if (File.Exists(path))
            {
                string content = string.Empty;
                using (var sr = new StreamReader(path))
                {
                    while (sr.Peek() > -1)
                    {
                        content += sr.ReadLine() + nextLineSeparator;
                    }
                    sr.Dispose();
                    sr.Close();
                }
                return content;
            }

            return string.Empty;
        }
    }
}