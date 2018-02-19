using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    public class Game
    {
        public long GroupId { get; set; }
        public List<Player> Players = new List<Player>();
        public Pile DoorPile = new Pile();
        public Pile TreasurePile = new Pile();
        public Stack DoorStack;
        public Stack TreasureStack;
        public event EventHandler<string> SendMessage;

        public Game()
        {
            DoorStack = new Stack(DoorPile, StackType.Door);
            DoorStack.SendMessage += SendMessage;
            TreasureStack = new Stack(TreasurePile, StackType.Treasure);
            TreasureStack.SendMessage += SendMessage;
        }
    }
}
