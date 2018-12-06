using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEditor;

public class PlataformGenerator: NetworkBehaviour
{

	public Transform StartPlataformGenerator;
	public static float speed = 1f;
	public static GameObject GameOver;
	GameObject NewPlataform;
	GameObject NewBackground;
	public static GameObject Player;
	System.Random random = new System.Random ();
	public static PlataformGenerator singleton;

	int ChildCount;
	public static Vector3 EndPosition;
	public static Vector3 BgEndPosition;
	Vector3 EndPosAdjust = new Vector3 (2.2f, 0, 0);

	private int bg = 0;

	void Awaken ()
	{
		if (!isServer) {
			NetworkServer.Spawn (Instantiate (Resources.Load ("God") as GameObject));
			NetworkServer.Spawn (Instantiate (Resources.Load ("StartPlataformGenerator") as GameObject));
		} else {
			if (singleton) {
				foreach (PlataformGenerator obj in GameObject.FindObjectsOfType<PlataformGenerator>()) {
					Destroy (this.gameObject);
				}
			} else {
				foreach (PlataformGenerator obj in GameObject.FindObjectsOfType<PlataformGenerator>()) {
					singleton = obj;
					break;
				}
			}
		}
	}

	void Start ()
	{
		StartPlataformGenerator = GameObject.Find ("StartPlataformGenerator").transform;
		if (isServer) {
			NetworkServer.Spawn (NewPlataform = Instantiate (Resources.Load ("Prefabs/plataforms/Plat1") as GameObject, StartPlataformGenerator.position, Quaternion.identity) as GameObject);
			NetworkServer.Spawn (NewBackground = Instantiate (Resources.Load ("Prefabs/plataforms/Bg1") as GameObject, StartPlataformGenerator.position, Quaternion.identity) as GameObject);
			NetworkServer.Spawn (Player = Instantiate (Resources.Load ("Prefabs/characters/" + PlayerPrefs.GetString ("Skin", "Player1")) as GameObject, StartPlataformGenerator.position + new Vector3 (0, 5f, 0f), Quaternion.identity) as GameObject);
		}
		GameOver = GameObject.Find ("GameOver");
		GameOver.SetActive (false);
		speed = 10f;
	}

	void Update ()
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
					int n = 1;
					if (findRecycle ("Plat" + n.ToString () + "(Clone)")) {
						NewPlataform = findRecycle ("Plat" + n.ToString () + "(Clone)");
						NewPlataform.tag = "Plataforma";
						NewPlataform.transform.position = EndPosition;
					} else if (isServer)
						NetworkServer.Spawn (NewPlataform = Instantiate (newPlataform (n), EndPosition, Quaternion.identity) as GameObject);
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
					Debug.Log (n);
					if (findRecycle ("Bg" + n.ToString () + "(Clone)")) {
						NewBackground = findRecycle ("Bg" + n.ToString () + "(Clone)");
						//PrefabUtility.RevertPrefabInstance(NewPlataform);
						NewBackground.tag = "Plataforma";
						NewBackground.transform.position = BgEndPosition;
					} else if (isServer)
						NetworkServer.Spawn (NewBackground = Instantiate (newBackground (n), BgEndPosition, Quaternion.identity) as GameObject);
				}

				foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
					if (!(plataforma.transform.position.x == 666 && plataforma.transform.position.y == 666 && plataforma.transform.position.z == 666))
						plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (-speed, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);

					if (plataforma.transform.position.x < -90f || plataforma.transform.position.y < -20f) {
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
		} else
			foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
				if (plataforma.gameObject.GetComponent<Rigidbody> () != null) {
					plataforma.gameObject.GetComponent<Rigidbody> ().useGravity = true;
					plataforma.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
					plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);
				} else if (plataforma.gameObject.GetComponent<Rigidbody> () == null) {
					plataforma.gameObject.AddComponent<Rigidbody> ();
					plataforma.gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
					plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);
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
			GameObject pew;
			NetworkServer.Spawn (pew = Instantiate (Resources.Load ("Prefabs/characters/Pew") as GameObject, Player.transform.position, Quaternion.identity) as GameObject);
			pew.gameObject.GetComponent<Pew> ().target = target;
			PlataformGenerator.Player.GetComponent<PlayerController> ().laserSound ();
		}
	}
}