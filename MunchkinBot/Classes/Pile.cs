using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    public class Pile
    {
        private List<Card> cards = new List<Card>(); //top is last index, bottom 0

        public void Refill(List<Card> cards)
        {
            foreach (Card card in this.cards)
            {
                cards.Add(card);
            }
            this.cards.Clear();
        }
    }
}
