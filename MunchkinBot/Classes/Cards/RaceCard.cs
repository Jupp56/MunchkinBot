using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class RaceCard : Card
    {
        public Races Race { get; }
        public RaceCard(string name, string stickerId, Races race) : base(name, stickerId)
        {
            Race = race;
        }

        public enum Races
        {
            Elf,
            Dwarf,
            Halfling,
            Orc
        }
    }
}
