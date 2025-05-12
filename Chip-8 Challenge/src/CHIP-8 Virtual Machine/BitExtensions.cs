namespace CHIP_8_Virtual_Machine
{
    public static class BitExtensions
    {
        public static byte SetBit(this byte input, int bitIndex, bool state)
        {
            // sets bit at bitIndex in the input byte to true or false based on the state parameter
            return state switch
            {
                // the expression (1 << bitIndex) creates a mask byte with a single bit set at the bitIndex position
                true => (byte)(input | (1 << bitIndex)), // we or the input byte with the mask byte to set just the bit we need
                false => (byte)(input & ~(1 << bitIndex)) // we and the input byte with the inverse of the mask byte to clear just the bit we need
            };
        }

        public static bool GetBit(this byte input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
        }
    }
}
