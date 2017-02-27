using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class _Core : MonoBehaviour {
	[Header("Game Id#")]
	public string GameID = "None";
	[Header("Stats LIVE")]
	public int Gold;
	public Slider Hp; // gets rounded
	public int Lv;
	public int Zone;
	public int Deaths;
	public Slider EXPbar;
	public int AmountWeTook = 0;
	public static int key = 921;
	[Header("GO's")]
	public Slider Bar;
	public GameObject Bad;
	public GameObject StartGO;
	public GameObject GameStart;
	public GameObject AblGO;
	[Header("AblButtons")]
	public GameObject scrachGO;
	[Header("List")]
	public List<Sprite> MobsNormal = new List<Sprite>();
	public List<Sprite> MobsAttack = new List<Sprite>();
	public List<Sprite> MobsHurt = new List<Sprite>();
	public List<Sprite> MobsAbl = new List<Sprite>();
	public List<string> MobAbls = new List<string> ();
	public Dictionary<string,string> playerStats = new Dictionary<string, string> ();
	[Header("UI stuff")]
	public GameObject ShopGO;
	public GameObject CloseBut;
	public List<GameObject> Pages = new List<GameObject> ();
	public int PageNum;
	public Text OutPut;
	public Text WelcomeText;
	[Header("Text for stats")]
	public Text GoldT;
	public Text HpT;
	public Text LvT;
	public Text ZoneT;
	public Text DeathsT;
	[Header("Dev Bools")]
	public bool AllowEditingHp = false;
	public bool AllowLocalSaving = true;
	public string PathForSaving = "";
	public string DatabaseName = "";
	public string DatabaseExt = ".DBv1";
	[Header("Store Stuff")]
	public bool ClickerUpgrade;
	public GameObject ClickerUpgradeGO;
	public Text ClickerButton;

	public static string EncryptDecrypt(string TextToEncrypt){
		StringBuilder inSb = new StringBuilder(TextToEncrypt);
		StringBuilder outSb = new StringBuilder(TextToEncrypt.Length);
		char c;
		for (int i = 0; i < TextToEncrypt.Length; i++) {
			c = inSb [i];
			c = (char)(c ^ key);
			outSb.Append (c);
		}
		return outSb.ToString();
	}
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
		//Zone++; Handled by badguy now...
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
		if (outs == "Healing") {

			Bad.GetComponent<badguy> ().Healing = true;
		} else {
			Bad.GetComponent<badguy> ().Healing = false;
		}
		if (outs == "Invis&Healing") {
			Bad.GetComponent<badguy> ().Invs = true;
			Bad.GetComponent<badguy> ().Healing = true;
		}
		Bad.GetComponent<badguy> ().BadHpBar.value = Bad.GetComponent<badguy> ().BadHpBar.maxValue;
		Bad.GetComponent<badguy> ().Damage = 1;
		Bad.SetActive (true);
	}
	public void OpenShop() {
	// need to summon a new dude after closing it
		PageNum = 0; // the list starts @ 0
		ShopGO.SetActive (true);
		CloseBut.SetActive (true);
		GameStart.SetActive (false);
		UpdatePages ();
	}

	public void CloseShop(){
		PageNum = 0; // the list starts at 0
		ShopGO.SetActive (true);
		CloseBut.SetActive (false);
		GameStart.SetActive (true);
		Pages [PageNum].SetActive (false);
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
		if (Lv.ToString() == "2" && AblGO.GetComponent<Abl>().PlayersAbls.Contains("scrach") != true) {
			scrachGO.SetActive (true);
			AblGO.GetComponent<Abl> ().UnlockAbl ("scrach", 0);
		}
		if (Hp.value <= 0) { // adds to the death pool but no math yet...
		
			Hp.value = 100;
			HpT.text = Hp.ToString();
			Deaths++;
			DeathsT.text = Deaths.ToString();
			// Money and math time!

			AmountWeTook = (AmountWeTook*2) * (Deaths*2);
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
		Save ();
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
		
	public void StartGame() {
		GameStart.SetActive (true);
		StartGO.SetActive (false);
	}
	public void Save() {
	// wait so we are allowing this?
		if (AllowLocalSaving) {
			// saving mech here
			//Debug.Log("Saving Game ( not this is still being built ) ");
			Directory.CreateDirectory (@PathForSaving);

			string DataToSave = "#Gold#" + Gold + "#Lv#" + Lv + "#Zone#" + Zone + "#Deaths#" + Deaths + "#ClickerUpgrade#" + ClickerUpgrade + "#DamPerHit#" + Bad.GetComponent<badguy> ().DamPerHit + "#GameID#" + GameID + "#AblPoints#" + AblGO.GetComponent<Abl> ().AblPoints + "#BadMaxHp#" + Bad.GetComponent<badguy>().BadHpBar.maxValue;
			// now we encrypt
			string NewDataToSave = "";
			NewDataToSave = EncryptDecrypt(DataToSave);
			//Debug.Log (NewDataToSave);
			File.WriteAllText(@PathForSaving + "/" + DatabaseName + DatabaseExt ,NewDataToSave);


			OutPut.text = "Saved your data!";
		}

	}
	public void Load() {
	// wait we are allowing this?!

			
		
		if (AllowLocalSaving) {
			
			//loading mech here
			//	Debug.Log("Loading Game ( not this is still being built ) ");
			//File.Decrypt (@"/Data/testing.data");
			try {
			string Oldtesting = File.ReadAllText (@PathForSaving + "/" + DatabaseName + DatabaseExt);
				string testing = "";
				testing = EncryptDecrypt(Oldtesting);
				Debug.Log(testing);
			//Debug.Log(testing);
			string[] newData = testing.Split ('#');
			int LineCount = 0;
			//	int RealLineCount = 0;
			bool Data = false;
			string Datas = "";
			List<string> CleanData = new List<string> ();
			foreach (var Line in newData) {
				//RealLineCount++;
				//if (RealLineCount > 1) {

				if (Data) {
					Data = false;
					Datas = Line;
				} else {
					Data = true;
					LineCount++;
					string output = Datas + ":" + Line;
					Datas = "";
					//Debug.Log ("(" + LineCount + "):" + output);
					CleanData.Add (output);
				}
				//}
			}
			LineCount = 0;
			
			foreach (var stat in CleanData) {
				LineCount++;
				if (LineCount != 1) {
					//	Debug.Log ("Loading Stat:"+stat);
				}
				string[] data = stat.Split (':');
				bool Name = true;
				string NameString = "";
				foreach (string datapoint in data) {
					//MudData.Add (datapoint);
					if (Name) {
						NameString = datapoint;
						Name = false;
					} else {
						// we now have the full data..
						if (NameString != "" && playerStats.ContainsKey(NameString) == false) {
							//Debug.Log ("(" + (LineCount - 1) + ")" + NameString + "@" + datapoint);
							playerStats.Add (NameString, datapoint);
						}
					}
				}

			}
			// we can now pull some stats...
			// lets pull stats out of the air.
				string NewGold = "-1";
				int NewGoldValue = 0;
				playerStats.TryGetValue ("Gold", out NewGold);
				int.TryParse (NewGold ,out NewGoldValue);
				Debug.Log ("Your Gold was set to:" + NewGoldValue);
				Gold = NewGoldValue;
				string NewLv = "-1";
				int NewLvValue = 0;
				playerStats.TryGetValue ("Lv", out NewLv);
				int.TryParse (NewLv ,out NewLvValue);
				Debug.Log ("Your Lv was set to:" + NewLvValue);
				Lv = NewLvValue;
				string NewZone = "-1";
				int NewZoneValue = 0;
				playerStats.TryGetValue ("Zone", out NewZone);
				int.TryParse (NewZone ,out NewZoneValue);
				Debug.Log ("Your Zone was set to:" + NewZoneValue);
				Zone = NewZoneValue;
				string NewDeaths = "";
				int NewDeathsValue = 0;
				playerStats.TryGetValue ("Deaths", out NewDeaths);
				int.TryParse (NewDeaths ,out NewDeathsValue);
				Debug.Log ("Your Deaths was set to:" + NewDeathsValue);
				Deaths = NewDeathsValue;
				string NewClickerUpgrade = "";
				bool NewClickerUpgradeValue = false;
				playerStats.TryGetValue("ClickerUpgrade",out NewClickerUpgrade);
				bool.TryParse(NewClickerUpgrade,out NewClickerUpgradeValue);
				Debug.Log("Your Clicker Upgrade Value was set to:" + NewClickerUpgradeValue);
				ClickerUpgrade = NewClickerUpgradeValue;
				string NewDamPerHit = "";
				int NewDamPerHitValue = 9;
				playerStats.TryGetValue("DamPerHit",out NewDamPerHit);
				int.TryParse(NewDamPerHit,out NewDamPerHitValue);
				Debug.Log("Your Dam Per Hit was set to:" + NewDamPerHitValue);
				Bad.GetComponent<badguy> ().DamPerHit = NewDamPerHitValue;
				playerStats.TryGetValue("GameID",out GameID);
				Debug.Log("Game ID:" + GameID);
				string AblPointsS = "";
				int AblPoints = 0;
				playerStats.TryGetValue("AblPoints",out AblPointsS);
				int.TryParse(AblPointsS,out AblPoints);
				AblGO.GetComponent<Abl>().AblPoints = AblPoints;
				Debug.Log("Your ABL Points where set to :" + AblPoints);
				string BadMaxHp = "";
				float MaxHp = 100f; // MIN!
				playerStats.TryGetValue("BadMaxHp",out BadMaxHp);
				float.TryParse(BadMaxHp,out MaxHp);
				Bad.GetComponent<badguy>().BadHpBar.maxValue = MaxHp;

				//BadMaxHp
		}
		catch (System.Exception ex) {
			// we cant save
			//OutPut.text = "Sorry we cant load anything...";
			// if we can't read the data then we need to make a new one...
			//Save();
			Debug.Log (ex);
		}
	}
	}
	public void Restart() {
	// clearing all data!
		File.Delete(@PathForSaving + "/" + DatabaseName + DatabaseExt);
	}
	public void BuyClickerUpgrade() {
		if (Gold >= 500) {
			ClickerUpgrade = true;
			Bad.GetComponent<badguy> ().DamPerHit = Bad.GetComponent<badguy> ().DamPerHit + 5;
			ClickerUpgradeGO.SetActive (false);
		} else {
			ClickerButton.text = "Cost 500!!!";
		}// so this is part of the save...
	}
	public void Start() {
		Load (); // will save if theres no data!!
		if (GameID == "None" || GameID == "") {
			// we have loaded so this is us making a save ID
			string[] thing = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
			GameID = thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + "&" + Random.Range (0, 10) + Random.Range (0, 20) + Random.Range (0, 30) + Random.Range (0, 40) + Random.Range (0, 50);
		}
		Save (); // will save def value if load is broke

		if (GameID == "Admin" || GameID == "admin") { // we are dealing with someone who knows the code
			WelcomeText.text = "Welcome admin to my land of code! " +
				"just wanted to say hi and inform you of the cheats that " +
				"you have just used 1st i have reset your GameID and next i" +
				"have given you lots and lots of gold and lv have fun" +
				" rule braker";
			string[] thing = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
			GameID = thing [Random.Range (0, 25)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + "&" + Random.Range (0, 10) + Random.Range (0, 20) + Random.Range (0, 30) + Random.Range (0, 40) + Random.Range (0, 50);
			Gold = Gold + 999999;
			Lv = Lv + 99;
			Save ();
		}
		UpdateStats();
		if (ClickerUpgrade) {
			ClickerUpgradeGO.SetActive (false);
		} else {
			ClickerUpgradeGO.SetActive (true);
		}
		if (Lv >= 1 && AblGO.GetComponent<Abl>().PlayersAbls.Contains("scrach") != true) {
			AblGO.GetComponent<Abl> ().UnlockAbl ("scrach", 0);
		}
		Bar.interactable = false;
		AmountWeTook = 1;
		StartGO.SetActive (true);
		GameStart.SetActive (false);
	}
}
