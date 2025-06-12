using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        GameInfo.instance.TriggerOnPause += UpdatePauseMenu;

       UpdateText();
    }

    private void UpdatePauseMenu()
    {
        // Be sure to have the Default view when press on Escape
        SetViewByDefault();

        UpdateText();
    }


    private void UpdateText()
    {
        // First we update the text of the score
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Your score: " + GameInfo.instance.GetScore());

        // Then we update the text of the current oboles retrieved
        this.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(GameInfo.instance.GetCurrentOboles().ToString());

        // Then we update the text of the current distance covered
        this.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("Distance Covered: " + GameInfo.instance.GetAllDistanceCovered());

        // Then we update the text of the reward retrieved
        this.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("Rewards Collected: " + GameInfo.instance.GetRewardList().Count);
    }

    // Method that put the ConfirmationMessage to hide and put back BtnResume and BtnMenu to interactable
    public void SetViewByDefault()
    {
        this.transform.GetChild(5).GetComponent<Button>().interactable = true;
        this.transform.GetChild(6).GetComponent<Button>().interactable = true;
        this.transform.GetChild(7).gameObject.SetActive(false);
    }

    public void ClickOnResume()
    {
        GameInfo.instance.TriggerPauseMenu();
    }

    public void ClickOnMenu()
    {
        this.transform.GetChild(5).GetComponent<Button>().interactable = false;
        this.transform.GetChild(6).GetComponent<Button>().interactable = false;
        this.transform.GetChild(7).gameObject.SetActive(true);
    }

    // TO DO : avoid to save data when back to Menu
    public void ClickOnMenu_Confirmed()
    {
        SceneManager.LoadScene("SceneMenu");
    }
}