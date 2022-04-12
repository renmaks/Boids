using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Этот класс реализует шаблон проектирования "Одиночка". Существовать может только один экземпляр Spawner, поэтому сохраним его в статической переменной S.
    static public Spawner S;
    static public List<Boid> boids;

    // Эти поля позволяют настраивать порядок создания объектов Boid
    [Header("Set in Inspector: Spawning")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;

    // Эти поля позволяют настраивать стайное поведение объектов Boid
    [Header("Set in Inspector: Boids")]
    public float velocity = 30f;
    public float neighborDist = 30f;
    public float collDist = 4f;
    public float velMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f;
    public float attractPush = 2f;
    public float attractPushDist = 5f;


     void Awake()
    {
        // Сохранить этот экземпляр Spawner в S
        S = this;
        // Запустить создание объектов Boid
        boids = new List<Boid>();
        InstantiateBoid();
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        boids.Add(b);
        if(boids.Count < numBoids )
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }
}
