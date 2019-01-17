using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Dungeon
    {
        [JsonProperty(PropertyName = "dungeon_id")]
        public int DungeonId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        //TODO: find out how to store events
        [JsonProperty(PropertyName = "events")]
        public string Events { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "bonus", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<int, int?> Bonus { get; set; }

        public int Id { get; set; }
    }
}
