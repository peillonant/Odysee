using TMPro;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    private bool b_IsDisplayed = false;
    private float f_PosBottom = -5;
    private float f_PosTop = 15;

    private float f_TimerAnime = 0;
    private float f_DelayAnime = 2;
    

    private Color greenColor = new(0.4f, 0.88f, 0.47f);
    private Color redColor = new(0.8f, 0.3f, 0.25f);

    public void DisplayValue(int i_scoreToAdd)
    {
        GetComponent<TextMeshProUGUI>().SetText(i_scoreToAdd.ToString());

        if (i_scoreToAdd > 0)
            GetComponent<TextMeshProUGUI>().color = greenColor;
        else
            GetComponent<TextMeshProUGUI>().color = redColor;

        b_IsDisplayed = true;
    }

    void Update()
    {
        if (b_IsDisplayed)
        {

            f_TimerAnime += Time.deltaTime;
            float f_easedT = Tweening.InOutBack(f_TimerAnime / f_DelayAnime);

            // Interpolation between the top position and bottom
            Vector3 newPosition = this.transform.localPosition;
            newPosition.y = Mathf.Lerp(f_PosBottom, f_PosTop, f_easedT);
            newPosition.x = Mathf.Lerp(0, f_PosTop, f_TimerAnime / f_easedT);

            Color newAlpha = GetComponent<TextMeshProUGUI>().color;
            newAlpha.a = Mathf.Lerp(1, 0, f_easedT);

            GetComponent<TextMeshProUGUI>().color = newAlpha;

            this.transform.localPosition = newPosition;

            if (f_TimerAnime > f_DelayAnime)
                Destroy(gameObject);
        }   
    }
}   