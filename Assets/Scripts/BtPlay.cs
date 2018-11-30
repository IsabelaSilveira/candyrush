using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtPlay : MonoBehaviour {

	void OnMouseDown () {
		this.GetComponent<AudioSource> ().Play ();
		SceneManager.LoadScene ("qwer");
	}
}
