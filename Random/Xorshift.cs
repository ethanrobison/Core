namespace Core.Random
{
    public class Xorshift
    {
        private uint _state;
        public Xorshift(uint seed) {
            _state = seed;
        }

        public uint Next() {
            _state ^= _state << 13;
            _state ^= _state >> 17;
            _state ^= _state << 5;
            return _state;
        }

        public uint Next(uint min, uint max) {
            if (min >= max) {
                Logging.Error("Min must be strictly less than max for generation", min, max);
                return 0;
            }

            uint res = Next();
            uint diff = max - min;
            uint offset = res % diff;
            return offset + min;
        }

        public int Next(int min, int max) {
            if (min >= max) {
                Logging.Error("Min must be strictly less than max for generation", min, max);
                return 0;
            }

            uint res = Next();
            // Okay because for any two ints a,b, a>b, a - b always fits in a uint
            uint diff = (uint)(max - min);
            uint offset = res % diff;
            return min + (int)offset;
        }
    }
}
