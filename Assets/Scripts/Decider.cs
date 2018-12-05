using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Decider : NetworkBehaviour
{
	public GameObject coin;
	private bool decided = false;
	public GameObject CCpanel;
	public GameObject Cpanel;
	private NetworkPlayer thisOne;

	// Update is called once per frame
	void Start (){
	}

	void Update ()
	{
		if (GameObject.FindGameObjectsWithTag ("NetPlayer").Length != 2) {
			Cpanel.SetActive (false);
		} else {
			Cpanel.SetActive (true);
		}
		if (!thisOne) {
			foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
				if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
					thisOne = nPlayer.GetComponent<NetworkPlayer> ();
				}
			}
		} else {
			if (thisOne.caraCoroa != "") {
				coin.SetActive (true);
			}
			if (coin.activeSelf && coin.GetComponent<CoinSpin> ().result != "") {
				if (coin.GetComponent<CoinSpin> ().result == thisOne.caraCoroa) {
					PlayerPrefs.SetString ("Mode", thisOne.choice);
				} else {
					if (thisOne.choice == "Walker") {
						thisOne.CmdTakeChoice("Meddler");
						PlayerPrefs.SetString ("Mode", "Meddler");
					} else {
						thisOne.CmdTakeChoice("Walker");
						PlayerPrefs.SetString ("Mode", "Walker");
					}
				}
				if (!decided)
					StartCoroutine (QWER ());
			}
		}
	}

	private IEnumerator QWER ()
	{
		decided = true;
		yield return new WaitForSeconds (2);
		// thisOne.play ();
		NetworkManager.singleton.ServerChangeScene ("qwer");

	}

	public void CaraCoroa (string cc)
	{
		thisOne.CmdTakeCaraCoroa(cc);
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (!nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
				if (cc == "Cara") {
					nPlayer.GetComponent<NetworkPlayer> ().CmdTakeCaraCoroa ("Coroa");
				} else {
					nPlayer.GetComponent<NetworkPlayer> ().CmdTakeCaraCoroa("Cara");
				}
			}
		}
	}

	public void Choice (string c)
	{
		thisOne.CmdTakeChoice(c);
		float min = Mathf.Infinity;
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (nPlayer.GetComponent<NetworkPlayer> ().playerControllerId < min) {
				min = nPlayer.GetComponent<NetworkPlayer> ().playerControllerId;
			}
		}
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer && nPlayer.GetComponent<NetworkPlayer> ().playerControllerId == min) {
				CCpanel.SetActive (true);
			}
		}
	}
}