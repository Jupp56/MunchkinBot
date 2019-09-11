using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class SpellCard : Card
    {
        public SpellEffect Effect { get; }
        public SpellCard(string name, string stickerId, SpellEffect effect = null) : base(name, stickerId)
        {
            Effect = effect ?? SpellEffect.None;
        }
    }
}
