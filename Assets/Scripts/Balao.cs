using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balao : MonoBehaviour
{
	private Vector3 initialP;
	private Quaternion initialR;
	private float count = 0;
	public float amplitude = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
		initialP = this.gameObject.transform.position;
		initialR = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
		count += Time.deltaTime;
		this.gameObject.transform.position = new Vector3 (initialP.x, initialP.y + Mathf.Sin(count) * amplitude, initialP.z);
		//this.gameObject.transform.rotation = Quaternion.Euler(initialR.x, initialR.y, initialR.z + Mathf.Sin(count*3) * amplitude);
    }
}
