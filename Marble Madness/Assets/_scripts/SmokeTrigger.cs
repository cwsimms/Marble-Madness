using UnityEngine;
using System.Collections;

public class SmokeTrigger : MonoBehaviour {

	private ParticleSystem Smoke;

	public void Smoketrigger()
	{
		Smoke = GetComponentInChildren<ParticleSystem>();
		Smoke.Play();
		Smoke.Clear();
	}

}
