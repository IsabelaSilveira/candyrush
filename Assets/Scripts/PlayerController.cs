using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
[RequireComponent (typeof(SwipeDetector))]
[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{

	public static bool active = false;
	public static int HP;
	public float JumpSpeed = 10;
	public float rayDistance = 2f;
	// distance center to ground
	private Animator animator;
	private Rigidbody rigidBody;
	private CapsuleCollider capsuleCollider;

	public Vector3 newCenterSweepKick;
	public Vector3 newCenterRun;

	public static AudioSource Sons;
	public AudioClip SomJump;
	public AudioClip SomSlide;

	public bool stumble;
	// tropeçar. trigger
	public bool sweepKick;
	// not a trigger
	public bool isGround;
	public bool goingUp;
	// true going up, false going down
	public float jumpDelay = 0.1f;
	// stop fast double jumps
	public float stumbleDuration = 0.5f;
	// time
	public float sweepKickDuration = 1f;
	// time
	public float inactiveTime;
	// if action time is less than this time, can't do action
	public float actionTime;

	public bool shield = false;

	private GameObject lastMonsterCollided;

	// Use this for initialization
	void Start ()
	{
		if (this.gameObject != PlataformGenerator.Player) {
			Destroy (this);
		}
		HP = 5;
		rigidBody = GetComponent<Rigidbody> ();
		animator = this.gameObject.GetComponentsInChildren <Animator> () [0];
		capsuleCollider = this.gameObject.GetComponent <CapsuleCollider> ();
		Sons = this.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position = Vector3.Lerp (new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z), new Vector3 (-13.2f, this.transform.position.y, this.transform.position.z), Time.deltaTime / 3);
		this.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, this.GetComponent<Rigidbody> ().velocity.y, 0f);

		if (active) {
			if (HP <= 0 || this.transform.position.x < -20f || this.transform.position.y < -5f) {
				try {
					GameObject.Find ("Main Camera W1").GetComponent<AudioSource> ().mute = true;
					GameObject.Find ("Main Camera M1").GetComponent<AudioSource> ().mute = true;
				} catch (System.NullReferenceException) {
				}
				God.GameOver ("Meddler");
			}

			rigidBody.AddForce (Vector3.down * JumpSpeed);
			if ((Input.GetKeyDown (KeyCode.UpArrow)) || (SwipeDetector.swipeValue > 0)) {
				jump ();
			}

			//pressionar o botao ou deslizar o dedo para baixo
			if (Input.GetKeyDown (KeyCode.DownArrow) || (SwipeDetector.swipeValue < 0)) {
				sweep ();
			}
			if (sweepKick == true) {
				actionTime += Time.deltaTime;
			}
			if (actionTime >= sweepKickDuration) {
				SwipeDetector.swipeValue = 0;
				capsuleCollider.height = 4.5f;
				capsuleCollider.center = newCenterRun;
				sweepKick = false;
				actionTime = 0;
			}
		}
	}

	public void jump ()
	{
		if (Time.time < inactiveTime) {
			Debug.Log ("Can't jump yet");
			return;
		} else if (isGround == false) {
			Debug.Log ("Can't jump, not grounded");
			return;
		}
		inactiveTime = Time.time + jumpDelay;
		goingUp = true;
		isGround = false;
		rigidBody.AddForce (Vector3.up * JumpSpeed * 40);
		SwipeDetector.swipeValue = 0;
		Sons.PlayOneShot (SomJump);
		animator.SetTrigger ("jumpTrigger");
	}

	public void sweep ()
	{
		sweepKick = true;
		capsuleCollider.height = 2.25f;
		capsuleCollider.center = newCenterSweepKick;
		Sons.PlayOneShot (SomSlide);
		animator.SetTrigger ("slideTrigger");
	}

	void FixedUpdate ()
	{
		// check if character is on ground
		if (Physics.Raycast (transform.position + (Vector3.up * rayDistance), Vector3.down, rayDistance * 2)) {
			isGround = true;
		} else {
			isGround = false;
		}

		// check if character is going up or down
		if (rigidBody.velocity.y > 0) {
			goingUp = true;
		} else {
			goingUp = false;
		}
	}

	public void laserSound ()
	{
		Sons.PlayOneShot (Resources.Load ("Audio/personagem/Laser") as AudioClip);
	}

	public void powerUpJump ()
	{
		StartCoroutine (PowerUpJump ());
	}

	public void powerUpShield ()
	{
		StartCoroutine (PowerUpShield ());
	}

	private IEnumerator PowerUpJump ()
	{
		JumpSpeed *= 1.2f;
		yield return new WaitForSeconds (5);
		JumpSpeed /= 1.2f;
		int ChildCount = 0;
		for (int i = 0; i < ChildCount; i++) {
			if (this.transform.GetChild (i).name == "Brilho") {
				Destroy (this.transform.GetChild (i));
			}
		}
	}

	private IEnumerator PowerUpShield ()
	{
		shield = true;
		yield return new WaitForSeconds (5);
		shield = false;
		int ChildCount = 0;
		for (int i = 0; i < ChildCount; i++) {
			if (this.transform.GetChild (i).name == "Brilho") {
				Destroy (this.transform.GetChild (i).gameObject);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (shield) {
			if (other.gameObject.tag == "Monster") {
				other.gameObject.GetComponent<Monster> ().HP = 0;
			}
		} else {
			if (other.gameObject.tag == "Monster" && other.gameObject != lastMonsterCollided) {
				lastMonsterCollided = other.gameObject;
				HP--;
			}
		}
	}
}
