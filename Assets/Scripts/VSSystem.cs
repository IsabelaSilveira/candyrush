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
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
				if (nPlayer.GetComponent<NetworkPlayer> ().choice == "Walker") {
					WalkerMode ();
				} else {
					MeddlerWork ();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		meddlerScore.text = (150 * Score.despawned + 200 * Score.powerUp).ToString ();
		walkerScore.text = (10 * Score.despawned + 200 * Score.died + 50 * Score.powerUp).ToString ();
	}

	void WalkerMode ()
	{
		//randomMonster = true;
		//GameObject.Find ("Main Camera M1").SetActive (false);
		GameObject.Find ("Meddler Panel").SetActive (false);
		PlayerController.active = true;
	}

	void MeddlerWork ()
	{
		//randomJump = true;
		//GameObject.Find ("Main Camera W1").SetActive (false);
		GameObject.Find ("Walker Panel").SetActive (false);
		GameMaster.active = true;
	}
}