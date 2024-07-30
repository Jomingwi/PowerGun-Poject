public enum GameTags
{
    Player,
    Enemy,
    Bullet,
    Skill
}

public enum Difficulty
{
  Easy,
  Normal,
  Hard
}


public class Tool
{
    public static string GetTag(GameTags value)
    {
        return value.ToString();
    }
   
}



