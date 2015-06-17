using UnityEngine;
using System.Collections;

public class Rotator2 : MonoBehaviour {
	public float rotateSpeed;
		// Update is called once per frame
	void Update () 
	{
		transform.Rotate(new Vector3(15, 45, 15) * Time.deltaTime * rotateSpeed);
	}
}
