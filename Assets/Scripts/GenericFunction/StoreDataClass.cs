using System.Collections.Generic;

[System.Serializable]
public class SaveDataAllGame
{
    // Variable that need to be stored
    public int i_HighScore;
    public int i_NbTotalOboles;
    public List<int> i_Top10Scores;
}

[System.Serializable]
public class SaveDataLastGame
{
    // Variable used by the game to store data
    public int i_Score;
    public int i_NbOboles;
}