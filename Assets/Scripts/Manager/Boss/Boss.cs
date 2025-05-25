using UnityEngine;

public class Boss : MonoBehaviour
{
    protected int i_BossHeath = 3;

    protected bool b_IsPreFight = false;

    public void SetIsPreFight(bool newState) => b_IsPreFight = newState;
    public bool IsPreFight() => b_IsPreFight;

    public void IncreaseHealth() => i_BossHeath++;

}