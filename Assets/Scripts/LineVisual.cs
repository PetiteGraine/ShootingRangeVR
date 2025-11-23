using UnityEngine;

public class LineVisual : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private float _lineLength = 0.5f;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Transform grandParent = transform.parent?.parent;

        if (grandParent == null || !grandParent.CompareTag("GunController"))
            return;

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.localPosition);
        _lineRenderer.SetPosition(1, transform.localPosition + new Vector3(0f, 0f, _lineLength));
    }
}
