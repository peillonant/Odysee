using UnityEngine;

public class BtnExit : MonoBehaviour
{
    public void OnClickExit()
    {
        DataLoad_Menu.instance.WriteDataAllGame();
        Application.Quit();
    }
}