using MunchkinBotControl.DB;
using MunchkinBotControl.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MunchkinBotControl
{
    class Program
    {
        private const string repoContainingDir = "C:\\Jupp56\\MunchkinBot\\repo\\";
        private const string nodesDir = "C:\\Jupp56\\MunchkinBot\\nodes\\";
        private const string repoDir = repoContainingDir + "MunchkinBot\\";
        private const string singleNodeDir = repoDir + "MunchkinBot\\bin\\Release\\";
        private const string nodeExecutableName = "MunchkinBot.exe";
        private static readonly List<Node> nodes = new List<Node>();
        private static string token;
        private static TelegramBotClient Bot;
        private static readonly MunchkinContext db = new MunchkinContext();

        static void Main(string[] args)
        {
            token = args[0];

            #region Init
            token = args[0];
            Bot = new TelegramBotClient(token);
            Bot.OnUpdate += Bot_OnUpdate;
            
            Directory.CreateDirectory(repoContainingDir);
            Directory.CreateDirectory(nodesDir);
            #endregion

            StartNewNode(x => Console.WriteLine(x));
            Bot.StartReceiving();

            string input;
            do
            {
                Console.WriteLine();
                input = Console.ReadLine();
                Guid nodeGuid;
                switch (input.FirstWord())
                {
                    case "startnode":
                        StartNewNode(x => Console.WriteLine(x));
                        break;
                    case "stop":
                        if (!input.Contains(" "))
                        {
                            Console.WriteLine("Usage: stop [node-guid]");
                            break;
                        }
                        if (!Guid.TryParse(input.Substring(input.IndexOf(" ")).Trim(), out nodeGuid))
                        {
                            Console.WriteLine("Invalid guid.");
                            break;
                        }
                        nodes.ForEach(x => { if (x.Guid.Equals(nodeGuid)) x.Stop(); });
                        Console.WriteLine("Stopping node with guid {0}.", nodeGuid.ToString());
                        break;
                    case "version":
                        if (!input.Contains(" "))
                        {
                            Console.WriteLine("Usage: version [node-guid]");
                            break;
                        }
                        if (!Guid.TryParse(input.Substring(input.IndexOf(" ")).Trim(), out nodeGuid))
                        {
                            Console.WriteLine("Invalid guid.");
                            break;
                        }
                        nodes.ForEach(x => { if (x.Guid.Equals(nodeGuid)) Console.WriteLine(x.Version); });
                        break;
                    case "exit":
                        if (nodes.Any(x => !x.Stopped))
                        {
                            Console.WriteLine("Stopping all nodes...");
                            nodes.ForEach(x => x.Stop());
                            while (nodes.Any(x => !x.Stopped)) { }
                            Console.WriteLine("Nodes stopped.");
                        }
                        break;
                    case "nodes":
                        nodes.RemoveAll(x => x.Stopped && !x.Errored);
                        nodes.ForEach(x => Console.WriteLine("Node {0} ({1}, {3}): Stopped={2}, Errored={4}", x.Process.ProcessName, x.Guid.ToString(), x.Stopped, x.Version, x.Errored));
                        if (nodes.Count < 1) Console.WriteLine("No nodes present.");
                        break;
                    default:
                        Console.WriteLine("Command does not exist.");
                        break;
                }
            } while (input.FirstWord() != "exit");

            #region Tidy up
            Bot.StopReceiving();
            db.Dispose();
            ClearRecursively(Directory.CreateDirectory(nodesDir));
            #endregion
        }

        private static void ClearRecursively(DirectoryInfo directoryInfo)
        {
            foreach (var dir in directoryInfo.EnumerateDirectories())
            {
                ClearRecursively(dir);
                dir.Delete();
            }
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
        }

        private static void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            if (e.Update.Type == UpdateType.CallbackQuery && e.Update.CallbackQuery.Data == "update")
            {
                if (!e.Update.CallbackQuery.From.IsGlobalAdmin(db))
                {
                    Bot.AnswerCallbackQueryAsync(e.Update.CallbackQuery.Id, "You are not authorized to do this!");
                    return;
                }

                nodes.ForEach(x => x.Stop());
                Telegram.Bot.Types.Message cmsg = e.Update.CallbackQuery.Message;
                string msgText = "Updating...\n";
                Task.Run(() => StartNewNode(x =>
                {
                    msgText += x + "\n";
                    if (msgText.Trim('\n', ' ') != cmsg.Text.Trim())
                        try { cmsg = Bot.EditMessageTextAsync(cmsg.Chat.Id, cmsg.MessageId, msgText).Result; } catch { }
                }));
                return;
            }

            if (e.Update.Type == UpdateType.CallbackQuery && e.Update.CallbackQuery.Data == "dontUpdate")
            {
                if (!e.Update.CallbackQuery.From.IsGlobalAdmin(db))
                {
                    Bot.AnswerCallbackQueryAsync(e.Update.CallbackQuery.Id, "You are not authorized to do this!");
                    return;
                }

                Bot.DeleteMessageAsync(e.Update.CallbackQuery.Message.Chat.Id, e.Update.CallbackQuery.Message.MessageId);
            }

            if (e.Update.Type == UpdateType.Message && e.Update.Message.Text == "/update")
            {
                if (!e.Update.Message.From.IsGlobalAdmin(db))
                {
                    Bot.SendTextMessageAsync(e.Update.Message.Chat.Id, "You are not authorized to do this!");
                    return;
                }

                nodes.ForEach(x => x.Stop());
                string msgText = "Updating...\n";
                Telegram.Bot.Types.Message cmsg = Bot.SendTextMessageAsync(e.Update.Message.Chat.Id, msgText).Result;
                Task.Run(() => StartNewNode(x =>
                {
                    msgText += x + "\n";
                    if (msgText.Trim('\n', ' ') != cmsg.Text.Trim())
                        try { cmsg = Bot.EditMessageTextAsync(cmsg.Chat.Id, cmsg.MessageId, msgText).Result; } catch { }
                }));
                return;
            }

            nodes.ForEach(x => x.SendMessage(new IpcMessage(e.Update)));
        }

        private static void StartNewNode(Action<string> outputHandler)
        {
            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "first.cmd"))
            {
                WorkingDirectory = repoContainingDir,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process.Start(psi).WaitForExit();

            outputHandler.Invoke("");
            outputHandler.Invoke("Building...");
            psi = new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "build.cmd"))
            {
                WorkingDirectory = repoContainingDir,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process buildProcess = Process.Start(psi);
            buildProcess.StandardOutput.ReadLineAsync().ContinueWith(x => PrintAndContinue(x, buildProcess.StandardOutput, outputHandler));
            buildProcess.WaitForExit();

            outputHandler.Invoke("");
            outputHandler.Invoke("Copying to directory...");
            Guid guid = Guid.NewGuid();
            var targetDir = Directory.CreateDirectory(Path.Combine(nodesDir, guid.ToString()));
            CopyDirectory(Directory.CreateDirectory(singleNodeDir), targetDir);

            psi = new ProcessStartInfo(Path.Combine(targetDir.FullName, nodeExecutableName))
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = targetDir.FullName
            };

            nodes.Add(new Node(psi, guid, token));

            outputHandler.Invoke("");
            outputHandler.Invoke($"Node {guid.ToString()} started.");
        }

        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var dir in source.EnumerateDirectories())
            {
                CopyDirectory(dir, target.CreateSubdirectory(dir.Name));
            }
            foreach (var file in source.EnumerateFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name));
            }
        }

        private static void PrintAndContinue(Task<string> line, StreamReader standardOutput, Action<string> outputHandler)
        {
            if (line.Result == null) return;
            outputHandler.Invoke(line.Result);
            standardOutput.ReadLineAsync().ContinueWith(x => PrintAndContinue(x, standardOutput, outputHandler));
        }
    }
}
