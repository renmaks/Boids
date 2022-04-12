using UnityEngine;

public class Attractor : MonoBehaviour
{
    static public Vector3 POS { get; private set; } = Vector3.zero;

    [Header("Set in Inspector")]
    [SerializeField] private float _radius = 10;
    [SerializeField] private float _xPhase = 0.5f;
    [SerializeField] private float _yPhase = 0.4f;
    [SerializeField] private float _zPhase = 0.1f;


// Fixed Update вызывается при каждом пересчете физики ( 50 раз в секунду )
    void FixedUpdate()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(_xPhase * Time.time) * _radius * scale.x;
        tPos.y = Mathf.Sin(_yPhase * Time.time) * _radius * scale.y;
        tPos.z = Mathf.Sin(_zPhase * Time.time) * _radius * scale.z;
        transform.position = tPos;
        POS = tPos;
    }
}
