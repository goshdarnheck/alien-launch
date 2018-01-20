using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour {

    [SerializeField] ParticleSystem collideParticles;
    //Renderer renderer;

    // Use this for initialization
    void Start () {
        //renderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        var hitObject = other.name;
        print("I collided with the " + hitObject + " !");

        collideParticles.Play();
    }
}
