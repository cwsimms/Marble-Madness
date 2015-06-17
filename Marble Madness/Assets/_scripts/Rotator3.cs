using UnityEngine;
using System.Collections;

public class Rotator3 : MonoBehaviour {
	public float rotateSpeed;
		// Update is called once per frame
	void Update () 
	{
		transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * rotateSpeed);
	}
}
