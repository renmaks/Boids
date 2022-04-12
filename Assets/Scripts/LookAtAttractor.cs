using UnityEngine;

public class LookAtAttractor : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Attractor.POS);
    }
}
