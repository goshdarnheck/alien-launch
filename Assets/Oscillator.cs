using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    float movementFactor; // 0 for not moved, 1 for fully moved
    Vector3 startingPos; 

	// Use this for initialization
	void Start() {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update() {
        // set movement factor automatically
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; // about 6.28
        float rawSineWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        movementFactor = rawSineWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
