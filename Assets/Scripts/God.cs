using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class God : MonoBehaviour {

	public static string world;
	public static TextAsset level;
	public static TextAsset cenario;

	void Awake() {
		// Do not destroy this game object:
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void LoadLevel(string level) {
		GameObject loading = Instantiate (Resources.Load ("Prefabs/UI/Loading") as GameObject);
		loading.transform.SetParent (GameObject.Find("Canvas").transform,false);
		SceneManager.LoadSceneAsync(level);
	}

	public static void changeSkin(string skinName){
		PlayerPrefs.SetString ("Skin", skinName);
	}

	public static void GameOver(string winner)	{
		PlayerPrefs.SetString("Winner", winner);
		SceneManager.LoadSceneAsync("GameOver");
	}
}