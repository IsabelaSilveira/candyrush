using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGenerator: MonoBehaviour {
	GameObject LastMonster;
	System.Random random = new System.Random();

	void Start()
	{
		StartCoroutine("MyEvent");
	}
	 
	private IEnumerator MyEvent()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.4f); // wait
			if(Mathf.Sin(Time.time) > Mathf.Sin(Cam.time)){
				LastMonster = Instantiate(Resources.Load("Prefabs/characters/Monster"+random.Next(1,5).ToString()) as GameObject, PlataformGenerator.EndPosition + new Vector3(-1f, 5f, 0.5f),Quaternion.identity) as GameObject;
			}
		}
	}
	
	void Update()
	{
		if (!PlataformGenerator.GameOver.activeSelf) {
			try {	

				foreach (var monster in GameObject.FindGameObjectsWithTag("Monster")) {
					monster.transform.Translate (-PlataformGenerator.speed * Time.deltaTime * 1.25f, 0, 0);

					if (monster.transform.position.x < -35f || monster.transform.position.y < -20f) {
						Score.died += 1;
						Destroy (monster);
					}
				} 

			} catch (MissingReferenceException) {
			}
		} else {
			StopCoroutine ("MyEvent");
		}
	}
}
