using UnityEngine;
using System.Collections;

public class DestroyParticula2 : MonoBehaviour
{

	public void d (int t)
	{
		StartCoroutine (D (t));
	}

	private IEnumerator D (int t)
	{
		yield return new WaitForSeconds (t);
		Destroy (gameObject);
	}

}
