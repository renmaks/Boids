using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<Boid> _boids;
    [SerializeField] private GameObject _boidPrefab;
    [SerializeField] private Transform _boidAnchor;
    private int _numBoids = 100;
    private float _spawnDelay = 0.1f;

    public static float SPAWN_RADIUS { get; private set; } = 100f;
    

    private void Awake()
    {
        _boids = new List<Boid>();
    }

    private void Start()
    {
        InstantiateBoid();
    }

    private void InstantiateBoid()
    {
        GameObject boid = Instantiate(_boidPrefab);
        Boid b = boid.GetComponent<Boid>();
        b.transform.SetParent(_boidAnchor);
        _boids.Add(b);
        if(_boids.Count < _numBoids )
        {
            Invoke("InstantiateBoid", _spawnDelay);
        }
    }
}
