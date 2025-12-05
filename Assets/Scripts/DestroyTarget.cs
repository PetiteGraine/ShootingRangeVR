using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [SerializeField] private float _delayTime = 0.25f;
    [SerializeField] private GameObject _spawner;
    [SerializeField] private GameObject _hitEffect;

    private void Start()
    {
        ChangeRandomPosition();
    }

    public void DestroyGameObject()
    {
        _hitEffect.SetActive(true);
        _hitEffect.GetComponent<FractureEffect>().BreakObject();
        this.gameObject.SetActive(false);
        Invoke(nameof(ChangeRandomPosition), _delayTime);
    }

    private void ChangeRandomPosition()
    {
        this.gameObject.SetActive(true);
        _hitEffect.SetActive(false);
        Vector3 newPosition = _spawner.GetComponent<Spawner>().RandomPosition();
        this.transform.parent.position = newPosition;
    }
}
