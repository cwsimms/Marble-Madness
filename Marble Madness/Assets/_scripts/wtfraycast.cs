using UnityEngine;
using System.Collections;

public class wtfraycast : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        RaycastHit hit;
        Ray gcheckRay = new Ray(this.transform.position, Vector3.down);
        Physics.Raycast(gcheckRay, out hit, 1.0f);
        
	}
}
