using UnityEngine;

public class DroneNavigation : MonoBehaviour
{
    [SerializeField] private Transform _followAnchor;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private int _numDirections = 32;
    [SerializeField] private float _coneAngle = 60f;
    [SerializeField] private float _lookAheadBase = 2.0f;
    [SerializeField] private float _clearanceRadius = 0.35f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _wFollow = 1.0f;
    [SerializeField] private float _wDyn = 0.5f;
    [SerializeField] private float _wLoS = 2.0f;

    private Vector3 _currentVelocity;
    private Vector3 _lastDirection;
    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;

    public float CurrentSpeed => _currentVelocity.magnitude;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _lastDirection = transform.forward;
    }

    private void Update()
    {
        Vector3 bestDir = CalculateBestDirection();
        bool isCautious = bestDir == Vector3.zero;

        float damping = isCautious ? 1f : 2f;
        _currentVelocity = Vector3.Lerp(_currentVelocity, bestDir * _maxSpeed, Time.deltaTime * damping);
        transform.position += _currentVelocity * Time.deltaTime;

        if (_currentVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_currentVelocity), Time.deltaTime * 5f);
        }

        UpdateShaderVisuals(isCautious);
    }

    private Vector3 CalculateBestDirection()
    {
        Vector3 targetDir = (_followAnchor.position - transform.position).normalized;
        float bestScore = float.MaxValue;
        Vector3 chosenDir = Vector3.zero;

        for (int i = 0; i < _numDirections; i++)
        {
            Vector3 candidateDir = i == 0 ? targetDir : Quaternion.Euler(Random.Range(-_coneAngle, _coneAngle), Random.Range(-_coneAngle, _coneAngle), 0) * targetDir;
            float l = _lookAheadBase + (_currentVelocity.magnitude * 0.5f);

            if (!Physics.SphereCast(transform.position, _clearanceRadius, candidateDir, out _, l, _obstacleMask))
            {
                float scoreFollow = Vector3.Angle(candidateDir, targetDir);
                float scoreDyn = Vector3.Angle(candidateDir, _lastDirection);
                float scoreLoS = Physics.Linecast(transform.position, _followAnchor.position, _obstacleMask) ? 100f : 0f;

                float totalScore = (scoreFollow * _wFollow) + (scoreDyn * _wDyn) + (scoreLoS * _wLoS);

                if (totalScore < bestScore)
                {
                    bestScore = totalScore;
                    chosenDir = candidateDir;
                }
            }
        }

        if (chosenDir != Vector3.zero) _lastDirection = chosenDir;
        return chosenDir;
    }

    private void UpdateShaderVisuals(bool isCautious)
    {
        if (_renderer == null) return;
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_RiskMode", isCautious ? 1f : 0f);
        _renderer.SetPropertyBlock(_propBlock);
    }
}