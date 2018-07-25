using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    public int boidCount = 5;
    public Transform boidPrefab;
    public Vector3 centerOfMass = Vector3.zero;
    public Vector3 mousePosition = Vector3.zero;
    public Transform player;
    private static BoidManager instance;
    public static BoidManager Instance { get { return instance; } }


    private const int gridWidth = 18;
    private const int gridHeight = 10;
    private List<Transform> boids = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    // Use this for initialization
    void Start () {
        createBoids();
        updateStats();
	}


    private void createBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Quaternion randomRotation = Random.rotation;
            randomRotation.x = 0;
            randomRotation.y = 0;
            Vector3 randomPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 50));
            Transform boid = Instantiate(boidPrefab, randomPosition, randomRotation);
            boids.Add(boid);
        }
    }

    void updateStats()
    {
        calculateCenterOfMass();
        calculateMousePosition();
    }

    void calculateMousePosition()
    {
        mousePosition = player.position;
    }

    void calculateCenterOfMass()
    {
        // center of mass
        Vector3 averagePosition = Vector3.zero;
        foreach (Transform boid in boids)
        {
            averagePosition += boid.position;
        }

        averagePosition /= boids.Count;
        centerOfMass = averagePosition;
        centerOfMass.z = 0;

        centerOfMass.x = Mathf.Min(centerOfMass.x, 8);
        centerOfMass.x = Mathf.Max(centerOfMass.x, -8);
        centerOfMass.y = Mathf.Min(centerOfMass.y, 4);
        centerOfMass.y = Mathf.Max(centerOfMass.y, -4);
    }
	
	// Update is called once per frame
	void Update () {
        updateStats();
	}
}
