using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _Core : MonoBehaviour {
	[Header("Stats LIVE")]
	public int Gold;
	public Slider Hp; // gets rounded
	public int Lv;
	public int Zone;
	public int Deaths;
	public Slider EXPbar;
	public int AmountWeTook = 0;
	[Header("GO's")]
	public Slider Bar;
	public GameObject Bad;
	public GameObject StartGO;
	public GameObject GameStart;
	[Header("List")]
	public List<Sprite> MobsNormal = new List<Sprite>();
	public List<Sprite> MobsAttack = new List<Sprite>();
	public List<Sprite> MobsHurt = new List<Sprite>();
	public List<Sprite> MobsAbl = new List<Sprite>();
	public List<string> MobAbls = new List<string> ();
	[Header("UI stuff")]
	public GameObject ShopGO;
	public List<GameObject> Pages = new List<GameObject> ();
	public int PageNum;
	[Header("Text for stats")]
	public Text GoldT;
	public Text HpT;
	public Text LvT;
	public Text ZoneT;
	public Text DeathsT;


	public void Removehp(float amount){
		Bar.value = Bar.value - amount;
	
	}
	public void Heal() {
		Bar.value = Bar.maxValue;
	}

	public void SummonNewBad() {
		Bad.SetActive (false);
		// update setings...
		int outp;
		outp = Random.Range(0,MobsNormal.Count);
		//Debug.Log (outp);
		Bad.GetComponent<badguy> ().NormalPic = MobsNormal [outp];
		Bad.GetComponent<badguy> ().AttackingPic= MobsAttack [outp];
		Bad.GetComponent<badguy> ().DamagedPic = MobsHurt [outp];
		Bad.GetComponent<badguy> ().AblPic = MobsAbl [outp];
		string outs = "";
		outs = MobAbls[outp];
		if (outs == "Invis") {
			Bad.GetComponent<badguy> ().Invs = true;
		} else {
			Bad.GetComponent<badguy> ().Invs = false;
		}
		if (outs == "Rage") {
			Bad.GetComponent<badguy> ().Rage = true;
		} else {
			Bad.GetComponent<badguy> ().Rage = false;
		}
		if (outs == "Pos") {
			Bad.GetComponent<badguy> ().Pos = true;
		} else {
			Bad.GetComponent<badguy> ().Pos = false;
		}
		Bad.GetComponent<badguy> ().BadHpBar.value = Bad.GetComponent<badguy> ().BadHpBar.maxValue;
		Bad.GetComponent<badguy> ().Damage = 1;
		Bad.SetActive (true);
	}
	public void OpenShop() {
	// need to summon a new dude after closing it
		PageNum = -1; // the list starts @ 0
		ShopGO.SetActive (true);
	}
	public void CloseShop(){
		PageNum = -1; // the list starts at 0
		ShopGO.SetActive (false);
	}
	public void UpdatePages() {
		// we need to close all pages...
		foreach (GameObject Page in Pages) {
			Page.SetActive (false);
		}
		// open the one we want.
		Pages [PageNum].SetActive (true);
	}
	public void TurnPage() {
		PageNum++;
		UpdatePages ();
	}
	public void TurnBackAPage() {
		PageNum--;
		UpdatePages ();
	}
	public void UpdateStats() {
		GoldT.text = Gold.ToString();
		HpT.text = Mathf.RoundToInt(Hp.value).ToString(); // looks better!
		LvT.text = Lv.ToString();
		ZoneT.text = Zone.ToString();
		DeathsT.text = Deaths.ToString();
		if (Hp.value <= 0) { // adds to the death pool but no math yet...
		
			Hp.value = 100;
			HpT.text = Hp.ToString();
			Deaths++;
			DeathsT.text = Deaths.ToString();
			// Money and math time!

			AmountWeTook = (AmountWeTook*2) + (Deaths*10);
			//Debug("Amount you had" + Gold.ToString() + " The amount we took" +AmountWeTook.ToString()); 
			Gold = Gold - AmountWeTook; // << thats one mean mean line!
			if (Gold < 0) {
				Gold = 0;
			}
		
		}
		if(EXPbar.value == EXPbar.maxValue){
			EXPbar.value = EXPbar.minValue;
			Lv++;
			EXPbar.maxValue = EXPbar.maxValue * 2; // trying to grow!
		}
	}
	int PreZone;
	public void KilledMob() {
		Zone++;
		PreZone++;
		Gold = Gold + Zone * Zone / 2;
		// Nice job!
		// need to give some EXP
		EXPbar.value = EXPbar.value + Zone/2;
		if (PreZone == 10) {
			EXPbar.maxValue = Zone * 2.5f + EXPbar.maxValue;
		}
	}
	public void Update(){
		UpdateStats ();
	}
	public void Start(){
		AmountWeTook = 1;
		StartGO.SetActive (true);
		GameStart.SetActive (false);
	}
	public void StartGame() {
		GameStart.SetActive (true);
		StartGO.SetActive (false);
	}
}
