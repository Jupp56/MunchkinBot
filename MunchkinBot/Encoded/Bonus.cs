namespace MunchkinBot.Encoded
{
    public class Bonus
    {
        private short[] boni = new short[200];

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
    }
}
