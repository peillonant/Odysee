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
    [SerializeField] private List<int> i_Top10Scores = new();

    /*********************************************** ENCAPSULATION FUNCTION ******************************************************/

    public int GetHighScore() => i_HighScore;
    public void SetHighScore(int i_newHighScore) => i_HighScore = i_newHighScore;
    public int GetTopScoreX(int i_indexTopScore) => i_Top10Scores[i_indexTopScore];
    public int GetNumberTopScore() => i_Top10Scores.Count;

    // Function that Retrieve the index of the Score if the score is on the List else return -1
    public int GetIndexTopScore(int i_ScoreToCheck) => i_Top10Scores.IndexOf(i_ScoreToCheck);

    // Function to add the new score on the list and push the last item out of the list
    public void SetTopScoreX(int i_addTopScore)
    {
        for (int i = 0; i < i_Top10Scores.Count; i++)
        {
            if (i_addTopScore > i_Top10Scores[i])
            {
                i_Top10Scores.Insert(i, i_addTopScore);
                break;
            }
        }

        if (!i_Top10Scores.Contains(i_addTopScore) && i_Top10Scores.Count < 10)
            i_Top10Scores.Add(i_addTopScore);

        if (i_Top10Scores.Count > 10)
            i_Top10Scores.RemoveAt(i_Top10Scores.Count - 1);
    }

    /*****************************************************************************************************************************/

    [System.Serializable]
    class SaveData
    {
        // Variable that need to be stored
        public int i_HighScore;
        public int i_NbTotalOboles;
        public List<int> i_Top10Scores;
    }

    public void WriteData()
    {
        SaveData data = new SaveData();

        // Variables that need to be stored
        data.i_HighScore = i_HighScore;
        data.i_NbTotalOboles = i_NbTotalOboles;
        data.i_Top10Scores = i_Top10Scores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Variable that need to be stored
            i_HighScore = data.i_HighScore;
            i_NbTotalOboles = data.i_NbTotalOboles;
            i_Top10Scores = data.i_Top10Scores;
        }
    }


    public void PushData()
    {
        if (GameInfo.GetScore() > i_HighScore)
            i_HighScore = GameInfo.GetScore();

        WriteData();
    }
}
