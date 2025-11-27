using UnityEngine;

public class ChangeGuns : MonoBehaviour
{
    public void ReplaceGun(GameObject gunPrefab)
    {
        GameObject[] GunsController = GameObject.FindGameObjectsWithTag("GunController");
        if (GunsController == null || GunsController.Length == 0 || gunPrefab == null) return;
        GunsController[0].GetComponentInChildren<Gun>()?.ChangeGun(gunPrefab);
        GunsController[1].GetComponentInChildren<Gun>()?.ChangeGun(gunPrefab);
    }
}
