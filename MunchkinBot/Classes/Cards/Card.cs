using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public abstract class Card
    {
        public string Name { get; }
        public string StickerId { get; }

        public Card(string name, string stickerId)
        {
            Name = name;
            StickerId = stickerId;
        }

        /*public enum CardType
        {
            Monster,
            Equipment,
            Companion,
            Curse,
            Class,
            Race,
            Spell,
            Mechanic
        }*/
    }
}
