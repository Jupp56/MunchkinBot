using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaltMalKurzNode.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Trigger { get; }
        public bool Standalone { get; set; } = true;
        public bool RequiresAdmin { get; set; } = false;
        public bool RequiresGlobalAdmin { get; set; } = false;
        public Context RequiredContext { get; set; } = Context.All;
        public string Usage { get; set; }
        public string Description { get; set; }
        public bool ProcessOnAllNodes { get; set; } = true;
        public bool ExecuteAsync { get; set; } = false;

        public CommandAttribute(string trigger)
        {
            Trigger = trigger;
        }

        public enum Context
        {
            All,
            Private,
            Group
        }
    }
}
