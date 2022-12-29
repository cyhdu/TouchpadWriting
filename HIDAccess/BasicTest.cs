namespace HIDAccess;


public class BasicTest : InputDevice
{
    public int X { get; set; }
    public int Y { get; set; }
    public double ret =>
        Math.Sqrt(X * X + Y * Y);
}