using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtExit : MonoBehaviour {

	void OnMouseDown () {
		this.GetComponent<AudioSource> ().Play ();
		Application.Quit ();
	}
}
