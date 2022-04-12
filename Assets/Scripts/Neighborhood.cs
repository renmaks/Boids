using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    private List<Boid> _neighbors;
    private SphereCollider _collider;
    private float _neighborDistance = 30f;
    private float _colliderDistance = 4f;

    private void Awake()
    {
        _neighbors = new List<Boid>();        
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _neighborDistance / 2;
    }

    private void FixedUpdate()
    {
        if(_collider.radius != _neighborDistance/2)
        {
            _collider.radius = _neighborDistance / 2;
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if(b != null)
        {
            if(_neighbors.IndexOf(b) == -1)
            {
                _neighbors.Add(b);
            }
        }
    }

     private void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b!=null)
        {
            if(_neighbors.IndexOf(b) != -1)
            {
                _neighbors.Remove(b);
            }
        }
    }

    public Vector3 averagePosition
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if(_neighbors.Count == 0)
            {
                return avg;
            }
            for(int i = 0; i<_neighbors.Count; i++)
            {
                avg += _neighbors[i].pos;
            }
            avg /= _neighbors.Count;

            return avg;
        }
        private set { }
    }

    public Vector3 averageVelocity
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if(_neighbors.Count == 0)
            {
                return avg;
            }
            for(int i = 0; i<_neighbors.Count; i++)
            {
                avg += _neighbors[i].rigid.velocity;
            }
            avg /= _neighbors.Count;

            return avg;
        }
        private set { }
    }

    public Vector3 averageClosePosition
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for(int i = 0; i < _neighbors.Count;i++)
            {
                delta = _neighbors[i].pos - transform.position;
                if(delta.magnitude <= _colliderDistance)
                {
                    avg += _neighbors[i].pos;
                    nearCount++;
                }
            }
            // Если нет соседей, летящих слишком близко, вернуть Vector3.zero
            if (nearCount == 0)
            {
                return avg;
            }
            // Иначе координаты центральной точки
            avg /= nearCount;
            return avg;
        }
        private set { }
    }
}
