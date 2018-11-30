using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cara_Coroa : MonoBehaviour
{
	void OnTriggerEnter (Collider other){
		if (this.transform.parent.gameObject.GetComponent<CoinSpin> ().flipped) {
			this.transform.parent.gameObject.GetComponent<CoinSpin> ().result = this.gameObject.name;
		}
	}
}
