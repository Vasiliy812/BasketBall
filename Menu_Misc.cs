using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Misc : MonoBehaviour {

	public int LastMenuID = 0;
	public Text BestResult;
	void Start()
	{
		BestResult.text = "Лучший результат: " + GameObject.Find("XMLManager").GetComponent<XML>().GetParam("params", "HiScore");
	}

}
