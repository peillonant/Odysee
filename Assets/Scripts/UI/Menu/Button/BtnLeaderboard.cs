using UnityEngine;

public class BtnLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject go_CanvasMenu;
    [SerializeField] GameObject go_CanvasLeaderboard;

    public void OnClickLeaderBoard()
    {
        go_CanvasMenu.SetActive(false);
        go_CanvasLeaderboard.SetActive(true);
    }

    public void OnClickBack()
    {
        go_CanvasLeaderboard.SetActive(false);
        go_CanvasMenu.SetActive(true);
    }
}
