public enum GameTags
{
    Player,
    Enemy,
    Bullet,
    Skill,
    EnemyBoss
}

public enum Difficulty
{
  Easy,
  Normal,
  Hard
}


public class Tool
{
    public static bool isStartMainScene = false;
    public static string difficultKey = "DifficultKey";

    public static string GetTag(GameTags value)
    {
        return value.ToString();
    }
   
}



