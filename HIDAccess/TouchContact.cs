namespace HIDAccess;

public class TouchContact
{
    private sealed class XYTouchingEqualityComparer : IEqualityComparer<TouchContact>
    {
        public bool Equals(TouchContact x, TouchContact y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Y == y.Y && x.touching == y.touching;
        }

        public int GetHashCode(TouchContact obj)
        {
            return HashCode.Combine(obj.X, obj.Y, obj.touching);
        }
    }

    public static IEqualityComparer<TouchContact> XYTouchingComparer { get; } = new XYTouchingEqualityComparer();

    public uint? ContactID { get; set; }
    public uint? X { get; set; }
    public uint? Y { get; set; }
    public int touching { get; set; } = 0;

    private bool _valid = false;
    
    public TouchContact(uint cID, uint x, uint y, int t)
    {
        ContactID = cID;
        X = x;
        Y = y;
        touching = t;
    }

    public TouchContact()
    {
        ContactID = 7;
        X = 0;
        Y = 0;
        touching = 0;
    }

    public bool IsValid()
    {
        _valid = ContactID.HasValue && X.HasValue && Y.HasValue;
        return _valid;
    }

    public override string ToString()
    {
        return $"{ContactID:D2}=> XPOS:{X:D4} YPOS:{Y:D4} Touching:{touching}";
    }
}