using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pew : MonoBehaviour {

	public GameObject target;

	void Update() {
		if(!target)
			Destroy (this.gameObject);
		this.gameObject.transform.position = Vector3.Lerp (this.transform.position, target.transform.position, Time.deltaTime * PlataformGenerator.speed);
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject == target.gameObject) {
			if (other.GetComponent<Monster> ()) {		
				other.GetComponent<Monster> ().HP--;
			}
			Destroy (this.gameObject);
		}
	}
}