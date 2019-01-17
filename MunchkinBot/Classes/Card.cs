using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CardType Type { get; set; }
        public int Count { get; set; } = 1;
        public int? Level { get; set; }
        public string Bonus { get; set; }
        public string BadThings { get; set; }
        public int? Loot { get; set; }
        public int? LvUp { get; set; }
        public string Events { get; set; }
        public string Traits { get; set; }
        public string Restrictions { get; set; }
        public bool? React { get; set; }
        public string Description { get; set; }

        public enum CardType
        {
            Monster,
            Clothing,
            Companion,
            Weapon,
            Curse
        }
    }
}
