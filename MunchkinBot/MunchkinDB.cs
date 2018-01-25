using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot
{
    public class MunchkinDB : DbContext
    {
        public MunchkinDB(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public virtual DbSet<Card> CardStock { get; set; }
        public virtual DbSet<Dungeon> DungeonStock { get; set; }
    }
}
