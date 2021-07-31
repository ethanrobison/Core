using System;

namespace Core.SaveLoad
{
    public class SaveLoadClassAttribute : Attribute
    {
        public enum SaveLoadOption
        {
            AllPublic = 1 << 0,
            AllProtected = 1 << 1,
            AllPrivate = 1 << 2,
            All = AllPublic | AllProtected | AllPrivate,

            Explicit = 1 << 3,
        }

        private ushort _flags;

        public SaveLoadClassAttribute(SaveLoadOption option) {
            _flags = (ushort)option;
        }

        public bool Any() => 0 != _flags;
        public bool AllPublic() => 0 != (_flags & (ushort)SaveLoadOption.AllPublic);
        public bool AllProtected() => 0 != (_flags & (ushort)SaveLoadOption.AllProtected);
        public bool AllPrivate() => 0 != (_flags & (ushort)SaveLoadOption.AllPrivate);
        public bool AnyExplicit() => 0 != (_flags & (ushort)SaveLoadOption.Explicit);
    }

    /// <summary>
    /// Use this attribute to mark up fields in a class that you'd like to be
    /// serialized.<br/>
    /// 
    /// Make sure to mark the class as using
    /// <see cref="SaveLoadClassAttribute.SaveLoadOption.Explicit">SaveLoadOption.Explicit</see>,
    /// otherwise the fields will be skipped. </summary>
    public class SaveLoadAttribute : Attribute
    {
    }
}
