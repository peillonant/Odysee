using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private GameObject go_RewardPrefab;

    public void AddRewardOnSea(GameObject go_Sea)
    {
        // now we check if we add the Reward
        if (!GameInfo.instance.GetRewardList().Contains(GameInfo.instance.GetCurrentRegion().typeRegion))
        {
            // Check if an item is already instantiate on the zone, if so, we remove it
            Transform ItemZone = go_Sea.transform.GetChild(5).GetChild(0);

            // Check if an item is already instantiate on the zone, if so, we remove it
            if (ItemZone.childCount > 0)
                GetComponent<ItemManager>().RemoveItems(go_Sea);

            Instantiate(go_RewardPrefab, ItemZone);
            go_RewardPrefab.GetComponent<Renderer>().material = GameInfo.instance.GetCurrentRegion().rewardMaterial;
        }
    }
}