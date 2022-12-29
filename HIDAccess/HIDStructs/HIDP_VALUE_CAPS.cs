using System.Runtime.InteropServices;

namespace HIDAccess.HIDStructs;

[StructLayout(LayoutKind.Sequential)]
public struct HIDP_VALUE_CAPS
{
    public ushort   UsagePage;
    public byte   ReportID;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool IsAlias;
    
    public ushort  BitField;
    public ushort  LinkCollection;
    public ushort   LinkUsage;
    public ushort   LinkUsagePage;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool IsRange;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool IsStringRange;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool IsDesignatorRange;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool IsAbsolute;
    
    [MarshalAs(UnmanagedType.U1)]
    public bool HasNull;
    
    public byte   Reserved;
    public ushort BitSize;
    public ushort ReportCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public ushort[] Reserved2;
    public uint   UnitsExp;
    public uint   Units;
    public int    LogicalMin;
    public int    LogicalMax;
    public int    PhysicalMin;
    public int    PhysicalMax;
    
    public ushort UsageMin;
    public ushort UsageMax;
    public ushort StringMin;
    public ushort StringMax;
    public ushort DesignatorMin;
    public ushort DesignatorMax;
    public ushort DataIndexMin;
    public ushort DataIndexMax;

    // NotRange
    public ushort Usage => UsageMin;
    public ushort StringIndex => StringMin;
    public ushort DesignatorIndex => DesignatorMin;
    public ushort DataIndex => DataIndexMin;
    
}