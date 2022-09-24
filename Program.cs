using open_docx.Helpers;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

var handle = GetConsoleWindow();

// Скрыть консоль
ShowWindow(handle, 0);

// Если реестр не найден, тогда создать
if (!FileHelper.CheckClasses())
{
    // Создать реестр
    FileHelper.CreateRegister();
}

// Если параметр не пустой
if (args != null && args.Length > 0)
{
    var split = args[0].Split("jmuagent://");
    try
    {
        if (split[1].Contains("word"))
        {
            var result = split[1].Split("word");
            // Делаем запрос на программу
            await ApiHelper.JsonPostWithToken("secret", "http://jmu.api.lgpu.org/reports-education/" + result[1], "GET", "Отчет" , "word");
        }
        else if (split[1].Contains("excel"))
        {
            var result = split[1].Split("excel");
            // Делаем запрос на программу
            await ApiHelper.JsonPostWithToken("secret", "http://jmu.api.lgpu.org/reports-education/" + result[1], "GET", "Отчет" , "excel");
        }
    }
    catch (Exception ex)
    {
        ShowWindow(handle, 1);
        Console.WriteLine(ex.Message);
        Console.ReadKey();
    }  
}