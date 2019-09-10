using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class ClassCard : Card
    {
        public Classes Class { get; }

        public ClassCard(string name, string stickerId, Classes @class) : base(name, stickerId)
        {
            Class = @class;
        }

        public enum Classes
        {
            Cleric,
            Warrior,
            Thief,
            Wizard
        }
    }
}
