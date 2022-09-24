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
/*if (!FileHelper.CheckClasses())
{
    // Создать реестр
    FileHelper.CreateRegister();
}
*/

// Если параметр не пустой
if (args != null && args.Length > 0)
{
    // Создать реестр
    FileHelper.CreateRegister();
    // отрезаем первую часть
    var split = args[0].Split("jmuagent://");
    // Делаем запрос на программу
    await ApiHelper.JsonPostWithToken("secret", "http://jmu.api.lgpu.org/reports-education/" + split[1], "GET", "Отчет");
}