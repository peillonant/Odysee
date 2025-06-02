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
        GameObject go_MenuTable = this.transform.GetChild(1).gameObject;

        int i_NumberOfTopScore = DataLoad_Menu.instance.GetNumberTopScore();

        // Loop to update the list of 10 top score
        for (int i = 0; i < i_NumberOfTopScore; i++)
        {
            int i_tmp = i + 1;
            go_MenuTable.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText("Top " + i_tmp + ": " + DataLoad_Menu.instance.GetTopScoreX(i));
        }

        // Update the NbOboles
        this.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Total Oboles\n" + DataLoad_Menu.instance.GetNbObolesTotal());
    }
}