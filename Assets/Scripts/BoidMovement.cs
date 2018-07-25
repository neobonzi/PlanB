 using System.Collections;
using System.Collections.Generic;
using Assets.Utilities;
using UnityEngine;

public class BoidMovement : MonoBehaviour {

    public float moveX;
    public float moveY;
    public float centerOfMassRotationSpeed = .5f;
    public float targetRotationSpeed = .6f;
    public float avoidanceRotationSpeed = .1f;
    public float speed = 200.0f;
    public float neighborhoodRadius = 2f;

    private Rigidbody2D rigidBody;

    // Use this for initialization
    private void Awake()
    {

    }

    void Start () {
        rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveBoid();
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, neighborhoodRadius);
    }

    // Given a current position and point to turn to calculate the angle that will get you there.
    // Up is a Unity vector 3 with a 1 in the position that the game world considers *up*
    // Away parameter indicates whether you want the angle to be away from target (true) or towards (false)
    public static Quaternion calculateAngleTowardsPoint(
        Vector3 currentPosition,
        Vector3 point, 
        Vector3 up, 
        bool away = false)
    {
        Vector3 vectorToTarget = (currentPosition - point).normalized;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle += away ? 180 : 0;
        return Quaternion.AngleAxis(angle, up);
    }

    //Calculate the rotation towards center of mass of all boids
    Quaternion calculateAngleTowardsCenterOfMass()
    {
        return calculateAngleTowardsPoint(transform.position, BoidManager.Instance.centerOfMass, Vector3.forward);
    }

    /// <summary>
    /// Finds all the boids close to the current boid and returns their distance
    /// and direction to get away
    /// </summary>
    /// <returns></returns>
    private List<Tuple<Quaternion, float>> calculateAnglesAwayFromOtherBoids()
    {
        List<Tuple<Quaternion, float>> closestBoids = new List<Tuple<Quaternion, float>>();

        // TODO: Circle might not be the best - we might want to pick people in our vision path
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), neighborhoodRadius);

        // Iterate through each boid in our neighborhood and turn away from them
        for (int i = 0; i  < hitColliders.Length; i++)
        {
            Vector3 vectorToTarget = (transform.position - hitColliders[i].transform.position);
            Quaternion angleAwayFromTarget = calculateAngleTowardsPoint(
                transform.position,
                hitColliders[i].transform.position,
                Vector3.forward,
                true);

            closestBoids.Add(new Tuple<Quaternion, float>(angleAwayFromTarget, vectorToTarget.magnitude));
        }

        return closestBoids;
    }

    void MoveBoid()
    {
        Quaternion centerOfMass = calculateAngleTowardsCenterOfMass();
        List<Tuple<Quaternion, float>> anglesAwayFromOtherBoids = calculateAnglesAwayFromOtherBoids();
        Quaternion angleTowardsTarget = calculateAngleTowardsPoint(transform.position, BoidManager.Instance.mousePosition, Vector3.forward);

        // Interpolate all the angles to get a final angle we'd like to actually steer towards
        Quaternion finalAngle = transform.rotation;

        // Start with the angles away from other boids
        // The farthest we can be away from something is on opposite diagonals of the game screen
        float maxDistance = Mathf.Sqrt(Screen.width ^ 2 + Screen.height ^ 2);

        foreach (Tuple<Quaternion, float> angleAway in anglesAwayFromOtherBoids)
        {
            // The closer we are, the more we want to increase the interpolation factor
            //float interpolationFactor =  (1 / (angleAway.second / maxDistance)) / avoidanceRotationSpeed;

            // We can change the tangent so this interpolation gives preference to closer shit.
            // Todo: Factor in time.delta?
            finalAngle = Quaternion.Slerp(finalAngle, angleAway.first, .2f * Time.deltaTime);
        }

        // Next, interpolate with the center of mass
        finalAngle = Quaternion.Slerp(finalAngle, centerOfMass, centerOfMassRotationSpeed * Time.deltaTime);

        // Finally, interpolate with the target for the flock
        finalAngle = Quaternion.Slerp(finalAngle, angleTowardsTarget, targetRotationSpeed * Time.deltaTime);

        transform.rotation = finalAngle;

        // Give the boid a small push forward;
        //rigidBody.velocity = transform.right * -1.0f * speed;
        rigidBody.AddForce(transform.right * -1.0f * speed);
    }
}
