using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Linq;

[RequireComponent (typeof(NetworkManager))]
public class NetworkInterface : MonoBehaviour
{
	public void getIp (Text target){
		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopHost ();
		NetworkManager.singleton.networkAddress = Dns.GetHostEntry (Dns.GetHostName ()).AddressList.FirstOrDefault (a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
		target.text += NetworkManager.singleton.networkAddress;
		NetworkManager.singleton.StartHost ();
	}

	public void rememberIp (InputField target){
		target.text = PlayerPrefs.GetString ("LastIp", Dns.GetHostEntry (Dns.GetHostName ()).AddressList.FirstOrDefault (a => a.AddressFamily == AddressFamily.InterNetwork).ToString ());
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