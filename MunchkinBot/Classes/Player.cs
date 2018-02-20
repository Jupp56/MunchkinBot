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
        public int Level { get; set; }
        public int AttackValue { get; set; }
        public List<Card> Hand = new List<Card>();
        public List<Card> OnTable = new List<Card>();
        
        public void ComputeAttackValue()
        {
            AttackValue = Level;
            foreach (Card c in OnTable)
            {
                //AttackValue += Convert.ToInt32(c.Bonus);
            }
        }
    }
}
