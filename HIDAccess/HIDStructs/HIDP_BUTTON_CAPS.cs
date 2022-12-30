using System.Runtime.InteropServices;

namespace HIDAccess.HIDStructs;

[StructLayout(LayoutKind.Explicit)]
public struct HIDP_BUTTON_CAPS
{
    [FieldOffset(0)]
    public ushort UsagePage;
    [FieldOffset(2)]
    public byte ReportID;
    [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
    public bool IsAlias;
    [FieldOffset(4)]
    public ushort BitField;
    [FieldOffset(6)]
    public ushort LinkCollection;
    [FieldOffset(8)]
    public ushort LinkUsage;
    [FieldOffset(10)]
    public ushort LinkUsagePage;
    [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
    public bool IsRange;
    [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
    public bool IsStringRange;
    [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
    public bool IsDesignatorRange;
    [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
    public bool IsAbsolute;

    [FieldOffset(16), MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    readonly int[] Reserved;
    [FieldOffset(56)]
    public ButtonCapsRange Range;
    [FieldOffset(56)]
    public ButtonCapsNotRange NotRange;
}

[StructLayout(LayoutKind.Sequential)]
    public struct ButtonCapsRange
    {
        /// <summary>
        /// Indicates the inclusive lower bound of usage range whose inclusive upper bound is specified by Range.UsageMax.
        /// </summary>
        public ushort UsageMin;
        /// <summary>
        /// Indicates the inclusive upper bound of a usage range whose inclusive lower bound is indicated by Range.UsageMin.
        /// </summary>
        public ushort UsageMax;
        /// <summary>
        /// Indicates the inclusive lower bound of a range of string descriptors
        /// (specified by string minimum and string maximum items) whose inclusive upper bound is indicated by Range.StringMax.
        /// </summary>
        public ushort StringMin;
        /// <summary>
        /// Indicates the inclusive upper bound of a range of string descriptors
        /// (specified by string minimum and string maximum items) whose inclusive lower bound is indicated by Range.StringMin.
        /// </summary>
        public ushort StringMax;
        /// <summary>
        /// Indicates the inclusive lower bound of a range of designators 
        /// (specified by designator minimum and designator maximum items) whose inclusive lower bound is indicated by Range.DesignatorMax.
        /// </summary>
        public ushort DesignatorMin;
        /// <summary>
        /// Indicates the inclusive upper bound of a range of designators 
        /// (specified by designator minimum and designator maximum items) whose inclusive lower bound is indicated by Range.DesignatorMin.
        /// </summary>
        public ushort DesignatorMax;
        /// <summary>
        /// Indicates the inclusive lower bound of a sequential range of data indices that correspond, one-to-one and in the same order, 
        /// to the usages specified by the usage range Range.UsageMin to Range.UsageMax.
        /// </summary>
        public ushort DataIndexMin;
        /// <summary>
        ///Indicates the inclusive upper bound of a sequential range of data indices that correspond, one-to-one and in the same order, 
        ///to the usages specified by the usage range Range.UsageMin to Range.UsageMax.
        /// </summary>
        public ushort DataIndexMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ButtonCapsNotRange
    {
        /// <summary>
        /// Indicates a usage ID.        
        /// </summary>
        public ushort Usage;
        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        public ushort Reserved1;
        /// <summary>
        /// Indicates a string descriptor ID for the usage specified by NotRange.Usage.
        /// </summary>
        public ushort StringIndex;
        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        public ushort Reserved2;
        /// <summary>
        /// Indicates a designator ID for the usage specified by NotRange.Usage.
        /// </summary>
        public ushort DesignatorIndex;
        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        public ushort Reserved3;
        /// <summary>
        /// Indicates the data index of the usage specified by NotRange.Usage.
        /// </summary>
        public ushort DataIndex;
        /// <summary>
        /// Reserved for internal system use.
        /// </summary>
        public ushort Reserved4;
    }
