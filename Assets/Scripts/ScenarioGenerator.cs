using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ScenarioGenerator: MonoBehaviour
{

	public Transform StartPlataformGenerator;
	public static float speed = 3f;
	public static GameObject GameOver;
	GameObject NewPlataform;
	GameObject Player;
	System.Random random = new System.Random ();

	int ChildCount;
	public static Vector3 EndPosition;
	Vector3 EndPosAdjust = new Vector3 (2.2f, 0, 0);

	void Start ()
	{
		NewPlataform = Instantiate (Resources.Load ("Prefabs/plataforms/Plat1") as GameObject, StartPlataformGenerator.position, Quaternion.identity) as GameObject;
		GameOver = GameObject.Find ("GameOver");
		GameOver.SetActive (false);
	}

	void FixedUpdate ()
	{
		if (!PlataformGenerator.GameOver.activeSelf) {
			try {
				ChildCount = NewPlataform.transform.childCount;
				for (int i = 0; i < ChildCount; i++) {
					if (NewPlataform.transform.GetChild (i).name == "End") {
						EndPosition = NewPlataform.transform.GetChild (i).position + EndPosAdjust;
						break;
					}
				}
				if (EndPosition.x < 100f) {
					int n = (random.Next (1, 91) == 1)? 1 : 2;
					if (findRecycle ("Plat" + n.ToString () + "(Clone)")) {
						NewPlataform = findRecycle ("Plat" + n.ToString () + "(Clone)");
						//PrefabUtility.RevertPrefabInstance(NewPlataform);
						NewPlataform.tag = "Plataforma";
						NewPlataform.transform.position = EndPosition;
					} else
						NewPlataform = Instantiate (newPlataform (n), EndPosition, Quaternion.identity) as GameObject;
				}

				foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
					if (!(plataforma.transform.position.x == 666 && plataforma.transform.position.y == 666 && plataforma.transform.position.z == 666))
						plataforma.transform.Translate (-speed * Time.deltaTime, 0, 0);

					if (plataforma.transform.position.x < -20f || plataforma.transform.position.y < -20f) {
						recyclePlataform (plataforma);
					}
				} 

			} catch (MissingReferenceException) {
			}
		} else
			foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
				if (plataforma.gameObject.GetComponent<Rigidbody> () == null) {
					plataforma.gameObject.AddComponent<Rigidbody> ();
					plataforma.gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
				}
			}
	}

	void Restart ()
	{ //E eu vou te esperar...
		SceneManager.LoadScene ("menu");
	}

	GameObject newPlataform (int n)
	{
		return Resources.Load ("Prefabs/plataforms/Plat" + n.ToString ()) as GameObject;
	}

	void recyclePlataform (GameObject X)
	{
		X.transform.position = new Vector3 (666f, 666f, 666f);
		X.tag = "Recycle";
	}

	GameObject findRecycle (string name)
	{
		if (GameObject.FindGameObjectsWithTag ("Recycle") != new GameObject[0]) {
			foreach (GameObject recycle in GameObject.FindGameObjectsWithTag ("Recycle")) {
				if (recycle.name == name) {
					return recycle;
				}
			}
		}
		return null;
	}
}