using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnPlay : MonoBehaviour
{
    [SerializeField] GameObject imgTransition;

    public void OnClickPlay()
    {
        imgTransition.GetComponent<FadingTransitionMenu>().TriggerTransition( () => SceneManager.LoadScene("SceneGame"));
    }
}
