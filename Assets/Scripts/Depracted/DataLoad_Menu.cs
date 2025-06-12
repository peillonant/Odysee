using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoad_Menu : MonoBehaviour
{
    public static DataLoad_Menu instance;

    // Launch the persistence of the gameObject
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;
    }

    [SerializeField] private int i_HighScore = 0;
    [SerializeField] private int i_NbTotalOboles = 0;
    [SerializeField] private int i_NbObolesToAdd = 0;
    [SerializeField] private int i_newScoreToAdd = 0;
    [SerializeField] private List<int> i_Top10Scores = new();

    private bool b_NewObolesToAdd = false;
    private int i_PreviousNbTotalOboles = 0;

    /*********************************************** ENCAPSULATION FUNCTION ******************************************************/

    public int GetTopScoreX(int i_indexTopScore) => i_Top10Scores[i_indexTopScore];
    public int GetNumberTopScore() => i_Top10Scores.Count;
    public bool IsNewObolesToAdd() => b_NewObolesToAdd;
    public int GetNewObolesToAdd() => i_NbObolesToAdd;
    public int GetNbObolesPreviously() => i_PreviousNbTotalOboles;
    public int GetNbObolesTotal() => i_NbTotalOboles;

    /*****************************************************************************************************************************/

    void Start()
    {
        LoadDataAllGame();
        b_NewObolesToAdd = false;
        LoadDataLastGame();
    }

    public void LoadDataAllGame()
    {
        string path = Application.persistentDataPath + "/savefileAllGame.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataAllGame data = JsonUtility.FromJson<SaveDataAllGame>(json);

            // Variable that need to be stored
            i_HighScore = data.i_HighScore;
            i_NbTotalOboles = data.i_NbTotalOboles;
            //i_Top10Scores = data.i_Top10Scores;
        }
    }

    public void LoadDataLastGame()
    {
        string path = Application.persistentDataPath + "/savefileLastGame.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataLastGame data = JsonUtility.FromJson<SaveDataLastGame>(json);

            i_newScoreToAdd = data.i_Score;
            i_NbObolesToAdd = data.i_NbOboles;

            UpdateDataAllGames();
        }
    }

    // Method to update the List Score with the new Score added by the previous game
    private void UpdateDataAllGames()
    {
        // Loop to add the new score on the list and push the last item out of the list
        for (int i = 0; i < i_Top10Scores.Count; i++)
        {
            if (i_newScoreToAdd > i_Top10Scores[i])
            {
                i_Top10Scores.Insert(i, i_newScoreToAdd);

                // Condition to update the highscore if the newScore become the new HighScore
                if (i == 0)
                    i_HighScore = i_newScoreToAdd;

                break;
            }
        }

        // Condition to add the newScore when the leaderboard is not full and the loop before was not able to do it
        if (!i_Top10Scores.Contains(i_newScoreToAdd) && i_Top10Scores.Count < 10)
        {
            i_Top10Scores.Add(i_newScoreToAdd);
        }

        // Condition to remove the top score that is now 11
        if (i_Top10Scores.Count > 10)
        {
            i_Top10Scores.RemoveAt(i_Top10Scores.Count - 1);
        }

        // Variable modification regarding the Oboles 
        b_NewObolesToAdd = true;
        i_PreviousNbTotalOboles = i_NbTotalOboles;
        i_NbTotalOboles += i_NbObolesToAdd;
    }

    public void WriteResetDataLastGame()
    {
        SaveDataLastGame data = new();

        // Variables that need to be stored
        data.i_Score = 0;
        data.i_NbOboles = 0;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefileLastGame.json", json);
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }

    public void WriteDataAllGame()
    {
        SaveDataAllGame data = new();

        // Variables that need to be stored
        data.i_HighScore = i_HighScore;
        data.i_NbTotalOboles = i_NbTotalOboles;
        //data.i_Top10Scores = i_Top10Scores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefileAllGame.json", json);
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }
}
