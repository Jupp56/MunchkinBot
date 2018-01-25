using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


namespace MunchkinBot
{
    class Program
    {
        #region MAIN
        static void Main(string[] args)
        {
            Program Program = new Program();
            Console.WriteLine("<Bot startet...>\n");
            Console.WriteLine("Munchkin Bot v. (alpha) 0.0.1 \n@Author: Olfi01 und SAvB\n\nNur zur privaten Verwendung! Munchkin: (c) Steve Jackson Games 2001 und Pegasus Spiele 2003 für die deutsche Übersetzung.\nAlle Rechte bleiben bei den entsprechenden Eigentümern\n");

            bool started = Program.startBot();
            
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

        public bool startBot()
        {
            bool started = false;
            //Hier die Startüberprüfungen (Datenbank???)(Token überprüfen...)
            try
            {
                if (!File.Exists(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin"));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nKein gespeichertes Telegram-Bot-Token gefunden. Bitte eingeben:\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    token = Console.ReadLine();
                    File.WriteAllText(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf", token);
                }

                else
                {
                    token = File.ReadAllText(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf"));                                     
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n<Error> Fehler beim Lesen des Tokens! Fehler: {0}", ex);
                token = "Fehler";
            }

            Console.Write("\n{0}\n",token);

            if (token == "Fehler")
            {
                started = false;
            }
            else
            {
                started = true;
            }

            Console.WriteLine(started.ToString());
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

        #region variables

        string token = "0";

        #endregion
    }

}