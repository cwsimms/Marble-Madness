using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour 
{
	//variable declaration
	public float speed;	
	public float jump;
	public float timeLimit;
	private float subTimeLimit;
	private int count; 
	public GUIText countText;
	public GUIText winText;
	public GUIText timeText;
	bool timerActive;

	//initiates UI text
	void Start ()
	{
		timerActive = true;
		timeText.text = "";
		count = 0;
		SetCountText();
		winText.text = "";
	}

	//controls
	void FixedUpdate ()
	{
		//movement
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 jumpHeight = new Vector3(0.0f, jump, 0.0f);
		GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
		//jumping
		if  (Input.GetButtonDown("Fire1"))
		{
			GetComponent<Rigidbody>().AddForce(jumpHeight * jump);
		}
		//time countdown
		if ((timeLimit > 0) && (timerActive == true))
		{
			timeLimit -= Time.deltaTime;
			SetCountText();
		}
	}
	//collects pickups
		void OnTriggerEnter(Collider other) 
	{

		if (other.gameObject.tag == "Slowmo")
		{
			subTimeLimit = 5;
			other.gameObject.SetActive(false);
			if (subTimeLimit > 0)
			{
				subTimeLimit -= Time.deltaTime;
				timerActive = false; 
				Time.timeScale = 0.5f;
			}

			if (subTimeLimit <= 0)
			{
				timerActive = true;
				Time.timeScale = 1;
			}
		}
		if(other.gameObject.tag == "PickUp")
		{
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
		}
	}

	//scoring
	void SetCountText ()
	{
		countText.text = "Score: " + count.ToString();
		timeText.text = "Time remaining: " + timeLimit.ToString();
		if(count >= 13)
		{
			winText.text = "YOU WIN!";
			timerActive = false;
			timeText.text = "";
		}
		if (timeLimit <= 0)
		{
			timeText.text = "Time remaining: 0.0";
			winText.text = "GAME OVER";
			Time.timeScale = 0;
			Debug.Log (timeLimit);
		}
	}
}
