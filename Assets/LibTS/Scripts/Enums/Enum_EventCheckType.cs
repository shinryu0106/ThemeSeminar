using System;

namespace LibTS
{
    [Flags]
    public enum EventCheckType
    {
        None = 0,
        Layer = 1 << 0,
        Tag = 1 << 1,
        Distance = 1 << 2,
        Name = 1 << 3,
    }
}