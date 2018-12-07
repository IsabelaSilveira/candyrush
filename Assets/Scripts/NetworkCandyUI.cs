﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent (typeof(NetworkManager))]
public class NetworkCandyUI : MonoBehaviour
{
	public void getIp (Text target){
		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopHost ();
		NetworkManager.singleton.networkAddress = IPManager.GetIP (ADDRESSFAM.IPv4);
		target.text += NetworkManager.singleton.networkAddress;
		NetworkManager.singleton.StartHost ();
	}

	public void rememberIp (InputField target){
		target.text = PlayerPrefs.GetString ("LastIp", IPManager.GetIP (ADDRESSFAM.IPv4));
	}

	public void setIp (InputField target){
		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopHost ();
		NetworkManager.singleton.networkAddress = target.text;
		PlayerPrefs.GetString ("LastIp", NetworkManager.singleton.networkAddress);
		NetworkManager.singleton.StartClient ();
	}

	public void stop (){
		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopHost ();
		NetworkManager.Shutdown ();
	}
}