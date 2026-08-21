using Windows.Foundation;

namespace SensorsConsole;

internal static class Printer
{
    public static void Print<T>(string name, IAsyncOperation<T> task, Func<T, string> toString, int offsetX, int offsetY) =>
        PrintAt(
            TABLE_LEFT + offsetX * TABLE_CELL_WIDTH_SHORT,
            TABLE_TOP + offsetY * TABLE_CELL_HEIGHT,
            name, task, toString);

    public static void Print<T>(string name, IAsyncOperation<T> task, Func<T, string> toString, int offsetY) =>
        PrintAt(TABLE_LEFT,
            TABLE_TOP + offsetY * TABLE_CELL_HEIGHT,
            name, task, toString);

    public static void Print(string name, string info, int offsetX, int offsetY) =>
        PrintAt(
            TABLE_LEFT + offsetX * TABLE_CELL_WIDTH_SHORT,
            TABLE_TOP + offsetY * TABLE_CELL_HEIGHT,
            name, info.PadRight(TABLE_CELL_WIDTH_SHORT));

    public static void Print(string name, string info, int offsetY) =>
        PrintAt(TABLE_LEFT,
            TABLE_TOP + offsetY * TABLE_CELL_HEIGHT,
            name, info.PadRight(TABLE_CELL_WIDTH_LONG));

    #region Internal

    const int TABLE_LEFT = 30;
    const int TABLE_TOP = 1;
    const int TABLE_CELL_WIDTH_SHORT = 15;
    const int TABLE_CELL_WIDTH_LONG = 50;
    const int TABLE_CELL_HEIGHT = 3;

    static readonly Lock _locker = new();

    private static void PrintAt<T>(int left, int top, string name, IAsyncOperation<T> task, Func<T, string> toString)
    {
        lock (_locker)
        {
            task.Wait();
            var info = toString.Invoke(task.GetResults());
            PrintAt(left, top, name, info.PadRight(15));
        }
    }

    private static void PrintAt(int left, int top, string name, string info)
    {
        lock (_locker)
        {
            Console.CursorLeft = left;
            Console.CursorTop = top;
            Console.Write($"{name}:");
            Console.CursorLeft = left;
            Console.CursorTop = top + 1;
            Console.Write(info);
        }
    }

    #endregion
}
