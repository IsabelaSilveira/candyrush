using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameMaster : NetworkBehaviour
{
	public static bool active = false;
	public Button[] buttons;
	[SyncVar]
	public int obstaculos = 44;
	public static GameMaster singleton;
	public Text countText;
	private int monster = -1;
	private string powerUp = "";
	private bool dropPlataforma = false;
	private bool barracaDoce = false;

	// Start is called before the first frame update
	void Start ()
	{
		singleton = this;
		if (!isServer)
			foreach (Button button in buttons) {
				attribRandom (button);
			}
	}

	// Update is called once per frame
	void Update ()
	{
		if (obstaculos <= 0) {
			PlataformGenerator.GameOver.SetActive (true);
			PlataformGenerator.GameOver.transform.rotation = Quaternion.Euler (new Vector3 (0f, 90f, 0f));
			PlataformGenerator.GameOver.transform.position = new Vector3 (PlataformGenerator.GameOver.transform.position.x, PlataformGenerator.GameOver.transform.position.y, -7.2f);
		}
		if (barracaDoce) {
			RpcBarracaDoce ();
			barracaDoce = false;
		}
		if (dropPlataforma) {
			RpcDropPlataform ();
			dropPlataforma = false;
		}
		if (monster != -1) {
			RpcSpawnMonster (monster);
			monster = -1;
		}
		if (powerUp != "") {
			RpcSpawnPowerUp (powerUp);
			powerUp = "";
		}
	}

	[ClientRpc]
	public void RpcSpawnMonster (int n)
	{
		foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
			if ((plataforma.transform.position.x > 60 && plataforma.transform.position.x < 60 + PlataformGenerator.speed / 2) && plataforma.name.StartsWith ("Plat")) { 
				NetworkServer.Spawn (Instantiate (Resources.Load ("Prefabs/characters/Monster" + n) as GameObject, PlataformGenerator.EndPosition + new Vector3 (-3f, 5f, 0f), Quaternion.identity) as GameObject);
				break;
			}
		}
	}

	[ClientRpc]
	public void RpcSpawnPowerUp (string powerUp)
	{
		foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
			if ((plataforma.transform.position.x > 60 && plataforma.transform.position.x < 60 + PlataformGenerator.speed / 2) && plataforma.name.StartsWith ("Plat")) {
				NetworkServer.Spawn (Instantiate (Resources.Load ("Prefabs/plataforms/Power Up " + powerUp) as GameObject, PlataformGenerator.EndPosition + new Vector3 (-3f, 5f, 0f), Quaternion.identity) as GameObject);
				break;
			}
		}
	}

	[ClientRpc]
	public void RpcDropPlataform ()
	{
		foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
			if ((plataforma.transform.position.x > 30 && plataforma.transform.position.x < 30 + PlataformGenerator.speed / 2) && plataforma.name.StartsWith ("Plat")) {
				//Destroy (plataforma.gameObject);
				plataforma.GetComponent<BoxCollider> ().enabled = false;
				var ChildCount = plataforma.transform.childCount;
				if (plataforma.gameObject.GetComponent<Rigidbody> () != null) {
					plataforma.gameObject.GetComponent<Rigidbody> ().useGravity = true;
					plataforma.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.down);
					plataforma.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
					plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);
				} else if (plataforma.gameObject.GetComponent<Rigidbody> () == null) {
					plataforma.gameObject.AddComponent<Rigidbody> ();
					plataforma.gameObject.GetComponent<Rigidbody> ().freezeRotation = true;
					plataforma.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, plataforma.GetComponent<Rigidbody> ().velocity.y, 0f);
				}
				break;
			}
		}
	}

	[ClientRpc]
	public void RpcBarracaDoce ()
	{
		foreach (var plataforma in GameObject.FindGameObjectsWithTag("Plataforma")) {
			if ((plataforma.transform.position.x > 60 && plataforma.transform.position.x < 60 + PlataformGenerator.speed / 2) && plataforma.name.StartsWith ("Plat")) {
				NetworkServer.Spawn (Instantiate (Resources.Load ("Prefabs/plataforms/Barraca doces"), plataforma.transform.position + new Vector3 (0f, 4.8f, 1f), Quaternion.identity) as GameObject);
				break;
			}
		} 
	}

	[Command]
	public void CmdSpawnMonster (int n)
	{

		monster = n;
	}

	[Command]
	public void CmdSpawnPowerUp (string powerUp)
	{
		powerUp = powerUp;
	}

	[Command]
	public void CmdDropPlataform ()
	{
		dropPlataforma = true;
	}

	[Command]
	public void CmdBarracaDoce ()
	{
		barracaDoce = true;
	}

	[Command]
	private void CmdUpdateObstacle (){
		obstaculos--;
	}

	private void attribRandom (Button b)
	{
		if (!isServer) {
			//NetworkServer.Spawn (b.gameObject);
			CmdUpdateObstacle ();
			countText.text = "" + obstaculos;
			if (obstaculos < 4) {
				Destroy (b.gameObject);
				return;
			}
			Image img = b.gameObject.GetComponent<Image> ();
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (() => attribRandom (b));
			Texture2D[] textures = { null, null, null, null, null, null };
			textures [0] = Resources.Load ("Icons_barraca") as Texture2D;
			textures [1] = Resources.Load ("Icons_fantasma") as Texture2D;
			textures [2] = Resources.Load ("Icons_mola") as Texture2D;
			textures [3] = Resources.Load ("Icons_estrela") as Texture2D;
			textures [4] = Resources.Load ("Icons_foguete") as Texture2D;
			textures [5] = Resources.Load ("Icons_tile") as Texture2D;
			Sprite spr;
			switch (Random.Range (0, 6)) {
			case 0:
				b.onClick.AddListener (() => CmdBarracaDoce ());
				spr = Sprite.Create (textures [0], new Rect (0f, 0f, textures [0].width, textures [0].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			case 1:
				b.onClick.AddListener (() => CmdSpawnMonster (0));
				spr = Sprite.Create (textures [1], new Rect (0f, 0f, textures [1].width, textures [1].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			case 2:
				b.onClick.AddListener (() => CmdSpawnPowerUp ("Jump"));
				spr = Sprite.Create (textures [2], new Rect (0f, 0f, textures [2].width, textures [2].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			case 3:
				b.onClick.AddListener (() => CmdSpawnPowerUp ("Shield"));
				spr = Sprite.Create (textures [3], new Rect (0f, 0f, textures [3].width, textures [3].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			case 4:
				b.onClick.AddListener (() => CmdSpawnPowerUp ("Speed"));
				spr = Sprite.Create (textures [4], new Rect (0f, 0f, textures [4].width, textures [4].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			case 5:
				b.onClick.AddListener (() => CmdDropPlataform ());
				spr = Sprite.Create (textures [5], new Rect (0f, 0f, textures [5].width, textures [5].height), new Vector2 (0.5f, 0.5f));
				img.sprite = spr;
				break;
			default:
				break;
			}
		}
	}
}