using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{


	public string powerUp;

	// Update is called once per frame
	void Update (){
		this.gameObject.GetComponentsInChildren<Transform> ()[1].Rotate (Vector3.up * Time.deltaTime * 180f);
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.name.StartsWith ("Player")) {
			Score.powerUp++;
			switch (powerUp) {
			case "Jump":
				other.gameObject.GetComponent<PlayerController> ().powerUpJump ();
				break;
			case "Speed":
				PlataformGenerator.speed *= 1.5f;
				break;
			case "Shield":
				other.gameObject.GetComponent<PlayerController> ().powerUpShield ();
				break;
			default:
				break;
			}
			var ChildCount = this.transform.childCount;
			for (int i = 0; i < ChildCount; i++) {
				if (this.transform.GetChild (i).name == "Brilho") {
					this.transform.GetChild (i).gameObject.AddComponent<DestroyParticula2> ();
					this.transform.GetChild (i).gameObject.GetComponent<DestroyParticula2> ().d(5);
					this.transform.GetChild (i).SetParent (other.transform);
					break;
				}
			}
			Destroy (this.gameObject);
		}
	}
}