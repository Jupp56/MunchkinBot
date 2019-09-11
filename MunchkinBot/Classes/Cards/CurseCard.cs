using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class CurseCard : Card
    {
        public CurseEffect Effect { get; }
        public CurseCard(string name, string stickerId, CurseEffect effect = null) : base(name, stickerId)
        {
            Effect = effect ?? CurseEffect.None;
        }
    }
}
