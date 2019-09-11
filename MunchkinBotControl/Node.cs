using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZetaIpc.Runtime.Client;
using ZetaIpc.Runtime.Helper;

namespace MunchkinBotControl
{
    internal class Node
    {
        public Process Process { get; }
        public Guid Guid { get; }
        private readonly IpcClient _client = new IpcClient();
        private readonly int clientPort;
        private readonly ManualResetEvent clientInitialized = new ManualResetEvent(false);
        private string EventHandleName { get => Guid.ToString() + ":started"; }
        public bool Stopped { get; set; } = false;
        public bool Errored { get; set; } = false;
        public Exception Exception { get; set; }
        public string Version { get { clientInitialized.WaitOne(); return SendMessage(IpcMessage.GetVersionMessage); } }

        public Node(ProcessStartInfo psi, Guid guid, string token)
        {
            Guid = guid;
            clientPort = FreePortHelper.GetFreePort();
            psi.Arguments = $"{clientPort} {EventHandleName} {token}";
            Process = Process.Start(psi);
            Task.Run(() => { try { Process.WaitForExit(); Stopped = true; } catch (Exception ex) { Errored = true; Stopped = true; Exception = ex; } });
            Task.Run((Action)StartClient);
        }

        private void StartClient()
        {
            EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, EventHandleName);
            eventWaitHandle.WaitOne();
            _client.Initialize(clientPort);
            clientInitialized.Set();
        }

        public string SendMessage(IpcMessage message)
        {
            clientInitialized.WaitOne();
            try
            {
                return _client.Send(message.ToString());
            }
            catch (Exception ex)
            {
                Exception = ex;
                return null;
            }
        }

        public void Stop()
        {
            SendMessage(IpcMessage.StopMessage);
        }
    }
}
