﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuBts : MonoBehaviour
{
	public string scenePlay;

	public void Jogar ()
	{
		SceneManager.LoadScene (scenePlay);
	}

	public void OpenPanel (string panel){
		GameObject.Find (panel).SetActive (true);
	}

	public void Home (){
		GameObject.Find ("Loja").SetActive (false);	
		GameObject.Find ("Pause").SetActive (false);
		GameObject.Find ("Opções").SetActive (false);
		GameObject.Find ("Tutorial").SetActive (false);
		GameObject.Find ("Créditos").SetActive (false);
		GameObject.Find ("Controles").SetActive (false);
	}
}
