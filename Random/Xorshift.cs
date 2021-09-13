using Core.SaveLoad;

namespace Core.Random
{
    /// <summary>
    /// Xorshift implementation. Based off of https://www.jstatsoft.org/article/view/v008i14, p4.
    /// </summary>
    [SaveLoadClass(SaveLoadClassAttribute.SaveLoadOption.Explicit)]
    public class Xorshift
    {
        [SaveLoad] private uint _state;

        public Xorshift() { }

        public Xorshift(uint seed) {
            Logging.Assert(seed > 0, "Cannot seed an Xorshift with 0.");
            _state = seed;
        }

        public bool CoinToss() => Next(1) == 0;

        public uint Next() {
            Logging.Assert(_state != 0, "Ended up with an xorshift at 0");

            _state ^= _state << 13;
            _state ^= _state >> 17;
            _state ^= _state << 5;
            return _state;
        }

        public uint Next(uint max) => Next(0, max);
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

        public int Next(int max) => Next(0, max);
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

        private const float UINT_TO_FLOAT = (1.0f / 4294967296.0f);
        public float Next(float max) => Next(0.0f, max);
        public float Next(float min, float max) {
            if (min >= max) {
                Logging.Error("Min must be strictly less than max for generation", min, max);
                return 0f;
            }

            float res = Next() * UINT_TO_FLOAT;
            float diff = max - min;
            return min + res * diff;
        }
    }
}
