namespace HIDAccess;

public interface InputDevice
{
    int X { get; set; }
    int Y { get; set; }
    
    double ret { get; }
}