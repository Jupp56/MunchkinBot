using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace MunchkinBot.Classes
{
    public class Game
    {
        #region variables

        public long GroupId { get; set; }
        public List<Player> Players = new List<Player>();
        public Pile DoorPile = new Pile();
        public Pile TreasurePile = new Pile();
        public Stack DoorStack;
        public Stack TreasureStack;
        public event EventHandler<string> SendMessage;
        private static TelegramBotClient Bot; //do we do this from here?

        #endregion

        #region constructor

        public Game(List<Player> PlayerIDs)
        {
            
            DoorStack = new Stack(DoorPile, StackType.Door);
            DoorStack.SendMessage += SendMessage;
            TreasureStack = new Stack(TreasurePile, StackType.Treasure);
            TreasureStack.SendMessage += SendMessage;
            Players = PlayerIDs;
            SendStartingMessage();

        }

        private void SendStartingMessage()
        {
            foreach (Player p in Players) //does it work like this?
            {
                Bot.SendTextMessageAsync(p.ToString(), "Hallo! Willkommen zum MunchkinBot. Du hast erfolgreich das Spiel betreten!"); //text is subject to change
            }
        }

        #endregion
    }
}
