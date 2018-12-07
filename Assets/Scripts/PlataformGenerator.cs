using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlataformGenerator: MonoBehaviour
{

	public Transform StartPlataformGenerator;
	public static float speed = 1f;
	GameObject NewPlataform;
	GameObject NewBackground;
	public static GameObject Player;
	System.Random random = new System.Random ();
	public static PlataformGenerator instance;

	int ChildCount;
	public static Vector3 EndPosition;
	public static Vector3 BgEndPosition;
	Vector3 EndPosAdjust = new Vector3 (2.2f, 0, 0);

	private int bg = 0;

	void Start ()
	{
		instance = this;
		NewPlataform = Instantiate (Resources.Load ("Prefabs/plataforms/Plat1") as GameObject, StartPlataformGenerator.position, Quaternion.identity) as GameObject;
		NewBackground = Instantiate (Resources.Load ("Prefabs/plataforms/Bg1") as GameObject, StartPlataformGenerator.position, Quaternion.identity) as GameObject;
		Player = Instantiate (Resources.Load ("Prefabs/characters/" + PlayerPrefs.GetString ("Skin", "Player1")) as GameObject, StartPlataformGenerator.position + new Vector3 (0, 5f, 0f), Quaternion.identity) as GameObject;
		speed = 10f;
	}

	void FixedUpdate ()
	{
		try {
			ChildCount = NewPlataform.transform.childCount;
			for (int i = 0; i < ChildCount; i++) {
				if (NewPlataform.transform.GetChild (i).name == "End") {
					EndPosition = NewPlataform.transform.GetChild (i).position + EndPosAdjust;
					break;
				}
			}
			if (EndPosition.x < 100f) {
				int n = 1;
				if (findRecycle ("Plat" + n.ToString () + "(Clone)")) {
					NewPlataform = findRecycle ("Plat" + n.ToString () + "(Clone)");
					NewPlataform.tag = "Plataforma";
					NewPlataform.transform.position = EndPosition;
				} else
					NewPlataform = Instantiate (newPlataform (n), EndPosition, Quaternion.identity) as GameObject;
			}

			ChildCount = NewBackground.transform.childCount;
			for (int i = 0; i < ChildCount; i++) {
				if (NewBackground.transform.GetChild (i).name == "End") {
					BgEndPosition = NewBackground.transform.GetChild (i).position;
					break;
				}
			}
			if (BgEndPosition.x < 100f) {
				bg++;
				int n = (bg % 3) + 1;//Random.Range(1,4);
				if (findRecycle ("Bg" + n.ToString () + "(Clone)")) {
					NewBackground = findRecycle ("Bg" + n.ToString () + "(Clone)");
					NewBackground.tag = "Plataforma";
					NewBackground.transform.position = BgEndPosition;
				} else
					NewBackground = Instantiate (newBackground (n), BgEndPosition, Quaternion.identity) as GameObject;
			}

			foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
				if (!(plataforma.transform.position.x == 666 && plataforma.transform.position.y == 666 && plataforma.transform.position.z == 666))
					plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (-speed, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);

				if (plataforma.transform.position.x < -50f || plataforma.transform.position.y < -20f) {
					if (plataforma.name.StartsWith ("Plat") && plataforma.GetComponent<BoxCollider> ().enabled == false) {
						Destroy (plataforma);
					} else {
						plataforma.GetComponent<Rigidbody> ().useGravity = false;
						recycle (plataforma);
					}
				}
			}

			foreach (var monster in GameObject.FindGameObjectsWithTag("Monster")) {
				monster.GetComponent<Rigidbody> ().velocity = new Vector3 (-speed * 1.2f, monster.GetComponent<Rigidbody> ().velocity.y, 0f);
				if (monster.transform.position.x < -30f || monster.transform.position.y < -20f) {
					Score.despawned += 1;
					Destroy (monster);
				}
			}

		} catch (MissingReferenceException) {
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

	GameObject newBackground (int n)
	{
		return Resources.Load ("Prefabs/plataforms/Bg" + n.ToString ()) as GameObject;
	}

	void recycle (GameObject X)
	{	
		X.GetComponent<Rigidbody> ().velocity = Vector3.zero;
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

	public void buttonPew ()
	{
		GameObject target = NewPlataform;
		foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
			float mX = monster.transform.position.x;
			float pX = Player.transform.position.x;
			float tX = target.transform.position.x;
			if (monster.transform.position.y > 0f && mX > pX && mX - pX < tX - pX) {
				target = monster;
			}
		}
		if (target != NewPlataform) {
			GameObject pew = Instantiate (Resources.Load ("Prefabs/characters/Pew") as GameObject, Player.transform.position, Quaternion.identity) as GameObject;
			PlataformGenerator.Player.GetComponent<PlayerController> ().laserSound ();
			pew.gameObject.GetComponent<Pew> ().target = target;
			Debug.Log (pew.gameObject.GetComponent<Pew> ().target.gameObject.GetComponent<Monster> ().HP);
		}
	}
}