namespace Lib.JSON.Converters
{
    internal interface IEntityKeyMember
    {
        string Key { get; set; }

        object Value { get; set; }
    }
}