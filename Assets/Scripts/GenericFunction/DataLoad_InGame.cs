using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoad_InGame : MonoBehaviour
{
    public static DataLoad_InGame instance;

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
    [SerializeField] private List<int> i_Top10Scores = new();


    /*********************************************** ENCAPSULATION FUNCTION ******************************************************/

    public int GetHighScore() => i_HighScore;
    public int GetTopScoreX(int i_indexTopScore) => i_Top10Scores[i_indexTopScore];
    public int GetNumberTopScore() => i_Top10Scores.Count;

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
            i_Top10Scores = data.i_Top10Scores;
        }
    }

    public void WriteDataCurrentGame()
    {
        SaveDataLastGame data = new();

        // Variables that need to be stored
        data.i_Score = GameInfo.instance.GetScore();
        data.i_NbOboles = GameInfo.instance.GetCurrentOboles();

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefileLastGame.json", json);
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }
}
