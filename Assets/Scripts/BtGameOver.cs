using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtGameOver : MonoBehaviour {

	void OnMouseDown () {
		SceneManager.LoadScene ("MenuInicial");
	}
}
