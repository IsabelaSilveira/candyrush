using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
	public string choice { get; private set; }
	public string caraCoroa { get; private set; }

    // Start is called before the first frame update
    void Start()
	{
		choice = "";
		caraCoroa = "";
    	DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
	}

	[Command]
	public void CmdTakeChoice(string s){
		choice = s;
	}

	[Command]
	public void CmdTakeCaraCoroa(string s){
		caraCoroa = s;
	}
}