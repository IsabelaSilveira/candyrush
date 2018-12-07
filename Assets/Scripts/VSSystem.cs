using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSSystem : MonoBehaviour
{

	private bool randomJump = false;
	private bool randomMonster = false;
	public Text meddlerScore;
	public Text walkerScore;
	GameObject Monster;

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.GetString ("Mode", "Walker") == "Walker") {
			WalkerMode ();
		} else {
			MeddlerWork ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlataformGenerator.Player.transform.position.x < -15f || PlataformGenerator.Player.transform.position.y < -5f) {
			try {
				GameObject.Find ("Main Camera W1").GetComponent<AudioSource> ().mute = true;
				GameObject.Find ("Main Camera M1").GetComponent<AudioSource> ().mute = true;
			} catch (System.NullReferenceException) {
			}
			God.GameOver ("Meddler");
		}
		if (randomJump && Random.value < 0.01f) {
			if (Random.value < 0.3f)
				PlataformGenerator.instance.buttonPew ();
			if (Random.value < 0.5f) {
				PlataformGenerator.Player.gameObject.GetComponent<PlayerController> ().jump ();
			} else {
				PlataformGenerator.Player.gameObject.GetComponent<PlayerController> ().sweep ();
			}
			if (PlataformGenerator.Player.transform.position.x < -20f || PlataformGenerator.Player.transform.position.y < -5f) {
				try {
					GameObject.Find ("Main Camera W1").GetComponent<AudioSource> ().mute = true;
					GameObject.Find ("Main Camera M1").GetComponent<AudioSource> ().mute = true;
				} catch (System.NullReferenceException) {
				}
				God.GameOver ("Meddler");
			}
		}
		if (randomMonster && Random.value < 0.02f) {
			GameMaster.instance.obstaculos--;
			if (Random.value < 0.1) {
				GameMaster.dropPlataform ();
			} else if (Random.value < 0.1) {
				GameMaster.barracaDoce ();
			} else if (Random.value < 0.75) {
				GameMaster.spawnMonster (0);
			} else {
				switch (Random.Range (1, 4)) {
				case 1:
					GameMaster.spawnPowerUp ("Jump");
					break;
				case 2:
					GameMaster.spawnPowerUp ("Shield");
					break;
				case 3:
					GameMaster.spawnPowerUp ("Speed");
					break;
				}
			}
		}

		meddlerScore.text = (150 * Score.despawned + 200 * Score.powerUp).ToString ();
		walkerScore.text = (10 * Score.despawned + 200 * Score.died + 50 * Score.powerUp).ToString (); 
	}

	void WalkerMode ()
	{
		randomMonster = true;
		GameObject.Find ("Main Camera M1").SetActive (false);
		GameObject.Find ("Meddler Panel").SetActive (false);
		PlayerController.active = true;
	}

	void MeddlerWork ()
	{
		randomJump = true;
		GameObject.Find ("Main Camera W1").SetActive (false);
		GameObject.Find ("Walker Panel").SetActive (false);
		GameMaster.active = true;
	}
}

/*
			bool jumps = false;
			foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
				if (((plataforma.transform.position.x > -10 && plataforma.transform.position.x < 0) && plataforma.name.StartsWith ("Plat"))) {
					jumps = true;
				}
			}
			if (jumps && Random.value < 0.9f) {
				PlataformGenerator.Player.gameObject.GetComponent<PlayerController> ().jump ();
				jumps = false;
			}
			bool sweeps = false;
			foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
				if (((plataforma.transform.position.x > -10 && plataforma.transform.position.x < 0) && plataforma.name.StartsWith ("Barr"))) {
					sweeps = true;
				}
			}
			if (sweeps && Random.value < 0.9f) {
				PlataformGenerator.Player.gameObject.GetComponent<PlayerController> ().sweep ();
				sweeps = false;
			}
*/