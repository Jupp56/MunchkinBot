using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    public class Player
    {
        public int Id { get; set; }
        public List<Card> Hand = new List<Card>();
    }
}
