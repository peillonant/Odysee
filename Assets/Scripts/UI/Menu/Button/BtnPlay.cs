using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnPlay : MonoBehaviour
{
    [SerializeField] GameObject imgTransition;

    public void OnClickPlay()
    {
        DataLoad_Menu.instance.WriteDataAllGame();
        DataLoad_Menu.instance.WriteResetDataLastGame();
        imgTransition.GetComponent<FadingTransitionMenu>().TriggerTransition( () => SceneManager.LoadScene("SceneGame"));
    }
}
