using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Abl : MonoBehaviour {
	[Header("Player Stats")]
	public int AblPoints = 0;
	[Header("Abl list")]
	public List<string> PlayersAbls = new List<string> (); 
	public List<GameObject> AblGOs = new List<GameObject>();
	[Header("Abl Cooldown List")]
	public bool scrachCD = false;
	public bool healingCD = false;
	[Header("Common GO's")]
	public GameObject AblSlotOneGO;
	public GameObject AblSlotTwoGO;
	public GameObject BadGO;
	public GameObject _CoreGO;
	//public Image AblSlotOne;
	public Slider Bad;
	[Header("Common Pic's")]
	public List<Sprite> AblSprites = new List<Sprite> ();
	public List<Image> AblImages = new List<Image> ();
	[Header("TimeStats")]
	public int Ticks = 0;
	public int sec = 0;
	public int min = 0;
	bool _30sCDUsed = false;
	[Header("Abl vars")]
	public int Healing_TimeOfEffect = 10;
	public int Healing_AmountHealedPerTick = 1;
	public int Scrach_Damage = 20;


	IEnumerator AblScrach()
	{
		//AblGOs [0].SetActive (false);
		int Value = Random.Range (0, AblGOs.Count-1);
		int Value2 = 0;
		//int Buff = 2;
		//int Debuff = 4;
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		scrachCD = true;
		Bad.value = Bad.value - Scrach_Damage;
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Scrach_Damage;
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Scrach_Damage;
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Scrach_Damage;
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Scrach_Damage;
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		// turn off all!
		foreach (var go in AblGOs) {
			go.SetActive (false);
		}
	}
	IEnumerator healing() {
	// nothing for now.
		int Value = Random.Range (0, AblGOs.Count-1);
		int Value2 = 0;
		//int Buff = 2;
		//int Debuff = 4;
		int WaitTime = Mathf.RoundToInt (Healing_TimeOfEffect / 4);
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		scrachCD = true;
		_CoreGO.GetComponent<_Core> ().Hp.value = _CoreGO.GetComponent<_Core> ().Hp.value + Healing_AmountHealedPerTick;
		yield return new WaitForSeconds(WaitTime); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		_CoreGO.GetComponent<_Core> ().Hp.value = _CoreGO.GetComponent<_Core> ().Hp.value + Healing_AmountHealedPerTick;
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		_CoreGO.GetComponent<_Core> ().Hp.value = _CoreGO.GetComponent<_Core> ().Hp.value + Healing_AmountHealedPerTick;
		yield return new WaitForSeconds(WaitTime); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		_CoreGO.GetComponent<_Core> ().Hp.value = _CoreGO.GetComponent<_Core> ().Hp.value + Healing_AmountHealedPerTick;
		yield return new WaitForSeconds(WaitTime); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		//Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		_CoreGO.GetComponent<_Core> ().Hp.value = _CoreGO.GetComponent<_Core> ().Hp.value + Healing_AmountHealedPerTick;
		yield return new WaitForSeconds(WaitTime); 
		AblGOs[Value].SetActive(true);
		// turn off all!
		foreach (var go in AblGOs) {
			go.SetActive (false);
		}
	}
	public void LvUp(int AmountToGive) {
		AblPoints = AblPoints + AmountToGive;
	}
	public void UnlockAbl(string Abl,int Cost){
		Abl = Abl.ToLower ();
		if (AblPoints >= Cost) {
			AblPoints = AblPoints - Cost;
			PlayersAbls.Add (Abl);

		}
	}
	public void UseAbl(string AblName){
		if(PlayersAbls.Contains(AblName))
		{
			// we have the abl...
			if (AblName == "scrach") {
				if (scrachCD != true) {
					foreach (var Ima in AblImages) {
						Ima.sprite = AblSprites [0];
					}
					StartCoroutine ("AblScrach");
					AblSlotOneGO.SetActive (false);
				}
			}
			if (AblName == "healing") {
				if(healingCD != true){
					foreach (var Ima in AblImages) {
						Ima.sprite = AblSprites [1];
					}
					StartCoroutine("healing");
					AblSlotTwoGO.SetActive (false);
				}
			}
		}
	}
	public void ResetSkills(){ // NO UNLOCK!
		scrachCD = false;
		healingCD = false;
		//AblSlotOneGO.SetActive (true);
	}
	public void LockSkills() {
		scrachCD = true;
		healingCD = true;
		AblSlotOneGO.SetActive (false);
		AblSlotTwoGO.SetActive (false);
	}
	public void Update() {
		Ticks++;
		if (Ticks > 60) {
			sec++;
			Ticks = 0;
		}
		if (sec > 60) {
			min++;
			sec = 0;
		}
		if (min >= 60) {
			min = 0; // call all resets!
			Debug.Log("CD Reset here!");
			scrachCD = false;
		}
		//================================
		if (sec >= 59 ) {
		// reset all 1m cooldowns!
			_30sCDUsed = false;
		}
		if (sec >= 30) {
			if (_30sCDUsed != true) {
				if(PlayersAbls.Contains("scrach"))
				{
					scrachCD = false;
					AblSlotOneGO.SetActive (true);
				}
				if (PlayersAbls.Contains ("healing")) {
					healingCD = false;
					AblSlotTwoGO.SetActive (true);
				}
				_30sCDUsed = true;
			}
		}
	}
}
