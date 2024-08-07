namespace BuildingBlocks;

public static class ListExtensions
{
    public static string JoinListToString(this List<string> list, string connectCharacter = "; ")
    {
        if (list == null)
        {
            return string.Empty;
        }
        return string.Join(connectCharacter, list);
    }
}

