using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostMenu : MonoBehaviour
{
    public void UpdateLostMenu()
    {
        // First we update the text of the score
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Your score: " + GameInfo.instance.GetScore());

        // Then we update the text of the oboles retrieved during the run
        this.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(GameInfo.instance.GetCurrentOboles().ToString());

        // And we update the distance covered during the run
        this.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("Disctance covered: " + GameInfo.instance.GetAllDistanceCovered());

        // Condition to know if the current score is the new highScore or not
        if (GameInfo.instance.GetScore() > DataPersistence.instance.GetHighScore())
            this.transform.GetChild(4).gameObject.SetActive(true);
    }

    // Not able to trigger the function when clicking on the button   
    public void OnBtnMenu()
    {
        DataPersistence.instance.UpdateDataAllGames();
        SceneManager.LoadScene("SceneMenu");
    }
}