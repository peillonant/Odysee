using System;
using UnityEngine;
using UnityEngine.UI;

public class FadingTransitionMenu : MonoBehaviour
{
    bool b_TransitionHasBeenTriggerd;
    private float f_currentAlpha;
    private float f_targetAlpha;
    private Action action;

    void Start()
    {
        if (GetComponent<Image>().color.a == 0)
        {
            f_currentAlpha = 0;
            f_targetAlpha = 1;
        }
        else
        {
            f_currentAlpha = 1;
            f_targetAlpha = 0;
            TriggerTransition(() =>
            {
                this.transform.parent.GetChild(1).gameObject.SetActive(true);
                Destroy(this.gameObject);
            });
        }
    }

    public void TriggerTransition(Action action)
    {
        this.action = action;
        this.gameObject.SetActive(true);
        b_TransitionHasBeenTriggerd = true;
    }

    void Update()
    {
        if (b_TransitionHasBeenTriggerd)
        {
            f_currentAlpha = f_currentAlpha < f_targetAlpha ? f_currentAlpha + Time.deltaTime : f_currentAlpha - Time.deltaTime;

            GetComponent<Image>().color = new Color(0, 0, 0, f_currentAlpha);

            if (f_currentAlpha > f_targetAlpha && f_currentAlpha < 0.05f)
            {
                f_currentAlpha = 1;
                b_TransitionHasBeenTriggerd = false;
                action.Invoke();
            }
            else if (f_currentAlpha < f_targetAlpha && f_currentAlpha > 0.95f)
            {
                f_currentAlpha = 1;
                b_TransitionHasBeenTriggerd = false;
                action.Invoke();
            }
        }
    }
}