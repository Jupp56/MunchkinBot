namespace MunchkinBot.Encoded
{
    public class Bonus
    {
        private short[] boni = new short[200];

        public Bonus(string from)
        {
            string[] blocks = from.Split(';');
            foreach(string block in blocks)
            {
                string[] split = block.Split(':');
                boni[int.Parse(split[0])] = short.Parse(split[1]);
            }
        }

        public short this[int index]
        {
            get
            {
                if (boni[index] == 0) return boni[Targets.AllNotSpecified];
                else return boni[index];
            }
            set
            {
                boni[index] = value;
            }
        }

        public short All => this[Targets.All];
        public short AllNotSpecified => this[Targets.AllNotSpecified];
        public short Human => this[Targets.Human];
    }
}
