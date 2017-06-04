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
	public GameObject Bad;
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
	public bool BossAddonDone = false;
	public Image Pic;
	public int Count = -1;
	[Header("Boss Trigers")]
	public bool SlimeSummoned = false;
	[Header("Slime Kings Skins")]
	public List<Sprite> SlimeKingSkins = new List<Sprite>();


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
			//int YValue = 0;
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
		if (Count >= (_Core.GetComponent<_Core>().MobName.Count - 1)) {
			foreach (string boss in _Core.GetComponent<_Core>().BossNames) {
					Count++;
					GameObject NewBut = Instantiate (Beast_Start);
					NewBut.transform.SetParent (Beast_Holder.transform);
					float NewY = (NewBut.transform.position.y - ((30 * (Count + 1)) * 2)) - 300;

					NewBut.name = "Boss#" + (Count).ToString ();
					NewBut.transform.position = new Vector3 (Beast_Start.transform.position.x, NewY, Beast_Start.transform.position.z);

					NewBut.GetComponentInChildren<Text> ().text = boss;




			}
		}

		BossAddonDone = true;
	}
	public void UpdateText(GameObject name) {
		// we need the data
		int value = 0;
		bool Boss = false;
		int.TryParse (name.name, out value);
		string[] TempNames = name.name.Split ('#');
		foreach (var sname in TempNames) {
			Debug.Log (sname);
			if (sname == "Boss") { // we are dealing with a boss
				Boss = true; // that means skip one...
			} else {
				if (Boss) {
					int.TryParse (sname, out value);
				}
			}
		}
		if (Boss) {
			value = value - _Core.GetComponent<_Core>().MobName.Count;
			Title.text = _Core.GetComponent<_Core> ().BossNames [value];
			Des.text = _Core.GetComponent<_Core> ().BossDetails [value];
			Pic.sprite = _Core.GetComponent<_Core> ().BossNormal [value];
		
		} else {
			Title.text = _Core.GetComponent<_Core> ().MobName [value];
			Des.text = _Core.GetComponent<_Core> ().MobDetail [value];
			Pic.sprite = _Core.GetComponent<_Core> ().MobsNormal [value];
		}
	}
	public void SummonSlimeKing ()
	{
		if (SlimeSummoned != true && _Core.GetComponent<_Core>().SlimeKingStage <= 0) {
			// tell bad
			_Core.GetComponent<_Core>().SlimeKingStage += 1; // allow for death growth.
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 1000000; // insane hp for this lv
			badApi.BadHpBar.value = 1000000;
			badApi.Damage = badApi.Damage * 100; // grow...
			badApi.Adv_Grow = true; // and block as well
			badApi.Healing = true;
			SlimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = SlimeKingSkins [0];
			Bad.GetComponent<badguy> ().AttackingPic = SlimeKingSkins [1];
			Bad.GetComponent<badguy> ().DamagedPic = SlimeKingSkins [2];
			Bad.GetComponent<badguy> ().AblPic = SlimeKingSkins [0];

		}
	}
	public void SummonSlimeKingStageTwo() {
		// tell bad
		if (_Core.GetComponent<_Core> ().SlimeKingStage == 2) {
			_Core.GetComponent<_Core>().SlimeKingStage += 1; // allow for death growth.
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 1000000/2; // insane hp for this lv
			badApi.BadHpBar.value = 1000000/2;
			badApi.Damage = badApi.Damage * 100/2; // grow...
			badApi.Adv_Grow = true; // and block as well
			badApi.Healing = true;
			SlimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = SlimeKingSkins [3];
			Bad.GetComponent<badguy> ().AttackingPic = SlimeKingSkins [4];
			Bad.GetComponent<badguy> ().DamagedPic = SlimeKingSkins [5];
			Bad.GetComponent<badguy> ().AblPic = SlimeKingSkins [3];

		}
	}
	public void SummonSlimeKingStageThree() {
		// tell bad
		if (_Core.GetComponent<_Core> ().SlimeKingStage == 3) {
			_Core.GetComponent<_Core>().SlimeKingStage += 1; // allow for death growth.
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 1000000/3; // insane hp for this lv
			badApi.BadHpBar.value = 1000000/3;
			badApi.Damage = badApi.Damage * 100/3; // grow...
			badApi.Adv_Grow = true; // and block as well
			badApi.Healing = true;
			SlimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = SlimeKingSkins [6];
			Bad.GetComponent<badguy> ().AttackingPic = SlimeKingSkins [7];
			Bad.GetComponent<badguy> ().DamagedPic = SlimeKingSkins [8];
			Bad.GetComponent<badguy> ().AblPic = SlimeKingSkins [6];

		}
	}
	public void SummonSlimeKingStageFour() {
		// tell bad
		if (_Core.GetComponent<_Core> ().SlimeKingStage == 4) {
			_Core.GetComponent<_Core>().SlimeKingStage += 1; // allow for death growth.
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 1000000/4; // insane hp for this lv
			badApi.BadHpBar.value = 1000000/4;
			badApi.Damage = badApi.Damage * 100/4; // grow...
			badApi.Adv_Grow = true; // and block as well
			badApi.Healing = true;
			SlimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = SlimeKingSkins [9];
			Bad.GetComponent<badguy> ().AttackingPic = SlimeKingSkins [10];
			Bad.GetComponent<badguy> ().DamagedPic = SlimeKingSkins [11];
			Bad.GetComponent<badguy> ().AblPic = SlimeKingSkins [12];

		}
	}
	public void KilledSlimeKing() {
		_Core.GetComponent<_Core>().Gold += 100*2;
		_Core.GetComponent<_Core>().EXPbar.value += 50*2;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = 200*2;
			Bad.GetComponent<badguy> ().Boss = false;
		_Core.GetComponent<_Core>().SummonNewBad ();
		}
	}
