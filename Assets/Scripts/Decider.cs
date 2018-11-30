using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Decider : MonoBehaviour
{
	public GameObject coin;
	public string choice;
	public string caraCoroa;
	private bool decided = false;
	public GameObject CCpanel;
	//private NetworkPlayer thisOne;

	// Update is called once per frame
	void Update ()
	{
		/*if (!thisOne) {
			foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
				if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
					thisOne = nPlayer.GetComponent<NetworkPlayer> ();
				}
			}
		} else */{
			/*if (thisOne.caraCoroa != "") {
				coin.SetActive (true);
			}*/
			if (coin.activeSelf && coin.GetComponent<CoinSpin> ().result != "") {
				if (coin.GetComponent<CoinSpin> ().result == caraCoroa) {
					PlayerPrefs.SetString ("Mode", choice);
				} else {
					if (choice == "Walker") {
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
		//thisOne.play ();
		SceneManager.LoadSceneAsync ("qwer");

	}

	public void CaraCoroa (string cc)
	{
		caraCoroa = cc;
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
		choice = c;
//		float min = Mathf.Infinity;
//		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
//			Debug.Log (nPlayer.name);
//			min = nPlayer.GetComponent<NetworkPlayer> ().playerControllerId;
//			break;
//		}
//		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
//			if (nPlayer.GetComponent<NetworkPlayer> ().playerControllerId < min) {
//				min = nPlayer.GetComponent<NetworkPlayer> ().playerControllerId;
//			}
//		}
//		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
//			if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer && nPlayer.GetComponent<NetworkPlayer> ().playerControllerId == min) {
				CCpanel.SetActive (true);
//			}
//		}
	}
}