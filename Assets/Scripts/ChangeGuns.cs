using UnityEngine;

public class ChangeGuns : MonoBehaviour
{
    public void ReplaceGun(GameObject gunPrefab)
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        Gun[] guns = GameController.GetComponentsInChildren<Gun>();
        if (GameController == null || guns.Length == 0 || gunPrefab == null) return;
        guns[0]?.ChangeGun(gunPrefab);
        guns[1]?.ChangeGun(gunPrefab);
    }
}
