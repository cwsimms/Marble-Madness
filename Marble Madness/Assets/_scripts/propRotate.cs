using UnityEngine;
using System.Collections;

public class propRotate : MonoBehaviour {

	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(new Vector3(0.0f, 25, 0.0f) * Time.deltaTime);
	}
}
