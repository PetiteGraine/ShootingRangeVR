using System.Collections.Generic;
using UnityEngine;

public class FractureEffect : MonoBehaviour
{
    [SerializeField] private List<GameObject> _fractureCells;
    private List<Vector3> _initialPositions;
    private List<Rigidbody> _cellRb;

    private void Start()
    {
        _initialPositions = new List<Vector3>();
        _cellRb = new List<Rigidbody>();

        foreach (var cell in _fractureCells)
        {
            if (cell == null) continue;

            Rigidbody rb = cell.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _initialPositions.Add(cell.transform.localPosition);
                _cellRb.Add(rb);
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < _fractureCells.Count; i++)
        {
            Rigidbody cellRb = _cellRb[i];
            cellRb.linearVelocity = Vector3.zero;
            cellRb.angularVelocity = Vector3.zero;

            _fractureCells[i].transform.localPosition = _initialPositions[i];

            cellRb.Sleep();
        }
    }

    public void BreakObject()
    {
        foreach (Rigidbody cellRb in GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (cellRb.transform.position - transform.position).normalized
                            * Random.Range(2f, 5f);

            cellRb.AddForce(force, ForceMode.Impulse);
        }
    }
}