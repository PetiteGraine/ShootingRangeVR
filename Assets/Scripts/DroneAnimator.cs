using UnityEngine;

public class DroneAnimator : MonoBehaviour
{
	[SerializeField] private float _propSpeedBase = 500f;
	[SerializeField] private float _bobSpeed = 2f;
	[SerializeField] private float _bobHeight = 0.2f;
	[SerializeField] private float _wobble = 5f;
	[SerializeField] private float _wobbleSpeed = 1.5f;

	private Transform[] _props;
	private Transform _trans;
	private DroneNavigation _nav;

	private void Start()
	{
		_trans = transform;
		_nav = GetComponent<DroneNavigation>();
		_props = new Transform[4];

		int propCount = 0;
		foreach (Transform child in transform)
		{
			if (child.name.ToLower().Contains("motor") && propCount < 4)
			{
				_props[propCount] = child.GetChild(0);
				propCount++;
			}
		}
	}

	private void Update()
	{
		float speedFactor = _nav != null ? (_nav.CurrentSpeed / 5f) + 1f : 1f;

		foreach (Transform prop in _props)
		{
			if (prop != null)
			{
				prop.Rotate(0, 0, _propSpeedBase * speedFactor * Time.deltaTime);
			}
		}

		Vector3 pos = _trans.localPosition;
		pos.y = Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
		_trans.localPosition = pos;

		Vector3 rot = _trans.localEulerAngles;
		rot.x = 270 + Mathf.Sin(Time.time * _wobbleSpeed) * _wobble;
		rot.z = Mathf.Cos(Time.time * _wobbleSpeed) * _wobble;
		rot.y = 0;
		_trans.localEulerAngles = rot;
	}
}