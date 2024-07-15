public enum GameTags
{
    Player,
    Enemy,
    Bullet,
    Skill
}

public class Tool
{
    public static string GetTag(GameTags value)
    {
        return value.ToString();
    }
}

