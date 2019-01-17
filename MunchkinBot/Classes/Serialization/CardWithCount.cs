using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Classes.Serialization
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class CardWithCount
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "card")]
        public Card Card { get; set; }
    }
}
