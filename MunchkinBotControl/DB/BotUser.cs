using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MunchkinBotControl.DB
{
    [Table("Users")]
    public class BotUser
    {
        private static readonly int[] defaultGaIds = { 275094601, 267376056 };

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => string.Join(" ", FirstName, LastName).Trim();
        public string Username { get; set; }
        public bool IsGlobalAdmin { get; set; } = false;
        public int GamesPlayed { get; set; } = 0;

        public static BotUser FromUser(User user)
        {
            return new BotUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Id = user.Id,
                IsGlobalAdmin = defaultGaIds.Contains(user.Id)
            };
        }
    }
}
