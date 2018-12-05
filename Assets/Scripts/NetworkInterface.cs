using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent (typeof(NetworkManager))]
public class NetworkInterface : MonoBehaviour
{
	public void getIp (Text target){
		target.text = NetworkManager.singleton.networkAddress;
		NetworkManager.singleton.StartServer ();
	}

	public void setIp (InputField target){
		NetworkManager.singleton.networkAddress = target.text;
		NetworkManager.singleton.StartClient ();
	}
}