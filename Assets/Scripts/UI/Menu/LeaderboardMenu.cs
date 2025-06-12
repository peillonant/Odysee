using TMPro;
using UnityEngine;

public class LeaderBoardMenu : MonoBehaviour
{
    void Start()
    {
        UpdateLeaderBoardMenu();
    }

    private void UpdateLeaderBoardMenu()
    {
        GameObject go_MenuTable = this.transform.GetChild(2).gameObject;

        int i_NumberOfTopScore = DataPersistence.instance.GetNumberTopScore();

        // Loop to update the list of 10 top score
        for (int i = 0; i < i_NumberOfTopScore; i++)
        {
            string textScore = "Top " + (i + 1) + ": " + DataPersistence.instance.GetTopScoreX(i).score + " ( " + DataPersistence.instance.GetTopScoreX(i).distanceCovered + " )";
            go_MenuTable.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText(textScore);
        }

        // Update the NbOboles
        this.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(DataPersistence.instance.GetNbObolesTotal().ToString());
    }
}