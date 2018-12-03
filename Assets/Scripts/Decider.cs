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
	private NetworkPlayer thisOne;

	// Update is called once per frame
	void Update ()
	{
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
						PlayerPrefs.SetString ("Mode", "Meddler");
					} else {
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
		SceneManager.LoadSceneAsync ("qwer");

	}

	public void CaraCoroa (string cc)
	{
		thisOne.caraCoroa = cc;
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (!nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
				if (cc == "Cara") {
					nPlayer.GetComponent<NetworkPlayer> ().caraCoroa = "Coroa";
				} else {
					nPlayer.GetComponent<NetworkPlayer> ().caraCoroa = "Cara";
				}
			}
		}
	}

	public void Choice (string c)
	{
		thisOne.choice = c;
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