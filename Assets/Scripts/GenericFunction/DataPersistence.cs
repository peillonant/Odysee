using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence instance;

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
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private int i_HighScore = 0;
    [SerializeField] private int i_NbTotalOboles = 0;
    [SerializeField] private List<Score> i_Top10Scores = new();

    private int i_PreviousNbTotalOboles = 0;

    /*********************************************** ENCAPSULATION FUNCTION ******************************************************/
    public int GetHighScore() => i_HighScore;
    public Score GetTopScoreX(int i_indexTopScore) => i_Top10Scores[i_indexTopScore];
    public int GetNumberTopScore() => i_Top10Scores.Count;
    public int GetNbObolesPreviously() => i_PreviousNbTotalOboles;
    public int GetNbObolesTotal() => i_NbTotalOboles;
    /*****************************************************************************************************************************/

    void Start()
    {
        LoadDataAllGame();
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
            i_Top10Scores = data.i_Top10Scores;
        }
    }

    // Method to update the List Score with the new Score added by the previous game (Method to trigger when back to Menu after a run)
    public void UpdateDataAllGames()
    {
        Score newScoreToAdd = new()
        {
            score = GameInfo.instance.GetScore(),
            distanceCovered = GameInfo.instance.GetAllDistanceCovered()
        };

        // Loop to add the new score on the list and push the last item out of the list
        for (int i = 0; i < i_Top10Scores.Count; i++)
        {
            if (newScoreToAdd.score > i_Top10Scores[i].score)
            {
                i_Top10Scores.Insert(i, newScoreToAdd);
                break;
            }
        }

        // Condition to add the newScore when the leaderboard is not full and the loop before was not able to do it
        if (!i_Top10Scores.Contains(newScoreToAdd) && i_Top10Scores.Count < 10)
        {
            i_Top10Scores.Add(newScoreToAdd);
        }

        // Condition to remove the top score that is now 11
        if (i_Top10Scores.Count > 10)
        {
            i_Top10Scores.RemoveAt(i_Top10Scores.Count - 1);
        }

        // Update the new HighScore
        if (i_HighScore < newScoreToAdd.score)
            i_HighScore = newScoreToAdd.score;

        // Variable modification regarding the Oboles 
        i_PreviousNbTotalOboles = i_NbTotalOboles;
        i_NbTotalOboles += GameInfo.instance.GetCurrentOboles();

        WriteDataAllGame();
    }

    public void WriteDataAllGame()
    {
        SaveDataAllGame data = new();

        // Variables that need to be stored
        data.i_HighScore = i_HighScore;
        data.i_NbTotalOboles = i_NbTotalOboles;
        data.i_Top10Scores = i_Top10Scores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefileAllGame.json", json);
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }



}
