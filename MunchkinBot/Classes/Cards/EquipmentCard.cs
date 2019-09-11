using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class EquipmentCard : Card
    {
        public int GoldValue { get; }
        public EquipmentEffect Effect { get; }
        public EquipmentAttributes Attributes { get; }
        public EquipmentCard(string name, string stickerId, int goldValue = 0, EquipmentEffect effect = null, EquipmentAttributes attributes = null) : base(name, stickerId)
        {
            GoldValue = goldValue;
            Effect = effect ?? EquipmentEffect.None;
            Attributes = attributes ?? EquipmentAttributes.None;
        }
    }
}
