using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostMenu : MonoBehaviour
{
    public void UpdateLostMenu()
    {
        // First we update the text of the score
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Your score: " + GameInfo.GetScore());

        // Then we update the text of the oboles retrieved during the run
        this.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Oboles collected: " + GameInfo.GetCurrentOboles());

        // Condition to know if the current score is the new highScore or not
        if (GameInfo.GetScore() > DataLoad_InGame.instance.GetHighScore())
        {
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    // Not able to trigger the function when clicking on the button   
    public void OnBtnMenu()
    {
        DataLoad_InGame.instance.WriteDataCurrentGame();
        SceneManager.LoadScene("SceneMenu");
    }
}