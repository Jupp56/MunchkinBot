using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MunchkinBot
{
    class Program
    {
        #region MAIN
        static void Main(string[] args)
        {
            Console.WriteLine("<Bot startet...>\n");
            Console.WriteLine("Munchkin Bot v. (alpha) 0.0.1 \n@Author: Olfi01 und SAvB\n\nNur zur privaten Verwendung! Munchkin: (c) Steve Jackson Games 2001 und Pegasus Spiele 2003 für die deutsche Übersetzung.\nAlle Rechte bleiben bei den entsprechenden Eigentümern\n");

            bool started = startBot();

            if (started == false)
            {
                Console.WriteLine("Es gab ein Problem mit dem Start des Bots. Entweder kann keine Verbindung zum Telegram-Server hergestellt werden, oder das Token ist falsch, oder...\nDetailiertere Fehlerbeschreibung oben.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            Console.WriteLine("<Bot gestartet!>\n");

            Console.ReadKey();
            stopBot();
        }
        #endregion

        #region StartandStop

        static bool startBot()
        {
            bool started = false;
            //Hier die Startüberprüfungen (Datenbank???)(Token überprüfen...)

            started = true;
            return started;
        }

        static void stopBot()
        {
            //Cleanup-Zeug... falls da was sein sollte          
            Console.WriteLine("<Bot hält an...");
            Thread.Sleep(1000);
            Console.WriteLine("<Bot beendet>");
            Environment.Exit(0);

        }

        #endregion
    }
}
