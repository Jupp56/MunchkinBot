//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MunchkinBot
{
    using System;
    using System.Collections.Generic;
    
    public partial class Card
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Card()
        {
            this.Count = 1;
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
        public Nullable<int> Level { get; set; }
        public string Bonus { get; set; }
        public string BadThings { get; set; }
        public Nullable<int> Loot { get; set; }
        public Nullable<int> LvUp { get; set; }
        public string Events { get; set; }
        public string Traits { get; set; }
        public string Restrictions { get; set; }
        public Nullable<bool> React { get; set; }
        public string Description { get; set; }
    }
}
