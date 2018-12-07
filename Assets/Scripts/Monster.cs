using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

	public int HP;

	// Use this for initialization
	void Start ()
	{
		HP = Random.Range (1, 5);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (HP <= 0) {
			this.gameObject.GetComponent<SphereCollider> ().isTrigger = true;
			Score.died += 1;
			GameObject explosion = Instantiate (Resources.Load ("Prefabs/Explosao") as GameObject, this.transform.position, Quaternion.identity) as GameObject;
			Destroy (this, 1f);
		}
		this.transform.localScale = new Vector3 (1.15f - Mathf.Sin (Time.time + this.transform.position.x) * 0.075f, 0.85f + Mathf.Sin (Time.time + this.transform.position.x) * 0.075f, 1.15f - Mathf.Sin (Time.time + this.transform.position.x) * 0.075f);
		this.GetComponent<SphereCollider> ().radius = this.transform.localScale.y * 0.78f;
	}

	void OnMouseDown ()
	{
		PlataformGenerator.Player.GetComponent<PlayerController> ().laserSound ();
		GameObject pew = Instantiate (Resources.Load ("Prefabs/characters/Pew") as GameObject, PlataformGenerator.Player.transform.position, Quaternion.identity) as GameObject;
		pew.gameObject.GetComponent<Pew> ().target = this.gameObject;
	}
}