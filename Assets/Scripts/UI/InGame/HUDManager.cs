using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public static HUDManager instance;

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

    // Variable linked to Life
    [SerializeField] private GameObject go_DisplayLife;
    private Color heartFull = new(0.75f, 0.08f, 0.08f);

    // Variable Linked to Speed
    [SerializeField] private GameObject go_DisplaySpeed;

    // Variable linked to Score
    [SerializeField] private GameObject go_DisplayScore;
    [SerializeField] private GameObject go_DisplayScoreToAdd;
    private Color bronzeColor = new(0.8f, 0.5f, 0.2f);
    private Color silverColor = new(0.75f, 0.75f, 0.75f);
    private Color goldColor = new(1, 0.84f, 0);

    // Variable linked to Multiplier
    [SerializeField] private GameObject go_DisplayMultiplier;
    private int i_mutliplierDisplay = 0;

    // Variable linked to Munition
    [SerializeField] private GameObject go_DisplayMunition;
    private bool b_IsOnCd = false;
    private float f_TimerCD = 0;

    // Variable linked to Oboles
    [SerializeField] private GameObject go_DisplayOboles;

    // Variable linked to Distance
    [SerializeField] private GameObject go_DisplayDistance;
    private float f_PosOrigin = 50;
    private float f_PostMax = 650;

    // Variable linked to Compass
    [SerializeField] private GameObject go_DisplayCompass;
    private Color colorGreen = new(0.13f, 0.6f, 0.13f);
    private Color colorRed = new(0.8f, 0.1f, 0.15f);

    // Variable linked to Boss Jauge
    [SerializeField] private BossManager go_Boss;
    [SerializeField] private GameObject go_BossJauge_Speed;
    [SerializeField] private GameObject go_BossJauge_Noise;
    private Color colorGreen_Noise = new(0.13f, 0.8f, 0.2f);
    private Color colorOrange_Noise = new(0.82f, 0.5f, 0.12f);
    private Color colorRed_Noise = new(0.44f, 0.13f, 0.13f);


    // Variable linked to the Tutorial
    [SerializeField] private GameObject go_Tutorial;


    void FixedUpdate()
    {
        if (!GameInfo.instance.IsGameLost() && !GameInfo.instance.IsGameOnPause())
        {
            UpdateDisplayLife();
            UpdateDisplaySpeed();
            UpdateDisplayScore();
            UpdateDisplayDistance();
            UpdateDisplayMunition();
            UpdateDisplayOboles();
            UpdateDisplayCompass();
            UpdateDisplayJaugeBoss();
        }
    }

    /* -------------------------------- */

    void UpdateDisplayLife()
    {
        for (int i = 0; i < go_DisplayLife.transform.childCount; i++)
        {
            if (i <= GameInfo.instance.GetCurrentHealth() - 1)
                go_DisplayLife.transform.GetChild(i).GetComponent<RawImage>().color = heartFull;
            else
                go_DisplayLife.transform.GetChild(i).GetComponent<RawImage>().color = Color.black;
        }
    }

    /* -------------------------------- */

    void UpdateDisplaySpeed()
    {
        go_DisplaySpeed.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(GameInfo.instance.GetCurrentSpeed()).ToString();
    }

    /* -------------------------------- */

    // Methods linked to the Score Display
    void UpdateDisplayScore()
    {
        if (GameInfo.instance.IsBossFight() && go_DisplayScore.activeSelf)
            go_DisplayScore.SetActive(false);
        else if (!GameInfo.instance.IsBossFight() && !go_DisplayScore.activeSelf)
            go_DisplayScore.SetActive(true);

        string s_textToDisplay = "Score: " + GameInfo.instance.GetScore();
        go_DisplayScore.GetComponent<TextMeshProUGUI>().text = s_textToDisplay;
        UpdateScoreMultiplier();
    }

    void UpdateScoreMultiplier()
    {
        int i_newMutliplier = GameInfo.instance.GetMultiplier();

        if (i_newMutliplier != i_mutliplierDisplay)
        {
            i_mutliplierDisplay = i_newMutliplier;
            TextMeshProUGUI textObject = go_DisplayMultiplier.GetComponent<TextMeshProUGUI>();

            string s_textToDisplay = "X " + i_mutliplierDisplay;
            textObject.text = s_textToDisplay;

            if (i_mutliplierDisplay == 1)
            {
                textObject.fontSize = 30;
                textObject.color = bronzeColor;
            }
            else if (i_mutliplierDisplay == 2)
            {
                textObject.fontSize = 35;
                textObject.color = silverColor;
            }
            else if (i_mutliplierDisplay == 3)
            {
                textObject.fontSize = 40;
                textObject.color = goldColor;
            }
        }
    }

    public void AddScoreDisplay(int i_scoreToAdd)    // Method call everytime we add or remove something to the score
    {
        GameObject go_newScoreToAdd = Instantiate(go_DisplayScoreToAdd, go_DisplayScore.transform.GetChild(1));
        go_newScoreToAdd.GetComponent<AddScore>().DisplayValue(i_scoreToAdd);
    }

    /* -------------------------------- */
    void UpdateDisplayDistance()
    {
        if (GameInfo.instance.IsBossFight() && go_DisplayDistance.activeSelf)
            go_DisplayDistance.SetActive(false);
        else if (!GameInfo.instance.IsBossFight() && !go_DisplayDistance.activeSelf)
            go_DisplayDistance.SetActive(true);

        // First we compute the ratio of the distance currently covered
        float ratioDistanceCovered = ((float)GameInfo.instance.GetDistance()) / 1000;

        Vector3 newPos = go_DisplayDistance.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        newPos.x = f_PosOrigin + ((f_PostMax - f_PosOrigin) * ratioDistanceCovered);

        go_DisplayDistance.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = newPos;
    }

    /* -------------------------------- */

    void UpdateDisplayMunition()
    {
        go_DisplayMunition.GetComponentInChildren<TextMeshProUGUI>().text = GameInfo.instance.GetNbAmmo().ToString();

        if (b_IsOnCd)
        {
            GameObject go_CD = go_DisplayMunition.transform.GetChild(2).gameObject;

            if (!go_CD.activeSelf)
                go_CD.SetActive(true);

            f_TimerCD += Time.deltaTime;

            go_CD.GetComponent<Image>().fillAmount = Mathf.Lerp(1, 0, f_TimerCD / GameConstante.F_CANNONCOLDDOWN);

            if (f_TimerCD > GameConstante.F_CANNONCOLDDOWN)
            {
                f_TimerCD = 0;
                go_CD.SetActive(false);
                go_CD.GetComponent<Image>().fillAmount = 1;
                b_IsOnCd = false;
            }
        }
    }

    public void TriggerColdDown() => b_IsOnCd = true;

    /* -------------------------------- */

    void UpdateDisplayOboles()
    {
        if (GameInfo.instance.IsBossFight() && go_DisplayOboles.activeSelf)
            go_DisplayOboles.SetActive(false);
        else if (!GameInfo.instance.IsBossFight() && !go_DisplayOboles.activeSelf)
            go_DisplayOboles.SetActive(true);

        go_DisplayOboles.GetComponentInChildren<TextMeshProUGUI>().text = GameInfo.instance.GetCurrentOboles().ToString();
    }

    /* -------------------------------- */

    void UpdateDisplayCompass()
    {
        Vector3 newAngleWind = go_DisplayCompass.transform.GetChild(1).localEulerAngles;
        Vector3 newAngleShip = go_DisplayCompass.transform.GetChild(2).localEulerAngles;

        // First we change the WindDirection & the BackGround
        go_DisplayCompass.transform.GetChild(1).localEulerAngles = new(newAngleWind.x, newAngleWind.y, -GameInfo.instance.GetWindAngle());

        // Second, we change the ShipDirection
        go_DisplayCompass.transform.GetChild(2).localEulerAngles = new(newAngleShip.x, newAngleShip.y, -GameInfo.instance.GetSailAngle());

        // Then, we change the Background and check regarding the color to apply
        go_DisplayCompass.transform.GetChild(0).localEulerAngles = new(newAngleWind.x, newAngleWind.y, -GameInfo.instance.GetWindAngle());

        if (GameInfo.instance.IsOnWind())
            go_DisplayCompass.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = colorGreen;
        else
            go_DisplayCompass.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = colorRed;
    }

    /* -------------------------------- */

    void UpdateDisplayJaugeBoss()
    {
        if (GameInfo.instance.GetCurrentRegion().typeRegion == TypeRegion.EGEE)
        {
            if (!GameInfo.instance.GetPreFightPerformed())
            {
                if (!go_BossJauge_Speed.activeSelf && !GameInfo.instance.IsBossFight())
                    go_BossJauge_Speed.SetActive(true);

                if (go_BossJauge_Noise.activeSelf)
                    go_BossJauge_Speed.SetActive(false);

                // First we increase the Speed Jauge
                float f_ratioSpeed = GameInfo.instance.GetCurrentSpeed() / GameInfo.instance.GetSpeedMax();
                Vector2 newSizeDelta = new Vector2(12.5f, 437.5f * f_ratioSpeed);
                go_BossJauge_Speed.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = newSizeDelta;

                // Second, we increase the Threshold Jauge
                float f_rationThreshold = GameInfo.instance.GetThresholdBoss() / GameInfo.instance.GetSpeedMax();
                newSizeDelta.y = 437.5f * f_rationThreshold;
                go_BossJauge_Speed.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = newSizeDelta;
            }
            else
            {
                if (go_BossJauge_Speed.activeSelf || GameInfo.instance.IsBossFight())
                    go_BossJauge_Speed.SetActive(false);

            }
        }
        else if (GameInfo.instance.GetCurrentRegion().typeRegion == TypeRegion.STYX || GameInfo.instance.GetCurrentRegion().typeRegion == TypeRegion.ARCADIA)
        {
            if (!GameInfo.instance.GetPreFightPerformed())
            {
                if (!go_BossJauge_Noise.activeSelf && !GameInfo.instance.IsBossFight())
                    go_BossJauge_Noise.SetActive(true);

                if (go_BossJauge_Speed.activeSelf)
                    go_BossJauge_Speed.SetActive(false);

                // First, we compute the size of the Jauge
                float f_ratioNoise_Size = go_Boss.GetCurrentNoise() / go_Boss.GetMaxNoise();
                Vector2 newSizeDelta = new Vector2(30f, 10 + (427.5f * f_ratioNoise_Size));
                go_BossJauge_Noise.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = newSizeDelta;

                // Now we change the color regarding the ratio
                float f_ratioNoise_Color = go_Boss.GetCurrentNoise() / go_Boss.GetNoiseThreshold();
                if (f_ratioNoise_Color < 0.5f)
                    go_BossJauge_Noise.transform.GetChild(0).GetComponent<Image>().color = colorGreen_Noise;
                else if (f_ratioNoise_Color < 0.75f)
                    go_BossJauge_Noise.transform.GetChild(0).GetComponent<Image>().color = colorOrange_Noise;
                else
                    go_BossJauge_Noise.transform.GetChild(0).GetComponent<Image>().color = colorRed_Noise;
            }
            else
            {
                if (go_BossJauge_Noise.activeSelf || GameInfo.instance.IsBossFight())
                    go_BossJauge_Noise.SetActive(false);

            }
        }
    }


    /* -------------------------------- */
    public void DisplayTutorial()
    {
        go_Tutorial.SetActive(!go_Tutorial.activeSelf);
    }
}
