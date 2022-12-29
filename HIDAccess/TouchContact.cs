namespace HIDAccess;

public class TouchContact
{
    public uint? ContactID { get; set; }
    public uint? X { get; set; }
    public uint? Y { get; set; }
    public bool touching { get; set; } = false;

    private bool _valid = false;
    
    public TouchContact(uint cID, uint x, uint y, bool t)
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
        touching = false;
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