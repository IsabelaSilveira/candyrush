﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Decider : NetworkBehaviour
{
	public GameObject coin;
	private bool decided = false;
	public GameObject CxPanel;
	public GameObject Cpanel;
	public GameObject CCpanel;
	private NetworkPlayer thisOne;
	private NetworkPlayer higherOne;

	// Update is called once per frame
	void Start ()
	{
	}

	void Update ()
	{
		if (GameObject.FindGameObjectsWithTag ("NetPlayer").Length != 2) {
			CxPanel.SetActive (true);
			Cpanel.SetActive (false);
		} else {
			CxPanel.SetActive (false);
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
				if (thisOne == higherOne) {
					if (coin.GetComponent<CoinSpin> ().result == thisOne.caraCoroa) {
						foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
							nPlayer.GetComponent<NetworkPlayer> ().CmdTakeChoice ((thisOne.choice == "Meddler") ? "Walker" : "Meddler");
						}
						thisOne.CmdTakeChoice (thisOne.choice);
					} else {
						foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
							nPlayer.GetComponent<NetworkPlayer> ().CmdTakeChoice (thisOne.choice);
						}
						thisOne.CmdTakeChoice ((thisOne.choice == "Meddler") ? "Walker" : "Meddler");
					}
				}
				PlayerPrefs.SetString ("Mode", thisOne.choice);
				/*if (coin.GetComponent<CoinSpin> ().result == thisOne.caraCoroa) {
					thisOne.CmdsetResult(thisOne.choice)
					PlayerPrefs.SetString ("Mode", thisOne.choice);
				} else {
					if (thisOne.choice == "Walker") {
						thisOne.CmdTakeChoice("Meddler");
						PlayerPrefs.SetString ("Mode", "Meddler");
					} else {
						thisOne.CmdTakeChoice("Walker");
						PlayerPrefs.SetString ("Mode", "Walker");
					}
				}*/
				if (!decided)
					StartCoroutine (QWER ());
			}
		}
	}

	private IEnumerator QWER ()
	{
		decided = true;
		GameObject loading = Instantiate (Resources.Load ("loading") as GameObject);
		loading.transform.SetParent (GameObject.FindObjectOfType<Canvas>().transform);
		loading.transform.localRotation = Quaternion.identity;
		loading.transform.localPosition = Vector2.zero;
		loading.transform.localScale = Vector2.one;
		yield return new WaitForSeconds (1);
		NetworkManager.singleton.ServerChangeScene ("qwer");

	}

	public void CaraCoroa (string cc)
	{
		thisOne.CmdTakeCaraCoroa (cc);
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (!nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer) {
				if (cc == "Cara") {
					nPlayer.GetComponent<NetworkPlayer> ().CmdTakeCaraCoroa ("Coroa");
				} else {
					nPlayer.GetComponent<NetworkPlayer> ().CmdTakeCaraCoroa ("Cara");
				}
			}
		}
	}

	public void Choice (string c)
	{
		thisOne.CmdTakeChoice (c);
		float min = Mathf.Infinity;
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (nPlayer.GetComponent<NetworkPlayer> ().playerControllerId < min) {
				min = nPlayer.GetComponent<NetworkPlayer> ().playerControllerId;
			}
		}
		foreach (GameObject nPlayer in GameObject.FindGameObjectsWithTag("NetPlayer")) {
			if (nPlayer.GetComponent<NetworkPlayer> ().isLocalPlayer && nPlayer.GetComponent<NetworkPlayer> ().playerControllerId == min) {
				higherOne = nPlayer.GetComponent<NetworkPlayer> ();
				CCpanel.SetActive (true);
			}
		}
	}
}