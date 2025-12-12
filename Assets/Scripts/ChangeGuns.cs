using UnityEngine;

public class ChangeGuns : MonoBehaviour
{
    public void ReplaceGun(GameObject gunPrefab)
    {
        Gun[] guns = GameplayManager.Instance.PlayerGuns.ToArray();
        if (guns == null || guns.Length == 0 || gunPrefab == null) return;

        foreach (Gun gun in guns)
        {
            gun?.ChangeGun(gunPrefab);
        }
    }
}
