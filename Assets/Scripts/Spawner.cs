using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Target Setup")]
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private GameObject _spawnerArea;
    [SerializeField] private float _spawnInterval = 1.0f;

    private List<GameObject> _spawnedTargets = new List<GameObject>();
    private Vector3 _maxSpawnPosition;
    private Vector3 _minSpawnPosition;
    private Coroutine _spawnLoop;

    public static Spawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            InitSpawnArea();
        }
    }

    private void InitSpawnArea()
    {
        if (_spawnerArea == null)
        {
            Debug.LogError("SpawnerArea is not assigned in Spawner.");
            return;
        }

        _maxSpawnPosition = _spawnerArea.transform.position + _spawnerArea.transform.localScale / 2f;
        _minSpawnPosition = _spawnerArea.transform.position - _spawnerArea.transform.localScale / 2f;
    }

    public void StartNewGame()
    {
        if (_spawnLoop != null)
        {
            StopCoroutine(_spawnLoop);
        }

        if (_spawnedTargets.Count > 0)
        {
            EnableAllTargetsAndReposition();
        }

        _spawnLoop = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitWhile(() => GameplayManager.Instance == null);

        while (true)
        {
            if (GameplayManager.Instance.CurrentTargets < GameplayManager.Instance.MaxTargets)
            {
                SpawnTarget();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnTarget()
    {
        GameObject targetToSpawn = null;

        foreach (var target in _spawnedTargets)
        {
            if (!target.activeInHierarchy)
            {
                targetToSpawn = target;
                break;
            }
        }

        if (targetToSpawn == null)
        {
            targetToSpawn = Instantiate(_targetPrefab, this.transform);
            _spawnedTargets.Add(targetToSpawn);
        }

        targetToSpawn.transform.position = RandomPosition();
        targetToSpawn.transform.rotation = Quaternion.Euler(0, -90, 0);
        targetToSpawn.SetActive(true);

        GameplayManager.Instance.RegisterTargetSpawn();
    }

    public Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(_minSpawnPosition.x, _maxSpawnPosition.x),
                           Random.Range(_minSpawnPosition.y, _maxSpawnPosition.y),
                           Random.Range(_minSpawnPosition.z, _maxSpawnPosition.z));
    }

    public void DisableAllTargets()
    {
        if (_spawnLoop != null)
        {
            StopCoroutine(_spawnLoop);
        }

        foreach (GameObject target in _spawnedTargets)
        {
            if (target.activeInHierarchy)
            {
                target.SetActive(false);
                GameplayManager.Instance.RegisterTargetDespawn();
            }
        }
    }

    public void EnableAllTargetsAndReposition()
    {
        foreach (GameObject target in _spawnedTargets)
        {
            target.transform.position = RandomPosition();
            target.SetActive(true);

            GameplayManager.Instance.RegisterTargetSpawn();
        }
    }
}