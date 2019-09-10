using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    // for example, Super Munchkin
    public class MechanicCard : Card
    {
        public MechanicEffect Effect { get; }
        public MechanicCard(string name, string stickerId, MechanicEffect effect = null) : base(name, stickerId)
        {
            Effect = effect ?? MechanicEffect.None;
        }
    }
}
