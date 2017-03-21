using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Note this script is _core but is here to lower the amount of code _core has to deal with
// right now 3/6/2017 _core has 600~ lines of code the goal here is to keep that around 1,000.
// thus enter _core2 just to help with the space.

// PLEASE NOTE THAT ALL METHODS ARE HERE TO BE CALLED AND NONE RUN ON START OR UPDATE!
public class _Core2 : MonoBehaviour {
	[Header("Main Go's")]
	public GameObject _Core;
	public GameObject Abl;
	[Header("Player Inv")]
	public int AmountOfHealingPots = 0;
	public int AmountOfManaPots = 0;
	public int AmountOfTimePots = 0;
	[Header("Player InvGO's")]
	public GameObject HpGO;
	public GameObject ManaGO;
	public GameObject TimeGO;
	[Header("UI/Beast Go's")]
	public GameObject Beast_Start;
	public GameObject Beast_Holder;
	public Text Title;
	public Text Des;
	public bool StartUsed = false; // after this we have made the beast... and DONT need to remake it.
	public bool BeastMade = false;
	public Image Pic;


	public void DrinkHpPot() {
		// check player
		if(AmountOfHealingPots > 0) {
			HpGO.SetActive (false);
			AmountOfHealingPots--;
			_Core.GetComponent<_Core> ().Hp.value = _Core.GetComponent<_Core> ().Hp.maxValue;
		}
		if(AmountOfHealingPots > 0) {
			HpGO.SetActive (true);
		}
	}
	public void DrinkManaPot() {
		if(AmountOfManaPots > 0) {
			ManaGO.SetActive (false);
			AmountOfManaPots--;
			if(Abl.GetComponent<Abl>().PlayersAbls.Contains("scrach"))
			{
				Abl.GetComponent<Abl>().scrachCD = false;
				Abl.GetComponent<Abl>().AblSlotOneGO.SetActive (true);
			}
			if (Abl.GetComponent<Abl>().PlayersAbls.Contains ("healing")) {
				Abl.GetComponent<Abl>().healingCD = false;
				Abl.GetComponent<Abl>().AblSlotTwoGO.SetActive (true);
				//GameObject.Find ("_Core").GetComponent<_Core> ().Hp.value = 1;
			}
		}
		if(AmountOfManaPots > 0) {
			ManaGO.SetActive (true);
		}
	}
	public void DrinkTimePot() {
		if(AmountOfTimePots > 0) {
			TimeGO.SetActive (false);
			AmountOfTimePots--;
			_Core.GetComponent<_Core> ().SummonTime ();
		}
		if(AmountOfTimePots > 0) {
			TimeGO.SetActive (true);
		}
	}
	public void MakeBeast() {
	// we are going to make a button for every monster
	// we have the 1st button made
		if (BeastMade == false) {
			int Count = -1; // WE have a 0
			foreach (string monster in _Core.GetComponent<_Core>().MobName) {
				Count++;
				if (StartUsed == false) {
					StartUsed = true;
					Beast_Start.GetComponentInChildren<Text> ().text = monster;
					Beast_Start.SetActive (true);
					Beast_Start.name = "0";

				} else {
					GameObject NewBut = Instantiate (Beast_Start);
					NewBut.transform.SetParent (Beast_Holder.transform);
					float NewY = (NewBut.transform.position.y - ((30 * (Count + 1)) * 2)) - 300;
					NewBut.name = (Count).ToString ();
					NewBut.transform.position = new Vector3 (Beast_Start.transform.position.x, NewY, Beast_Start.transform.position.z);

					NewBut.GetComponentInChildren<Text> ().text = monster;
			
			
			
				}
			}
		}
		BeastMade = true;
	}
	public void UpdateText(GameObject name) {
		// we need the data
		int value = 0;
		int.TryParse (name.name, out value);
		Title.text = _Core.GetComponent<_Core>().MobName[value];
		Des.text = _Core.GetComponent<_Core>().MobDetail[value];
		Pic.sprite = _Core.GetComponent<_Core> ().MobsNormal [value];
	}
}
