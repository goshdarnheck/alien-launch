using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (state == State.Alive) {
            RespondToTrustInput();
            RepsondToRotationInput();
        }
    }

    void OnCollisionEnter (Collision collision) {
        if (state != State.Alive) { return; } // No extra collision stuff when not alive

        switch (collision.gameObject.tag) {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                print("You got some fuel");
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartAliveSequence() {
        state = State.Alive;

        audioSource.Stop();
        audioSource.PlayOneShot(success);

        successParticles.Stop();
        deathParticles.Stop();

        Invoke("LoadNextLevel", 1f);
    }

    private void StartSuccessSequence() {
        state = State.Transcending;

        audioSource.Stop();
        audioSource.PlayOneShot(success);

        successParticles.Play();

        Invoke("LoadNextLevel", 1f);
    }

    private void StartDeathSequence() {
        state = State.Dying;

        audioSource.Stop();
        audioSource.PlayOneShot(death);

        deathParticles.Play();

        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(1);
        StartAliveSequence();
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
        StartAliveSequence();
    }

    private void RespondToTrustInput() {
        if (Input.GetKey(KeyCode.W)) {
            ApplyThrust();
        } else {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);

        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }

        mainEngineParticles.Play();
    }

    private void RepsondToRotationInput() {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}

