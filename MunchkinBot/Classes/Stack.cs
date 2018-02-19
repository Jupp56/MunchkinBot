using MunchkinBot.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    public class Stack
    {
        private List<Card> cards = new List<Card>(); //top is last index, bottom 0
        private Random random = new Random();
        private Pile pile;
        public event EventHandler<string> SendMessage;
        public StackType Type { get; set; }

        public Stack(Pile pile, StackType type)
        {
            this.pile = pile;
            Type = type;
        }

        public Card TakeFirst()
        {
            if (cards.Count < 1)
            {
                pile.Refill(cards);
                Shuffle();
                switch (Type)
                {
                    case StackType.Door:
                        SendMessage(this, Strings.DoorStackRefill);
                        break;
                    case StackType.Treasure:
                        SendMessage(this, Strings.TreasureStackRefill);
                        break;
                    default:
                        SendMessage(this, "The developers fucked up.");
                        break;
                }
            }
            Card first = cards.Last();
            cards.Remove(first);
            return first;
        }

        public void PutOnTop(Card card)
        {
            cards.Add(card);
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(x => random.Next()).ToList();
        }
    }

    public enum StackType
    {
        Door,
        Treasure
    }
}
