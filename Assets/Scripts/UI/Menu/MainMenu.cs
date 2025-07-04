using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_TextOboles;
    public int i_NbObolesStart = 0;
    public int i_NbObolesTarget = 0;
    public bool b_ObolesToAdd = false;
    private float f_Timer = 0;
    private float f_Delay = 2;

    void Start()
    {
        UpdateMainMenu();    
    }

    void Update()
    {
        if (b_ObolesToAdd)
        {
            int i_ObolesScoreDisplay = (int)Mathf.Ceil(Tweening.Lerp(ref f_Timer, f_Delay, i_NbObolesStart, i_NbObolesTarget));

            // Update the NbOboles text
            go_TextOboles.GetComponentInChildren<TextMeshProUGUI>().SetText(i_ObolesScoreDisplay.ToString());

            if (i_ObolesScoreDisplay == i_NbObolesTarget)
                b_ObolesToAdd = false;
        }
    }

    private void UpdateMainMenu()
    {

        i_NbObolesStart = DataPersistence.instance.GetNbObolesPreviously();
        i_NbObolesTarget = DataPersistence.instance.GetNbObolesTotal();

        b_ObolesToAdd = true;

        // Update the NbOboles text
        go_TextOboles.GetComponentInChildren<TextMeshProUGUI>().SetText(i_NbObolesStart.ToString());
    }
}