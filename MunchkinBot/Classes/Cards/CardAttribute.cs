using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class CardAttribute : Attribute
    {
        public int DefaultNumberInDeck { get; set; } = 1;
    }
}
