using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
	public string choice { get; private set; }
	public string caraCoroa { get; private set; }
	public string skin { get; private set; }

    // Start is called before the first frame update
    void Start()
	{
		choice = "";
		caraCoroa = "";
		if (isLocalPlayer) {
			CmdChangeSkin(PlayerPrefs.GetString ("Skin", "Player1"));
		}
    	DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		if (SceneManager.GetActiveScene ().name == "qwer") {
			if (isLocalPlayer && choice == "Meddler") {
				this.gameObject.GetComponent<Camera> ().CopyFrom (GameObject.Find ("Main Camera M1").GetComponent<Camera> ());
				this.gameObject.GetComponent<Camera> ().enabled = true;
				this.transform.SetParent (GameObject.Find ("Main Camera M1").transform);
			} else if (isLocalPlayer && choice == "Walker") {
				this.gameObject.GetComponent<Camera> ().CopyFrom (GameObject.Find ("Main Camera W1").GetComponent<Camera> ());
				this.gameObject.GetComponent<Camera> ().enabled = true;
				this.transform.SetParent (GameObject.Find ("Main Camera W1").transform);
			}
		}
	}

	[Command]
	private void CmdChangeSkin(string s){
		skin = s;
	}

	[Command]
	public void CmdTakeChoice(string s){
		choice = s;
	}

	[Command]
	public void CmdTakeCaraCoroa(string s){
		caraCoroa = s;
	}
	/*
	[Command]
	public void CmdsetResult(string s){
		if (isLocalPlayer) {
			choice = s;
		}
	}*/
}