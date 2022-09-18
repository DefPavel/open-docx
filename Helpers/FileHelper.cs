using Microsoft.Win32;
using System.Diagnostics;
namespace open_docx.Helpers;
public static class FileHelper
{
    public static bool CheckClasses()
    {
        // Если реестр есть 
        #pragma warning disable CA1416 // Validate platform compatibility
        if (Registry.ClassesRoot.OpenSubKey("openDocx\\shell\\open\\command") != null)
            return true;
        else return false;
        #pragma warning restore CA1416 // Validate platform compatibility
    }

    public static void CreateRegister()
    {
        // путь к файлу
        var exePath = AppDomain.CurrentDomain.BaseDirectory;
        #pragma warning disable CA1416 // Validate platform compatibility
        Registry.ClassesRoot.CreateSubKey("openDocx").SetValue("", "");
        Registry.ClassesRoot.CreateSubKey("openDocx").SetValue("URL Protocol", "");
        Registry.ClassesRoot.CreateSubKey("openDocx\\shell\\open\\command").SetValue("", $"\"{exePath}open-docx.exe\" \"%1\"");
        #pragma warning restore CA1416 // Validate platform compatibility
    }

    public static bool SaveReport(Stream inputStream, string reportName)
    {
        // Получить путь рабочего стола
        var path = "D:";

        var file = path + $"\\documents-programm\\{reportName.Replace(" ", "_")}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.docx";
        if (Directory.Exists(path + $"\\documents-programm"))
        {
            using var outputFileStream = new FileStream(file, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }
        else
        {
            Directory.CreateDirectory(path + $"\\documents-programm");
            using var outputFileStream = new FileStream(file, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }
        inputStream.Dispose();
        var docxUrl = EnumerateAllFiles("C:\\Program Files", "WINWORD.EXE");
        if (docxUrl.Any())
        {
            _ = Process.Start(@$"""{docxUrl.First()}""", file);
            return  true;
        }
        else return false;

    }
    public static IEnumerable<string> EnumerateAllFiles(string path, string pattern)
    {
        IEnumerable<string>? files = null;
        try { files = Directory.EnumerateFiles(path, pattern); }
        catch { }

        if (files != null)
        {
            foreach (var file in files) yield return file;
        }

        IEnumerable<string>? directories = null;
        try { directories = Directory.EnumerateDirectories(path); }
        catch { }

        if (directories != null)
        {
            foreach (var file in directories.SelectMany(d => EnumerateAllFiles(d, pattern)))
            {
                yield return file;
            }
        }
    }
}

