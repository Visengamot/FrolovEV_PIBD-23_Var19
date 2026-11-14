namespace ShoeStore.WinForms.Entities;

internal static class Guard
{
    public static Guid Id(
        Guid value,
        string name
    ) =>
        value != Guid.Empty
        ? value
        : throw new ArgumentException("Идентификатор не может быть пустым.", name);

    public static string Text(
        string? value,
        string name
    ) =>
        !string.IsNullOrWhiteSpace(value)
        ? value.Trim()
        : throw new ArgumentException("Обязательное поле не заполнено.", name);

    public static decimal Positive(
        decimal value,
        string name
    ) =>
        value > 0
        ? value
        : throw new ArgumentOutOfRangeException(name, "Значение должно быть положительным.");
}
