using System;

namespace DesignStudio.UI
{
    public static class UIHelpers
    {
        public static void SafeClear()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                    Console.Clear();
            }
            catch { }
        }

        public static void SafeReadKey()
        {
            Console.WriteLine("Натисніть Enter...");
            try
            {
                if (!Console.IsInputRedirected)
                    Console.ReadKey();
                else
                    Console.ReadLine();
            }
            catch
            {
                Console.ReadLine();
            }
        }
    }
}
