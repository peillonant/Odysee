using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject go_DisplayLife;
    [SerializeField] private GameObject go_DisplaySpeed;
    [SerializeField] private GameObject go_DisplayScore;
    [SerializeField] private GameObject go_DisplayMunition;
    [SerializeField] private GameObject go_DisplayOboles;
    [SerializeField] private GameObject go_DisplayDistance;


    void FixedUpdate()
    {
        UpdateDisplayLife();
        UpdateDisplaySpeed();
        UpdateDisplayScore();
        UpdateDisplayDistance();
        UpdateDisplayMunition();
        UpdateDisplayOboles();
    }

    void UpdateDisplayLife()
    {
        string s_textToDisplay = "Life: " + GameInfo.GetCurrentHealth();
        go_DisplayLife.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

    void UpdateDisplaySpeed()
    {
        string s_textToDisplay = "Speed: " + Mathf.Ceil(GameInfo.GetCurrentSpeed());
        go_DisplaySpeed.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

    void UpdateDisplayScore()
    {
        string s_textToDisplay = "Score: " + GameInfo.GetScore();
        go_DisplayScore.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

    void UpdateDisplayDistance()
    {
        string s_textToDisplay = "Distance: " + GameInfo.GetDistance();
        go_DisplayDistance.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

    void UpdateDisplayMunition()
    {
        string s_textToDisplay = "Munition: " + GameInfo.GetNbAmmo();
        go_DisplayMunition.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

    void UpdateDisplayOboles()
    {
        string s_textToDisplay = "Oboles: " + GameInfo.GetCurrentOboles();
        go_DisplayOboles.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
    }

}
