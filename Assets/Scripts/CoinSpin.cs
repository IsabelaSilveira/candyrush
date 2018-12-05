using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoinSpin : NetworkBehaviour
{
	[SyncVar]
	public bool flip = false;
	public bool flipped = false;
	[SyncVar]
	public Vector3 force;
	[SyncVar]
	public Vector3 torque;
	[SyncVar]
	public string result = "";

	void Update ()
	{
		bool voted = true;
		float min = Mathf.Infinity;
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			voted = voted && nPlayer.GetComponent<NetworkPlayer> ().choice != "";
			if (nPlayer.GetComponent<NetworkPlayer> ().playerControllerId < min) {
				min = nPlayer.GetComponent<NetworkPlayer> ().playerControllerId;
			}
		}
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (voted && nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer && nPlayer.GetComponent<NetworkPlayer> ().playerControllerId == min) {
				if (!flipped && ((Input.GetKeyDown (KeyCode.UpArrow)) || SwipeDetector.swipeValue > 0)) {
					flipping ();
					/*this.gameObject.GetComponent<Rigidbody> ().useGravity = true;
					this.gameObject.GetComponent<Rigidbody> ().maxAngularVelocity = Mathf.Infinity;
					force = Vector3.up * Mathf.Max (SwipeDetector.swipeValue * 20f, 20f);
					this.gameObject.GetComponent<Rigidbody> ().AddForce (force);
					torque = new Vector3 (Random.Range (-7f, 7f), 0f, Random.Range (-7f, 7f));
					this.gameObject.GetComponent<Rigidbody> ().AddTorque (torque);
					flip = true;
					flipped = true;*/
				} else {
					if (!flipped && flip) {
						this.gameObject.GetComponent<Rigidbody> ().useGravity = true;
						this.gameObject.GetComponent<Rigidbody> ().maxAngularVelocity = Mathf.Infinity;
						this.gameObject.GetComponent<Rigidbody> ().AddForce (force);
						this.gameObject.GetComponent<Rigidbody> ().AddTorque (torque);
						flipped = true;
					}
				}
			}
		}
	}

	//[ClientRpc]
	private void flipping(){
		this.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		this.gameObject.GetComponent<Rigidbody> ().maxAngularVelocity = Mathf.Infinity;
		force = Vector3.up * Mathf.Max (SwipeDetector.swipeValue * 20f, 20f);
		this.gameObject.GetComponent<Rigidbody> ().AddForce (force);
		torque = new Vector3 (Random.Range (-7f, 7f), 0f, Random.Range (-7f, 7f));
		this.gameObject.GetComponent<Rigidbody> ().AddTorque (torque);
		flip = true;
		flipped = true;
	}
}
