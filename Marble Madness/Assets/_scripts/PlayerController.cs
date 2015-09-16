//Rollerball game, main player script

/*TODO:
 *      Make UI and controls for iOS
 *      Make models for pickups/polish in general
 * 		Levels
 * 		Think of more powerups/features in general
 * 		Polish particle effects
 * 		
 * EVENTUALLY: Multiplayer mode with ablity to drop bombs/create holes in the ground (need to learn how to subtract through scripting)
 * In-game content creator with user uploading, browsing, and downloading functionality
 * Random obstacle spawner?
 * Possible enemies of some kind?
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class GameText
{
	public GUIText countText;
	public GUIText winText;
	public GUIText timeText;
	public GUIText lifeText;
}

public class PlayerController : MonoBehaviour 
{
	//variable declarations
	public float speed;	
	private float jump;
	public float timeLimit;
	public float livesCount;
	public float rayHeight;
	public float ballDrag;
    private float newHoriz;
    private float newVert;
    public float brakeFactor;
    private Vector3 threshold;
    private Vector3 magnitude;
    private float jumpTimer;
	private Vector3 truespeed;
	public float subTimeLimit;
	private int count; 
	private Vector3 initialSpawn;
    private Vector3 currentSpawn;
	public GameText gameText;
	public Joystick jstick;
	private ParticleSystem playerExplode;
	private Renderer playerRender;
    public GameObject raft;
    Rigidbody raftrb;
    public Vector3 raftv;

	bool timerActive;
	bool fastTimer;
	bool doJump;
	bool grounded;
	bool onGui;
	bool isSlowMo;
	bool isFastMo;
	bool isBrakes;
    bool checkpoint;


    public float brakeThresh = 0.9f;
    public float vectorDistance = 0f;
	//initiates UI text
	void Start ()
	{
        //lots of initial varible value assignments
		playerRender = GetComponent<Renderer>();
		timerActive = true;
		onGui = false;
		isSlowMo = false;
		isFastMo = false;
		fastTimer = false;
		isBrakes = false;
        checkpoint = false;
        jumpTimer = 0.02f;
        initialSpawn = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
		this.gameObject.GetComponent<TrailRenderer>().enabled = false;
		gameText.timeText.text = "";
		count = 0;
		SetCountText();
		gameText.winText.text = "";
		gameText.lifeText.text = "Lives remaining: ";
	}

	//controls
	void FixedUpdate ()
	{
       raftrb = raft.GetComponent<Rigidbody>();
        raftv = raftrb.velocity;
		truespeed = GetComponent<Rigidbody>().velocity;
		//movement
		jump = 20;
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
        Vector3 velocity = this.GetComponent<Rigidbody>().velocity.normalized;
        Vector3 rawVelocity = this.GetComponent<Rigidbody>().velocity;
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 jumpHeight = new Vector3(0.0f, jump, 0.0f);
		GetComponent<Rigidbody>().AddForce(movement * Mathf.Clamp(speed, 0.0f, 200.0f) * Time.deltaTime);
        magnitude = movement + velocity;
        if (Input.anyKey && grounded == true)
        {
            vectorDistance = Vector3.Distance(movement, velocity);
            if ( vectorDistance > brakeThresh)
            {
                rawVelocity.x *= brakeFactor;
                rawVelocity.z *= brakeFactor;
                this.GetComponent<Rigidbody>().velocity = rawVelocity;
            }
        }


        Debug.Log(jumpTimer);
        Debug.Log(grounded);


        //checks if the player is grounded
		RaycastHit hit;
		Ray gcheckRay = new Ray(this.transform.position, Vector3.down * rayHeight);
		Physics.Raycast(gcheckRay, out hit, rayHeight);
		if (hit.collider != null && hit.collider.tag == "Floor")
		{
			grounded = true;
		}
		else 
		{
			grounded = false;
		}

		//jumping
		if (Input.GetButtonUp("Fire1"))
		{
            jumpTimer += Time.deltaTime;
		}
		if  (Input.GetButtonDown("Fire1") && grounded == true && jumpTimer >= 0.02f)
		{
			GetComponent<Rigidbody>().AddForce(jumpHeight * jump);
            jumpTimer = 0.0f;
		}

		//time countdown
		if ((timeLimit > 0) && (timerActive == true))
		{
			timeLimit -= Time.deltaTime;
			SetCountText();
		}

		if ((timeLimit > 0) && (fastTimer == true))
		{
			timeLimit -= Time.deltaTime * 2;
			SetCountText();
		}


		//slowmo effect
		if (isSlowMo == true)
		{
			if (subTimeLimit > 0)
			{
				subTimeLimit -= Time.deltaTime;
				timerActive = false; 
				Time.timeScale = 0.5f;
				moveHorizontal *= 2;
			}
			
			if (subTimeLimit <= 0)
			{
				timerActive = true;
				Time.timeScale = 1;
				speed = 250;
				moveHorizontal *= 1;
			}
		}

		//fastmo effect
		if (isFastMo == true)
		{
			if (subTimeLimit > 0)
			{
				subTimeLimit -= Time.deltaTime;
				timerActive = false;
				fastTimer = true;
				this.gameObject.GetComponent<TrailRenderer>().enabled = true;
				Time.timeScale = 2;
				speed = 400;
			}

			if (subTimeLimit <= 0)
			{
				timerActive = true;
				fastTimer = false;
				this.gameObject.GetComponent<TrailRenderer>().enabled = false;
				Time.timeScale = 1;
				speed = 250;
			}
		}

        //brakes/slows the player
		if (isBrakes == true)
		{
			if (subTimeLimit > 0)
			{
				subTimeLimit -= Time.deltaTime;
				GetComponent<Rigidbody>().drag += ballDrag;
			}

			if (subTimeLimit <= 0)
			{
				isBrakes = false;
				GetComponent<Rigidbody>().drag = 0.0f;
			}
		}

		//fixes bug where if you collect both fast and slow at the same time the speed stays at 400
		if (isSlowMo == false && isFastMo == false)
		{
			Time.timeScale = 1;
			speed = 250;
		}

        if (timeLimit <= 0)
        {
            livesCount -= 1;
            noCheckpoint();
        }

	}
	

	//triggers
	public void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "SlowMo")
		{
			isSlowMo = true;
			subTimeLimit = 2;
		}
		if (other.gameObject.tag == "FastMo")
		{
			isFastMo = true;
			subTimeLimit = 3;
		}
		if (other.gameObject.tag == "Brakes")
		{
			isBrakes = true;
			subTimeLimit = 2;
		}
		if(other.gameObject.tag == "PickUp")
		{
			isSlowMo = false;
			isFastMo = false;
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
		}
        if (other.gameObject.tag == "Checkpoint")
        {
            currentSpawn = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            checkpoint = true;
        }
		if (other.gameObject.tag == "OutOfBounds")
		{
            Destroy();
		}
      
	}
		
    //collisions
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "SpikeWall")
		{
            if (checkpoint == true)
            {
                Destroy();
            }
            else
            {
                noCheckpoint();
            }
		}
        if (other.gameObject.tag == "Lava")
        {
            if (checkpoint == true)
            {
                Destroy();
            }
            else
            {
                noCheckpoint();
            }
        }
        if (other.gameObject.tag == "Platform")
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            raft = other.gameObject;
        }
	}

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Platform" && raftrb.velocity.z >= 10)
        {
            this.GetComponent<Rigidbody>().MovePosition(raftrb.transform.position + transform.forward * Time.deltaTime);
        }
    }
    
	//Text output and game endings
	void SetCountText ()
	{
		gameText.countText.text = "Score: " + count.ToString();
		gameText.timeText.text = "Time remaining: " + timeLimit.ToString();
		gameText.lifeText.text = "Lives remaining: " + livesCount.ToString();
		if(count >= 13)
		{
			gameText.winText.text = "YOU WIN!";
			timerActive = false;
			gameText.timeText.text = "";
		}
		if (timeLimit <= 0 || livesCount == 0)
		{
			gameText.timeText.text = "Time remaining: 0.0";
			gameText.winText.text = "GAME OVER";
			Time.timeScale = 0;
			onGui = true;
		}
	}

	//Game over menu
	void OnGUI()
	{
		if (onGui == true)
		{
			GUI.Box (new Rect(300, 75, 100, 150), "Restart?");
			if (GUI.Button (new Rect(310, 100, 80, 50), "Yes"))
			{
				Restart ();
			}
			if (GUI.Button (new Rect(310, 165, 80, 50), "No"))
			{
				Application.Quit();
			}
		}
	}

    //respawns
	public void Respawn()
	{
		livesCount = livesCount - 1;
		playerRender.enabled = false;
		this.transform.position = currentSpawn;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		playerRender.enabled = true;
	}

    public void noCheckpoint()
    {
        livesCount = livesCount - 1;
        playerRender.enabled = false;
        this.transform.position = initialSpawn;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        playerRender.enabled = true;
    }

    public void Destroy()
    {
        playerExplode = GetComponent<ParticleSystem>();
        playerExplode.Play();
        playerExplode.Clear();
        Respawn();
    }

	//restarts the game
	void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1;
	}
}
