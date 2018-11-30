using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMonster : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		this.transform.localScale = new Vector3(0.55f - Mathf.Sin (Time.time + this.transform.position.x) * 0.025f, 0.45f + Mathf.Sin (Time.time + this.transform.position.x) * 0.025f, 0.55f  - Mathf.Sin (Time.time + this.transform.position.x) * 0.025f);
		this.GetComponent<SphereCollider> ().radius = this.transform.localScale.y * 1.56f;
	}
}
