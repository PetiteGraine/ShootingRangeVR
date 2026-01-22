using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverDarknessManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Cible")]
    public GameObject targetObject;

    [Header("Réglages Shader")]
    public string shaderPropertyName = "_Darkness";
    public float darkValue = 0.8f;
    public float transitionSpeed = 8f;

    private MaterialPropertyBlock _propBlock;
    private float _currentValue = 0f;
    private float _targetValue = 0f;
    private Renderer[] _renderers;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        if (targetObject != null)
        {
            _renderers = targetObject.GetComponentsInChildren<Renderer>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetValue = darkValue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetValue = 0f;
    }

    void Update()
    {
        if (targetObject == null || _renderers == null) return;

        if (!Mathf.Approximately(_currentValue, _targetValue))
        {
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, Time.deltaTime * transitionSpeed);
            ApplyValue(_currentValue);
        }
    }

    private void ApplyValue(float val)
    {
        foreach (Renderer r in _renderers)
        {
            if (r == null) continue;

            r.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(shaderPropertyName, val);
            r.SetPropertyBlock(_propBlock);
        }
    }
}