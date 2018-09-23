using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f; // rcs = reaction control system
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource rocketSound;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        RocketThrust();
        RocketRotate();
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Hit Friendly");
                break;
            default:
                print("You're dead!");
                break;
        }
    }

    private void RocketThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float mainThrustPerFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * mainThrustPerFrame);
            if (!rocketSound.isPlaying)
            {
                rocketSound.Play();
            }
        }
        else
        {
            rocketSound.Stop();
        }
    }

    // private means it can only be called by the code not outside
    private void RocketRotate()
    {
        rigidBody.freezeRotation = true; // manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = true; // resume physics control of rotation
    }

} // end of class
