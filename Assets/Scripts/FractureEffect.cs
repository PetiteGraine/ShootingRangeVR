using System.Collections.Generic;
using UnityEngine;

public class FractureEffect : MonoBehaviour
{
    [SerializeField] private List<GameObject> _fractureCells;
    private List<Vector3> _initialPositions;

    private void Start()
    {
        _initialPositions = new List<Vector3>();
        foreach (var cell in _fractureCells)
        {
            _initialPositions.Add(cell.transform.localPosition);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < _fractureCells.Count; i++)
        {
            _fractureCells[i].transform.localPosition = _initialPositions[i];
        }
    }

    public void BreakObject()
    {
        foreach (Rigidbody cellRb in GetComponentsInChildren<Rigidbody>())
        {
            cellRb.isKinematic = false;
            cellRb.WakeUp();

            Vector3 force = (cellRb.transform.position - transform.position).normalized
                            * Random.Range(2f, 5f);

            cellRb.AddForce(force, ForceMode.Impulse);
        }
    }
}
