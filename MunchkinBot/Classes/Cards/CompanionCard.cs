using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class CompanionCard : Card
    {
        public CompanionEffect Effect { get; }
        public CompanionCard(string name, string stickerId, CompanionEffect effect = null) : base(name, stickerId)
        {
            Effect = effect ?? CompanionEffect.None;
        }
    }
}
