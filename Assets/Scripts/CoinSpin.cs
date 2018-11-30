using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
	public bool flipped = false;
	public string result = "";

	void Update ()
	{
		if (!flipped && ((Input.GetKeyDown (KeyCode.UpArrow)) || SwipeDetector.swipeValue > 0)) {
			this.gameObject.GetComponent<Rigidbody> ().useGravity = true;
			this.gameObject.GetComponent<Rigidbody> ().maxAngularVelocity = Mathf.Infinity;
			this.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.up * Mathf.Max(SwipeDetector.swipeValue * 20f, 20f));
			this.gameObject.GetComponent<Rigidbody> ().AddTorque (Random.Range(-7f,7f), 0f, Random.Range(-7f,7f));
			flipped = true;
		}
	}
}
