using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Decider : MonoBehaviour
{
	public GameObject coin;
	public string choice;
	public string caraCoroa;
	private bool decided = false;
	public GameObject CCpanel;

	// Update is called once per frame
	void Update ()
	{
		if (coin.activeSelf && coin.GetComponent<CoinSpin> ().result != "") {
			if (coin.GetComponent<CoinSpin> ().result == caraCoroa) {
				PlayerPrefs.SetString ("Mode", choice);
			} else {
				if (choice == "Walker") {
					PlayerPrefs.SetString ("Mode", "Meddler");
				} else {
					PlayerPrefs.SetString ("Mode", "Walker");
				}
			}
			if (!decided)
				StartCoroutine (QWER ());
		}
	}

	private IEnumerator QWER ()
	{
		decided = true;
		GameObject loading = Instantiate (Resources.Load ("loading") as GameObject);
		loading.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);
		loading.transform.localRotation = Quaternion.identity;
		loading.transform.localPosition = Vector2.zero;
		loading.transform.localScale = Vector2.one;
		yield return new WaitForSeconds (1);
		SceneManager.LoadSceneAsync ("qwer");

	}

	public void CaraCoroa (string cc)
	{
		caraCoroa = cc;
	}

	public void Choice (string c)
	{
		choice = c;
		CCpanel.SetActive (true);
	}
}