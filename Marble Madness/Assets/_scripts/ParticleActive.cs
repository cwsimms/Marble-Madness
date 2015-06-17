using UnityEngine;
using System.Collections;

public class ParticleActive : MonoBehaviour {

	public ParticleSystem ShimmerEffect;
	public static bool isCollide;


	void Start()
	{
		isCollide = false;
		ShimmerEffect.Stop();
		ShimmerEffect.Clear();
	}

void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag.Equals("Player"))
		{
			isCollide = true;
			ShimmerEffect.Play();			
		}
	}
}

