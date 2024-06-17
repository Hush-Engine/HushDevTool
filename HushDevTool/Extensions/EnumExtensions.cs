namespace HushDevTool.Extensions;

public static class EnumExtensions
{
    public static bool HasCompositeFlagUInt32(this Enum @base, Enum flag)
    {
        uint baseValue = Convert.ToUInt32(@base);
        uint flagValue = Convert.ToUInt32(flag);
        return (baseValue & flagValue) != 0;
    }
}