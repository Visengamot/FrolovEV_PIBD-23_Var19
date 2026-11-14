namespace ShoeStore.WinForms.Entities.Enums;

[Flags]
public enum Season
{
    None = 0,
    Spring = 1,
    Summer = 2,
    Autumn = 4,
    Winter = 8,
    All = 15
}
