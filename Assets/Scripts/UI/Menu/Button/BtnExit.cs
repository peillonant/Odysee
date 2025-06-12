using UnityEngine;

public class BtnExit : MonoBehaviour
{
    public void OnClickExit()
    {
        DataPersistence.instance.WriteDataAllGame();
        Application.Quit();
    }
}