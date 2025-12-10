using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _numberOfTargets = 0;
    [SerializeField] private GameObject _spawnerArea;
    private static Vector3 _maxSpawnPosition;
    private static Vector3 _minSpawnPosition;
    private static Spawner _instance;
    public static Spawner Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        InitSpawnArea();
    }

    private void Start()
    {
        for (int i = 0; i < _numberOfTargets; i++)
        {
            SpawnTarget();
        }
    }

    private void InitSpawnArea()
    {
        _maxSpawnPosition = new Vector3(_spawnerArea.transform.position.x + _spawnerArea.transform.localScale.x / 2,
                                     _spawnerArea.transform.position.y + _spawnerArea.transform.localScale.y / 2,
                                     _spawnerArea.transform.position.z + _spawnerArea.transform.localScale.z / 2);
        _minSpawnPosition = new Vector3(_spawnerArea.transform.position.x - _spawnerArea.transform.localScale.x / 2,
                                     _spawnerArea.transform.position.y - _spawnerArea.transform.localScale.y / 2,
                                     _spawnerArea.transform.position.z - _spawnerArea.transform.localScale.z / 2);
    }

    public void SpawnTarget()
    {
        Instantiate(_targetPrefab, RandomPosition(), Quaternion.Euler(0, -90, 0), this.transform);
    }

    public Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(_minSpawnPosition.x, _maxSpawnPosition.x),
                           Random.Range(_minSpawnPosition.y, _maxSpawnPosition.y),
                           Random.Range(_minSpawnPosition.z, _maxSpawnPosition.z));
    }
}
