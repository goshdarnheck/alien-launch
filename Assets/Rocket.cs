using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource[] audioSources;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        Thrust();
        Rotate();
        Uhhh();
        Ummm();
    }

    void OnCollisionEnter (Collision collision) {
        switch (collision.gameObject.tag) {
            case "Fuel":
                print("You got some fuel");
                break;
            case "Friendly":
                // do nothing
                print("OK");
                break;
            default:
                print("dead");
                // kill player or whatever
                break;
        }
    }

    private void Thrust () {
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.W)) {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            PlayAudioSource(0);
        } else {
            StopAudioSource(0);
        }

        rigidBody.freezeRotation = false;
    }

    private void PlayAudioSource(int index) {
        if (!audioSources[index].isPlaying) {
            audioSources[index].Play();
        }
    }

    private void StopAudioSource(int index) {
        audioSources[index].Stop();
    }

    private void Rotate () {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
        //    PlayAudioSource(1);
        //} else {
        //    StopAudioSource(1);
        //}

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void Uhhh() {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        //if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) {
        //    PlayAudioSource(1);
        //} else {
        //    StopAudioSource(1);
        //}

        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(Vector3.left * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(-Vector3.left * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void Ummm () {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.S)) {
            transform.Rotate(Vector2.up * rotationThisFrame);
            //PlayAudioSource(1);
        } else {
            //StopAudioSokurce(1);
        }

        rigidBody.freezeRotation = false;
    }
}
