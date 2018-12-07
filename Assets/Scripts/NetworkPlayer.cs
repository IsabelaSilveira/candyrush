using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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