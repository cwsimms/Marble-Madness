using UnityEngine;
using System.Collections;

public class isCollide : MonoBehaviour {

	void FixedUpdate()
	{
		if (ParticleActive.isCollide == true)
		{
			this.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			this.GetComponent<Renderer>().enabled = true;
		}
	}

}