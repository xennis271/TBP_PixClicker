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
	[Header("Common GO's")]
	public GameObject AblSlotOneGO;
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



	IEnumerator AblScrach()
	{
		//AblGOs [0].SetActive (false);
		int Value = Random.Range (0, AblGOs.Count-1);
		int Value2 = 0;
		int Buff = 2;
		int Debuff = 4;
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		scrachCD = true;
		Bad.value = Bad.value - Mathf.RoundToInt(BadGO.GetComponent<badguy>().DamPerHit*Buff + _CoreGO.GetComponent<_Core>().Zone/Debuff);
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Mathf.RoundToInt(BadGO.GetComponent<badguy>().DamPerHit*Buff+ _CoreGO.GetComponent<_Core>().Zone/Debuff);
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Mathf.RoundToInt(BadGO.GetComponent<badguy>().DamPerHit*Buff+ _CoreGO.GetComponent<_Core>().Zone/Debuff);
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Mathf.RoundToInt(BadGO.GetComponent<badguy>().DamPerHit*Buff+ _CoreGO.GetComponent<_Core>().Zone/Debuff);
		yield return new WaitForSeconds(0.5f); 
		AblGOs[Value].SetActive(true);
		Value = Random.Range (0, AblGOs.Count-1);
		Debug.Log ("Value1:" + Value + " Value2:" + Value2);
		Bad.value = Bad.value - Mathf.RoundToInt(BadGO.GetComponent<badguy>().DamPerHit*Buff+ _CoreGO.GetComponent<_Core>().Zone/Debuff);
		yield return new WaitForSeconds(0.5f); 
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
		}
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
					AblGOs [0].SetActive (true);
					AblSlotOneGO.SetActive (true);
				}
				_30sCDUsed = true;
			}
		}
	}
}
