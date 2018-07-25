using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rigidBody;
    public TouchPhase lastTouch = TouchPhase.Ended;
    public float horizontalSpeed = .03f;
    public float verticalSpeed = .03f;
	// Use this for initialization
	void Start () {
        rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            transform.position = Input.GetTouch(0).position;
        }
        else
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            transform.position = newPosition;
            Debug.Log("Position: " + transform.position);
        }

        //if(Input.acceleration.x > 0)
        //      {
        //          rigidBody.AddForce(Vector2.right * horizontalSpeed);
        //      }
        //      else if(Input.acceleration.x < 0)
        //      {
        //          rigidBody.AddForce(Vector2.left * horizontalSpeed);
        //      }

        //      if(Input.acceleration.y > 0)
        //      {
        //          rigidBody.AddForce(Vector2.up * verticalSpeed);
        //      }
        //      else if(Input.acceleration.y < 0)
        //      {
        //          rigidBody.AddForce(Vector2.down * verticalSpeed);
        //      }
    }
}
