using UnityEngine;
using System.Collections;

public class Deactivate : MonoBehaviour {

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			GetComponent<Renderer>().enabled = false;
			GetComponentInChildren<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			GetComponentInChildren<Collider>().enabled = false;
		}
	}

}
