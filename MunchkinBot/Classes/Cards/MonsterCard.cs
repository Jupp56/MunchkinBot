using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Cards
{
    public class MonsterCard : Card
    {
        public int Level { get; }
        public int RewardLevels { get; }
        public int RewardTreasures { get; }
        public BadStuff BadStuff { get; }
        public MonsterBehavior Behavior { get; }
        public MonsterCard(string name, string stickerId, int level, int rewardLevels = 1, int rewardTreasures = 1, BadStuff badStuff = null, MonsterBehavior behavior = null) : base(name, stickerId)
        {
            Level = level;
            RewardLevels = rewardLevels;
            RewardTreasures = rewardTreasures;
            BadStuff = badStuff ?? BadStuff.None;
            Behavior = behavior ?? MonsterBehavior.Default;
        }
    }
}
