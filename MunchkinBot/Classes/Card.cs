using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Card
    {
        [JsonProperty(PropertyName = "card_id")]
        public int CardId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public CardType Type { get; set; }
        [JsonProperty(PropertyName = "level")]
        public int? Level { get; set; }
        [JsonProperty(PropertyName = "bonus", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<int, int?> Bonus { get; set; }
        //TODO: Figure out how to store bad things
        [JsonProperty(PropertyName = "bad_things")]
        public string BadThings { get; set; }
        [JsonProperty(PropertyName = "loot")]
        public int? Loot { get; set; }
        [JsonProperty(PropertyName = "lv_up")]
        public int? LvUp { get; set; }
        //TODO: Find out how to store events
        [JsonProperty(PropertyName = "events")]
        public string Events { get; set; }
        //TODO: Find out how to store traits
        [JsonProperty(PropertyName = "traits")]
        public string Traits { get; set; }
        //TODO: Find out how to store restrictions
        [JsonProperty(PropertyName = "restrictions")]
        public string Restrictions { get; set; }
        /// <summary>
        /// Whether it's possible to react to the card
        /// </summary>
        [JsonProperty(PropertyName = "react")]
        public bool? React { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum CardType
        {
            Monster,
            Clothing,
            Companion,
            Weapon,
            Curse,
            Class,
            Race
        }
    }
}
