using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

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
    bool collisionsDisabled = false;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (state == State.Alive) {
            RespondToTrustInput();
            RepsondToRotationInput();
        }

        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }
    }

    void OnCollisionEnter (Collision collision) {
        if (state != State.Alive || collisionsDisabled) { return; } // No extra collision stuff when not alive

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

        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartSuccessSequence() {
        state = State.Transcending;

        audioSource.Stop();
        audioSource.PlayOneShot(success);

        successParticles.Play();

        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence() {
        state = State.Dying;

        audioSource.Stop();
        audioSource.PlayOneShot(death);

        deathParticles.Play();

        Invoke("LoadSameLevel", levelLoadDelay);
    }

    private void LoadSameLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        StartAliveSequence();
    }

    private void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings) {
           nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
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
            StopApplyingThrust();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }

        mainEngineParticles.Play();
    }

    private void StopApplyingThrust() {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void RepsondToRotationInput() {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    private void RespondToDebugKeys() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            collisionsDisabled = !collisionsDisabled;
        }
    }
}

