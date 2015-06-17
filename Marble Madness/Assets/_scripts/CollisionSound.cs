using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour {
    public AudioSource audio;
    public AudioSource audio2;
    public float minVelocity = 1.0f;
    public float pitchdivide;
    private Vector3 playerVelocity;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerVelocity = rb.velocity;
	}
	
	// Update is called once per frame
	void Update () {
        float velocity =  GetComponent<Rigidbody>().velocity.magnitude;
        float angularVelocity = GetComponent<Rigidbody>().angularVelocity.magnitude;
        Debug.Log(audio.pitch);
        if (velocity > minVelocity)
        {
            audio.volume = velocity - minVelocity;
            audio.pitch = 1.0f + (angularVelocity / pitchdivide);
           // audio.Play();
        }
        else
        {
            //audio.Stop();
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor" && playerVelocity.x == 0 && playerVelocity.z == 0)
        {
            //bounce sound
        }

        else
        {
            audio2.Play();
        }
    }
}
