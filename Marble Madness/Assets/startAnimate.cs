using UnityEngine;
using System.Collections;

public class startAnimate : MonoBehaviour {


    void OnCollisionEnter(Collision other)
    { 
        Debug.Log(other.collider.tag);
        if (other.gameObject.tag == "Player")
        {
           
            GetComponent<Animation>().Play();
        }
        else 
        { 

        }
    }

}
