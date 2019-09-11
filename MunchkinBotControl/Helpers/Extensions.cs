using MunchkinBotControl.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MunchkinBotControl.Helpers
{
    public static class Extensions
    {
        public static string FirstWord(this string str) => str.Contains(" ") ? str.Remove(str.IndexOf(" ")) : str;

        public static bool IsGlobalAdmin(this User user, MunchkinContext db) => user.FindOrCreateBotUser(db).IsGlobalAdmin;

        public static BotUser FindOrCreateBotUser(this User user, MunchkinContext db)
        {
            if (db.Users.Any(x => x.Id == user.Id))
            {
                return db.Users.Find(user.Id);
            }
            else
            {
                var added = db.Users.Add(BotUser.FromUser(user));
                db.SaveChanges();
                return added;
            }
        }

        public static string FullName(this User user)
        {
            return string.Join(" ", user.FirstName, user.LastName).Trim();
        }

        public static bool IsGroup(this Chat chat) => chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup;
    }
}
