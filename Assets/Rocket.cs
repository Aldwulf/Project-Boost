using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f; // rcs = reaction control system
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    Rigidbody rigidBody;
    AudioSource rocketSound;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RocketThrust();
            RocketRotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Hit Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        rocketSound.Stop();
        rocketSound.PlayOneShot(winSound);
        Invoke("LoadNextLevel", 1f);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        rocketSound.Stop();
        rocketSound.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RocketThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            rocketSound.Stop();
        }
    }

    private void ApplyThrust()
    {
        float mainThrustPerFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * mainThrustPerFrame);
        if (!rocketSound.isPlaying)
        {
            rocketSound.PlayOneShot(mainEngine);
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
