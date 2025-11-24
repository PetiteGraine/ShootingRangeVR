using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _numberOfTargets = 3;
    private Vector3 _maxSpawnPosition;
    private Vector3 _minSpawnPosition;

    private void Start()
    {
        InitSpawnArea();
        for (int i = 0; i < _numberOfTargets; i++)
        {
            SpawnTarget();
        }
    }

    private void InitSpawnArea()
    {
        _maxSpawnPosition = new Vector3(this.transform.position.x + this.transform.localScale.x / 2,
                                     this.transform.position.y + this.transform.localScale.y / 2,
                                     this.transform.position.z + this.transform.localScale.z / 2);
        _minSpawnPosition = new Vector3(this.transform.position.x - this.transform.localScale.x / 2,
                                     this.transform.position.y - this.transform.localScale.y / 2,
                                     this.transform.position.z - this.transform.localScale.z / 2);

    }

    public void SpawnTarget(bool initingSpawner = false)
    {
        if (initingSpawner)
            InitSpawnArea();
        Vector3 spawnPosition = new Vector3(Random.Range(_minSpawnPosition.x, _maxSpawnPosition.x),
                                             Random.Range(_minSpawnPosition.y, _maxSpawnPosition.y),
                                             Random.Range(_minSpawnPosition.z, _maxSpawnPosition.z));
        Instantiate(_targetPrefab, spawnPosition, Quaternion.Euler(0, -90, 0));
    }
}
