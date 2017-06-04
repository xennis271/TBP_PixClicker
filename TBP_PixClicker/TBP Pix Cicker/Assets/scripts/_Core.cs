using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class _Core : MonoBehaviour
{
	[Header ("Game Id#")]
	public string GameID = "Loading";
	[Header ("Stats LIVE")]
	public int Gold;
	public Slider Hp;
	// gets rounded
	public int Lv;
	public int BossesSummoned = 0;
	public int Zone;
	public int Deaths;
	public Slider EXPbar;
	public int AmountWeTook = 0;
	public static int key = 921;
	[Header ("GO's")]
	public Slider Bar;
	public GameObject Bad;
	public GameObject StartGO;
	public GameObject GameStart;
	public GameObject AblGO;
	public GameObject _Core2;
	[Header ("AblButtons")]
	public GameObject scrachGO;
	public GameObject healingGO;
	[Header ("List")]
	public List<Sprite> MobsNormal = new List<Sprite> ();
	public List<Sprite> MobsAttack = new List<Sprite> ();
	public List<Sprite> MobsHurt = new List<Sprite> ();
	public List<Sprite> MobsAbl = new List<Sprite> ();
	public List<string> MobAbls = new List<string> ();
	public List<string> MobDetail = new List<string> ();
	public List<string> MobName = new List<string> ();
	public List<Sprite> BossNormal = new List<Sprite> ();
	public List<Sprite> BossAttack = new List<Sprite> ();
	public List<Sprite> BossAbl = new List<Sprite> ();
	public List<Sprite> BossHurt = new List<Sprite> ();
	public List<string> BossDetails = new List<string> ();
	public List <string> BossNames = new List<string> ();
	public Dictionary<string,string> playerStats = new Dictionary<string, string> ();
	[Header ("UI stuff")]
	public GameObject ShopGO;
	public GameObject CloseBut;
	public List<GameObject> Pages = new List<GameObject> ();
	public int PageNum;
	public Text OutPut;
	public Text WelcomeText;
	[Header ("Text for stats")]
	public Text GoldT;
	public Text HpT;
	public Text LvT;
	public Text ZoneT;
	public Text DeathsT;
	[Header ("Dev Bools")]
	public bool AllowEditingHp = false;
	public bool AllowLocalSaving = true;
	public string PathForSaving = "";
	public string DatabaseName = "";
	public string DatabaseExt = ".DBv1";
	[Header ("Encryption Dont Turn Off!")]
	public bool Encryption = true;
	[Header ("Store Stuff")]
	public bool ClickerUpgrade;
	public bool HpUpgrade;
	public bool scrachUpgrade;
	public bool healingUpgrade;
	//
	public GameObject healingUpgradeGO;
	public GameObject ClickerUpgradeGO;
	public GameObject HpUpgradeGO;
	public GameObject scrachUpgradeGO;
	//
	public Text healingpotButton;
	public Text manapotButton;
	public Text TimepotButton;
	public Text healingButton;
	public Text scrachButton;
	public Text HpButton;
	public Text ClickerButton;
	[Header ("Boss stuff")]
	public bool TimeSummoned = false;
	public bool EarthSummoned = false;
	public int SlimeKingStage = 0;



	public string EncryptDecrypt (string TextToEncrypt)
	{

		if (Encryption == true) {
			StringBuilder inSb = new StringBuilder (TextToEncrypt);
			StringBuilder outSb = new StringBuilder (TextToEncrypt.Length);
			char c;
			for (int i = 0; i < TextToEncrypt.Length; i++) {
				c = inSb [i];
				c = (char)(c ^ key);
				outSb.Append (c);
			}
		
			return outSb.ToString ();
		} else {
			return TextToEncrypt;
		}
	}

	public void Removehp (float amount)
	{
		Bar.value = Bar.value - amount;
	}

	public void Heal ()
	{
		Bar.value = Bar.maxValue;
	}

	public void SummonNewBad ()
	{
		Bad.SetActive (false);
		//AblGO.GetComponent<badguy> ().Boss = false;
		Bad.GetComponent<badguy> ().Normal = true;
		// update setings...
		int outp;
		//Zone++; Handled by badguy now...
		outp = Random.Range (0, MobsNormal.Count);
		//Debug.Log (outp);
		Bad.GetComponent<badguy> ().NormalPic = MobsNormal [outp];
		Bad.GetComponent<badguy> ().AttackingPic = MobsAttack [outp];
		Bad.GetComponent<badguy> ().DamagedPic = MobsHurt [outp];
		Bad.GetComponent<badguy> ().AblPic = MobsAbl [outp];
		string outs = "";
		outs = MobAbls [outp];
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

	public void OpenShop ()
	{
		// need to summon a new dude after closing it
		PageNum = 0; // the list starts @ 0
		ShopGO.SetActive (true);
		CloseBut.SetActive (true);
		GameStart.SetActive (false);
		UpdatePages ();
	}

	public void CloseShop ()
	{
		PageNum = 0; // the list starts at 0
		ShopGO.SetActive (true);
		CloseBut.SetActive (false);
		GameStart.SetActive (true);
		Pages [PageNum].SetActive (false);
	}

	public void UpdatePages ()
	{
		// we need to close all pages...
		foreach (GameObject Page in Pages) {
			Page.SetActive (false);
		}
		// open the one we want.
		Pages [PageNum].SetActive (true);
	}

	public void TurnPage ()
	{
		PageNum++;
		UpdatePages ();
	}

	public void TurnBackAPage ()
	{
		PageNum--;
		UpdatePages ();
	}

	public void UpdateStats ()
	{
		GoldT.text = Gold.ToString ();
		HpT.text = Mathf.RoundToInt (Hp.value).ToString (); // looks better!
		LvT.text = Lv.ToString ();
		ZoneT.text = Zone.ToString ();
		DeathsT.text = Deaths.ToString ();
		if (Lv.ToString () == "2" && AblGO.GetComponent<Abl> ().PlayersAbls.Contains ("scrach") != true) {
			scrachGO.SetActive (true);
			AblGO.GetComponent<Abl> ().UnlockAbl ("scrach", 0);
		}
		if (Lv.ToString () == "4" && AblGO.GetComponent<Abl> ().PlayersAbls.Contains ("healing") != true) {
			healingGO.SetActive (true);
			AblGO.GetComponent<Abl> ().UnlockAbl ("healing", 0);
		}
		if (Hp.value <= 0) { // adds to the death pool but no math yet...
		
			Hp.value = 100;
			HpT.text = Hp.ToString ();
			Deaths++;
			DeathsT.text = Deaths.ToString ();
			// you died and thus we need to hurt your zone
			Zone = Zone / 2; // we are sending you way back!
			// Money and math time!

			AmountWeTook = (AmountWeTook * 2) * (Deaths * 2);
			//Debug("Amount you had" + Gold.ToString() + " The amount we took" +AmountWeTook.ToString()); 
			Gold = Gold - AmountWeTook; // << thats one mean mean line!
			if (Gold < 0) {
				Gold = 0;
			}
		
		}
		if (EXPbar.value == EXPbar.maxValue) {
			EXPbar.value = EXPbar.minValue;
			Lv++;
			EXPbar.maxValue = EXPbar.maxValue * 2; // trying to grow!
		}
		Save ();
	}

	int PreZone;

	public void KilledMob ()
	{
		Zone++;
		PreZone++;
		Gold = Gold + (Mathf.RoundToInt ((-29.952f + 2.251f * Zone))); // gives more than the old = in tell about 90-100 then it gives less something i think is fine.
		// Nice job!
		// need to give some EXP
		EXPbar.value = EXPbar.value + Zone * 2; // i think this is more fair
		if (PreZone == 10) {
			EXPbar.maxValue = Zone * 2.5f + EXPbar.maxValue;
		}
	}


		
	public void StartGame ()
	{
		GameStart.SetActive (true);
		StartGO.SetActive (false);
	}

	public void Save ()
	{
		// wait so we are allowing this?
		if (AllowLocalSaving) {
			// saving mech here
			//Debug.Log("Saving Game ( not this is still being built ) ");
			if (Bad.GetComponent<badguy> ().Boss == false) {
				Directory.CreateDirectory (@Application.persistentDataPath + PathForSaving);

				string DataToSave = // open all save data! (ONE HUGE LINE!)
					// 						FORMAT	:	#DATA# + VALUE 
					//						prossessing takes your data and puts it into a list for look up.
					// 						ie list[hp] = value;
					"#Gold#" + Gold +
					"#Lv#" + Lv +
					"#Zone#" + Zone +
					"#Deaths#" + Deaths +
					"#ClickerUpgrade#" + ClickerUpgrade +
					"#DamPerHit#" + Bad.GetComponent<badguy> ().DamPerHit +
					"#GameID#" + GameID +
					"#AblPoints#" + AblGO.GetComponent<Abl> ().AblPoints +
					"#BadMaxHp#" + Bad.GetComponent<badguy> ().BadHpBar.maxValue +
					"#hpUpgrade#" + HpUpgrade +
					"#scrachUpgrade#" + scrachUpgrade +
					"#healingUpgrade#" + healingUpgrade +
					"#scrachDamageUpgrade#" + AblGO.GetComponent<Abl> ().Scrach_Damage.ToString () +
					"#healingTime#" + AblGO.GetComponent<Abl> ().Healing_TimeOfEffect.ToString () +
					"#healingPots#" + _Core2.GetComponent<_Core2> ().AmountOfHealingPots.ToString () +
					"#manaPots#" + _Core2.GetComponent<_Core2> ().AmountOfManaPots.ToString ();
				// now we encrypt
				string NewDataToSave = "";
				NewDataToSave = EncryptDecrypt (DataToSave);
				//Debug.Log (NewDataToSave);
				File.WriteAllText (@Application.persistentDataPath + PathForSaving + "/" + DatabaseName + DatabaseExt, NewDataToSave);


				OutPut.text = "Saved your data!";
			} else {
				OutPut.text = "Can't save during a boss fight!";
			}
		}

	}

	public void Load ()
	{
		// wait we are allowing this?!

			
		
		if (AllowLocalSaving) {
			
			//loading mech here
			//	Debug.Log("Loading Game ( not this is still being built ) ");
			//File.Decrypt (@"/Data/testing.data");
			try {
				
				string Oldtesting = File.ReadAllText (@Application.persistentDataPath + PathForSaving + "/" + DatabaseName + DatabaseExt);
				string testing = "";
				testing = EncryptDecrypt (Oldtesting);
				Debug.Log (testing);
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
							if (NameString != "" && playerStats.ContainsKey (NameString) == false) {
								//Debug.Log ("(" + (LineCount - 1) + ")" + NameString + "@" + datapoint);
								playerStats.Add (NameString, datapoint);
							}
						}
					}

				}
				// we can now pull some stats...
				// 																Data grabing!!!
				//															 -the magic land-
				string NewGold = "-1";
				int NewGoldValue = 0;
				playerStats.TryGetValue ("Gold", out NewGold);
				int.TryParse (NewGold, out NewGoldValue);
				Debug.Log ("Your Gold was set to:" + NewGoldValue);
				Gold = NewGoldValue;
				string NewLv = "-1";
				int NewLvValue = 0;
				playerStats.TryGetValue ("Lv", out NewLv);
				int.TryParse (NewLv, out NewLvValue);
				Debug.Log ("Your Lv was set to:" + NewLvValue);
				Lv = NewLvValue;
				string NewZone = "-1";
				int NewZoneValue = 0;
				playerStats.TryGetValue ("Zone", out NewZone);
				int.TryParse (NewZone, out NewZoneValue);
				Debug.Log ("Your Zone was set to:" + NewZoneValue);
				Zone = NewZoneValue;
				string NewDeaths = "";
				int NewDeathsValue = 0;
				playerStats.TryGetValue ("Deaths", out NewDeaths);
				int.TryParse (NewDeaths, out NewDeathsValue);
				Debug.Log ("Your Deaths was set to:" + NewDeathsValue);
				Deaths = NewDeathsValue;
				string NewClickerUpgrade = "";
				bool NewClickerUpgradeValue = false;
				playerStats.TryGetValue ("ClickerUpgrade", out NewClickerUpgrade);
				bool.TryParse (NewClickerUpgrade, out NewClickerUpgradeValue);
				Debug.Log ("Your Clicker Upgrade Value was set to:" + NewClickerUpgradeValue);
				ClickerUpgrade = NewClickerUpgradeValue;
				string NewDamPerHit = "";
				int NewDamPerHitValue = 9;
				playerStats.TryGetValue ("DamPerHit", out NewDamPerHit);
				int.TryParse (NewDamPerHit, out NewDamPerHitValue);
				Debug.Log ("Your Dam Per Hit was set to:" + NewDamPerHitValue);
				Bad.GetComponent<badguy> ().DamPerHit = NewDamPerHitValue;
				playerStats.TryGetValue ("GameID", out GameID);
				Debug.Log ("Game ID:" + GameID);
				string AblPointsS = "";
				int AblPoints = 0;
				playerStats.TryGetValue ("AblPoints", out AblPointsS);
				int.TryParse (AblPointsS, out AblPoints);
				AblGO.GetComponent<Abl> ().AblPoints = AblPoints;
				Debug.Log ("Your ABL Points where set to :" + AblPoints);
				string BadMaxHp = "";
				float MaxHp = 100f; // MIN!
				playerStats.TryGetValue ("BadMaxHp", out BadMaxHp);
				float.TryParse (BadMaxHp, out MaxHp);
				Bad.GetComponent<badguy> ().BadHpBar.maxValue = MaxHp;
				string NewHpUpgrade = "";
				bool NewHpUpgradeValue = false;
				playerStats.TryGetValue ("hpUpgrade", out NewHpUpgrade);
				bool.TryParse (NewHpUpgrade, out NewHpUpgradeValue);
				Debug.Log ("Your Hp Upgrade Value was set to:" + NewHpUpgradeValue);
				HpUpgrade = NewHpUpgradeValue;
				string NewscrachUpgrade = "";
				bool NewscrachUpgradeValue = false;
				playerStats.TryGetValue ("scrachUpgrade", out NewscrachUpgrade);
				bool.TryParse (NewscrachUpgrade, out NewscrachUpgradeValue);
				Debug.Log ("Your scrach Upgrade Value was set to:" + NewscrachUpgradeValue);
				scrachUpgrade = NewscrachUpgradeValue;
				string NewhealingUpgrade = "";
				bool NewhealingUpgradeValue = false;
				playerStats.TryGetValue ("healingUpgrade", out NewhealingUpgrade);
				bool.TryParse (NewhealingUpgrade, out NewhealingUpgradeValue);
				Debug.Log ("Your healing Upgrade Value was set to:" + NewhealingUpgradeValue);
				healingUpgrade = NewhealingUpgradeValue;
				string NewhealingTimeUpgrade = "";
				int NewhealingTimeUpgradeValue = 10;
				playerStats.TryGetValue ("healingTime", out NewhealingTimeUpgrade);
				int.TryParse (NewhealingTimeUpgrade, out NewhealingTimeUpgradeValue);
				Debug.Log ("Your healing Time Upgrade Value was set to:" + NewhealingTimeUpgradeValue);
				AblGO.GetComponent<Abl> ().Healing_TimeOfEffect = NewhealingTimeUpgradeValue;
				string NewscrachDamageUpgrade = "";
				int NewscrachDamageUpgradeValue = 20;
				playerStats.TryGetValue ("scrachDamageUpgrade", out NewscrachDamageUpgrade);
				bool worked = int.TryParse (NewscrachDamageUpgrade, out NewscrachDamageUpgradeValue);
				Debug.Log ("Your scrach Damage Upgrade Value was set to:" + NewscrachDamageUpgradeValue + "Stats:" + worked);
				AblGO.GetComponent<Abl> ().Scrach_Damage = NewscrachDamageUpgradeValue;
				string NewHealingPots = "";
				int NewHealingPotsValue = 10;
				playerStats.TryGetValue ("healingPots", out NewHealingPots);
				int.TryParse (NewHealingPots, out NewHealingPotsValue);
				Debug.Log ("Your Healing Pots Value was set to:" + NewHealingPotsValue);
				_Core2.GetComponent<_Core2> ().AmountOfHealingPots = NewHealingPotsValue;
				string NewmanaPots = "";
				int NewmanaPotsValue = 10;
				playerStats.TryGetValue ("manaPots", out NewmanaPots);
				int.TryParse (NewmanaPots, out NewmanaPotsValue);
				Debug.Log ("Your mana Pots Value was set to:" + NewmanaPotsValue);
				_Core2.GetComponent<_Core2> ().AmountOfManaPots = NewmanaPotsValue;

				//																							 SKILLS ZONE!
				AblGO.GetComponent<Abl> ().ResetSkills ();
				if (Lv >= 2) {
					AblGO.GetComponent<Abl> ().UnlockAbl ("scrach", 0);
				}
				if (Lv >= 4) {
					AblGO.GetComponent<Abl> ().UnlockAbl ("healing", 0);
				}
				//BadMaxHp
			} catch (System.Exception ex) {
				// we cant save
				//OutPut.text = "Sorry we cant load anything...";
				// if we can't read the data then we need to make a new one...
				//Save();
				Debug.Log (ex);
			}
		}
	}

	public void Restart ()
	{
		// clearing all data!
		File.Delete (@PathForSaving + "/" + DatabaseName + DatabaseExt);
	}

	public void BuyClickerUpgrade ()
	{
		if (Gold >= 50) {
			ClickerUpgrade = true;
			Bad.GetComponent<badguy> ().DamPerHit = Bad.GetComponent<badguy> ().DamPerHit + 5;
			ClickerUpgradeGO.SetActive (false);
		} else {
			ClickerButton.text = "Cost 50!!!";
		}// so this is part of the save...
	}

	public void BuyHpUpgrade ()
	{
		if (Gold >= 50) {
			HpUpgrade = true;
			Hp.maxValue = Hp.maxValue + 100;
			HpUpgradeGO.SetActive (false);
		} else {
			HpButton.text = "Cost 50!!!";
		}// so this is part of the save...
	
	}

	public void BuyScrachUpgrade ()
	{
		if (Gold >= 100) {
			scrachUpgrade = true;
			AblGO.GetComponent<Abl> ().Scrach_Damage = AblGO.GetComponent<Abl> ().Scrach_Damage + 20;
			scrachUpgradeGO.SetActive (false);
			Save ();
		} else {
			scrachButton.text = "Cost:100!!!";
		}
	}

	public void BuyhealingUpgrade ()
	{
		if (Gold >= 100) {
			healingUpgrade = true;
			AblGO.GetComponent<Abl> ().Healing_TimeOfEffect = AblGO.GetComponent<Abl> ().Healing_TimeOfEffect - 1;
			healingUpgradeGO.SetActive (false);
			Save ();
		} else {
			healingButton.text = "Cost:100!!!";
		}
	}

	public void BuyhealingPot ()
	{
		if (Gold >= 25) {
			Gold = Gold - 25;
			_Core2.GetComponent<_Core2> ().AmountOfHealingPots++;
			_Core2.GetComponent<_Core2> ().HpGO.SetActive (true);
			
		} else {
			healingpotButton.text = "Cost:25!!!";
		}

	}

	public void BuymanaPot ()
	{
		if (Gold >= 25) {
			Gold = Gold - 25;
			_Core2.GetComponent<_Core2> ().AmountOfManaPots++;
			_Core2.GetComponent<_Core2> ().ManaGO.SetActive (true);

		} else {
			manapotButton.text = "Cost:25!!!";
		}
	}

	public void BuyTimePot ()
	{
		if (Gold >= 150) {
			Gold = Gold - 150;
			_Core2.GetComponent<_Core2> ().AmountOfTimePots++;
			_Core2.GetComponent<_Core2> ().TimeGO.SetActive (true);

		} else {
			TimepotButton.text = "Cost:150!!!";
		}
	}

	public void Start ()
	{
		Load (); // will save if theres no data!!
		if (GameID == "None" || GameID == "") {
			// we have loaded so this is us making a save ID
			string[] thing = {
				"a",
				"b",
				"c",
				"d",
				"e",
				"f",
				"g",
				"h",
				"i",
				"j",
				"k",
				"l",
				"m",
				"n",
				"o",
				"p",
				"q",
				"r",
				"s",
				"t",
				"u",
				"v",
				"w",
				"x",
				"y",
				"z"
			};
			GameID = thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + "&" + Random.Range (0, 10) + Random.Range (0, 20) + Random.Range (0, 30) + Random.Range (0, 40) + Random.Range (0, 50);
		}
		Save (); // will save def value if load is broke

		if (GameID == "Admin" || GameID == "admin") { // we are dealing with someone who knows the code
			WelcomeText.text = "Welcome admin to my land of code! " +
			"just wanted to say hi and inform you of the cheats that " +
			"you have just used 1st i have reset your GameID and next i" +
			"have given you lots and lots of gold and lv have fun" +
			" rule braker";
			string[] thing = {
				"a",
				"b",
				"c",
				"d",
				"e",
				"f",
				"g",
				"h",
				"i",
				"j",
				"k",
				"l",
				"m",
				"n",
				"o",
				"p",
				"q",
				"r",
				"s",
				"t",
				"u",
				"v",
				"w",
				"x",
				"y",
				"z"
			};
			GameID = thing [Random.Range (0, 25)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + thing [Random.Range (0, 26)] + "&" + Random.Range (0, 10) + Random.Range (0, 20) + Random.Range (0, 30) + Random.Range (0, 40) + Random.Range (0, 50);
			Gold = Gold + 999999;
			Lv = Lv + 99;
			Save ();
		}

		if (ClickerUpgrade) {
			ClickerUpgradeGO.SetActive (false);
		} else {
			ClickerUpgradeGO.SetActive (true);
		}
		if (HpUpgrade) {
			HpUpgradeGO.SetActive (false);
		} else {
			HpUpgradeGO.SetActive (true);
		}
		if (scrachUpgrade) {
			scrachUpgradeGO.SetActive (false);
		} else {
			scrachUpgradeGO.SetActive (true);
		}
		if (healingUpgrade) {
			healingUpgradeGO.SetActive (false);
		} else {
			healingUpgradeGO.SetActive (true);
		}
		if (_Core2.GetComponent<_Core2> ().AmountOfHealingPots > 0) {
			_Core2.GetComponent<_Core2> ().HpGO.SetActive (true);
		}
		if (_Core2.GetComponent<_Core2> ().AmountOfManaPots > 0) {
			_Core2.GetComponent<_Core2> ().ManaGO.SetActive (true);
		}
		if (Lv >= 1 && AblGO.GetComponent<Abl> ().PlayersAbls.Contains ("scrach") != true) {
			AblGO.GetComponent<Abl> ().UnlockAbl ("scrach", 0);
		}
		UpdateStats ();
		Bar.interactable = false;
		AmountWeTook = 1;
		//StartGO.SetActive (true);
		GameStart.SetActive (true);
	}

	public void SummonTime ()
	{
		// this is a event that can and will be called at lv 10 OR vi player.
		if (TimeSummoned != true) {
			// tell bad
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 10000; // insane hp for this lv
			badApi.BadHpBar.value = 10000;
			badApi.Damage = badApi.Damage * 50; // grow...
			badApi.Invs = true;
			badApi.Healing = true;
			TimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = BossNormal [0];
			Bad.GetComponent<badguy> ().AttackingPic = BossAttack [0];
			Bad.GetComponent<badguy> ().DamagedPic = BossHurt [0];
			Bad.GetComponent<badguy> ().AblPic = BossAbl [0];

		}
	}

	public void SummonEarth ()
	{
		// this is a event that can and will be called at lv 10 OR vi player.
		if (EarthSummoned != true) {
			// tell bad
			var badApi = Bad.GetComponent<badguy> ();
			badApi.Boss = true;
			badApi.BadHpBar.value = 0;
			badApi.BadHpBar.maxValue = 1000000; // insane hp for this lv
			badApi.BadHpBar.value = 1000000;
			badApi.Damage = badApi.Damage * 100; // grow...
			badApi.Flash = true; // and block as well
			//badApi.Healing = true;
			TimeSummoned = true;
			Bad.GetComponent<badguy> ().NormalPic = BossNormal [1];
			Bad.GetComponent<badguy> ().AttackingPic = BossAttack [1];
			Bad.GetComponent<badguy> ().DamagedPic = BossHurt [1];
			Bad.GetComponent<badguy> ().AblPic = BossAbl [1];

		}
	}
		

	public void KillTime ()
	{
		// in this case this boss has one life so it restarts
		// BUT you could "come back"
		Gold += 50;
		EXPbar.value += 25;
		Bad.GetComponent<badguy> ().BadHpBar.maxValue = 105;

		Bad.GetComponent<badguy> ().Boss = false;
		SummonNewBad ();
	}

	public void KillEarth ()
	{
		Gold += 100;
		EXPbar.value += 50;
		Bad.GetComponent<badguy> ().BadHpBar.maxValue = 200;

		Bad.GetComponent<badguy> ().Boss = false;
		SummonNewBad ();
	}

	public void BossKilled ()
	{
		// who did we kill
		if (TimeSummoned) {
			KillTime ();
		}
		if (EarthSummoned) {
			KillEarth ();
		}
		if (_Core2.GetComponent<_Core2> ().SlimeSummoned == true) {
			if (SlimeKingStage == 1) {
				_Core2.GetComponent<_Core2> ().SummonSlimeKingStageTwo ();
			}
		}
		if (_Core2.GetComponent<_Core2> ().SlimeSummoned == true) {
			if (SlimeKingStage == 2) {
				_Core2.GetComponent<_Core2> ().SummonSlimeKingStageThree ();
			}
		}
		if (_Core2.GetComponent<_Core2> ().SlimeSummoned == true) {
			if (SlimeKingStage == 3) {
				_Core2.GetComponent<_Core2> ().SummonSlimeKingStageFour ();
			}
		}
		if (_Core2.GetComponent<_Core2> ().SlimeSummoned == true) {
			if (SlimeKingStage == 4) {
				_Core2.GetComponent<_Core2> ().KilledSlimeKing ();
			}
		}
		if (Zone <= 10 && Zone > 20) {
			Gold += 10;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 20 && Zone > 10) {
			Gold += 20 * 2;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 30 && Zone > 20) {
			Gold += 30 * 3;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 40 && Zone > 30) {
			Gold += 40 * 4;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 50 && Zone > 40) {
			Gold += 50 * 5;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 60 && Zone > 50) {
			Gold += 60 * 6;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 70 && Zone > 60) {
			Gold += 70 * 7;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 80 && Zone > 70) {
			Gold += 80 * 8;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone <= 90 && Zone > 80) {
			Gold += 90 * 9;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
		if (Zone > 100) {
			Gold += 100 * 10;
			Bad.GetComponent<badguy> ().BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) / 100;
			Bad.GetComponent<badguy> ().Boss = false;
		}
	}

	public void Doge ()
	{
		// this is just trigers the event
		StartCoroutine ("DogeEm");
	}

	IEnumerator DogeEm ()
	{
		// give the player imuity for like 1s
		Bad.GetComponent<badguy> ().P_Invs = true;
		//Debug.Log ("You are god");
		yield return new WaitForSeconds (5);
		Bad.GetComponent<badguy> ().P_Invs = false;
		//Debug.Log("And nvm");
	}

	public void SummonNormalBoss ()
	{
		// in this case we just pick from the pool of monsters and give them stats that are like that of a 2x thing.
		var badApi = Bad.GetComponent<badguy> ();
		badApi.Boss = true;
		badApi.BadHpBar.value = 0;
		badApi.BadHpBar.maxValue = ((Lv + 1) + (Zone / 2)) * 100; // insane hp for this lv
		badApi.BadHpBar.value = badApi.BadHpBar.maxValue;
		badApi.Damage = ((Lv + 1) + (Zone / 2) * 2); // grow...
		//badApi.Invs = true;
		badApi.Healing = true;
		TimeSummoned = true;
		int outn = Random.Range (0, BossNormal.Count);
		Bad.GetComponent<badguy> ().NormalPic = BossNormal [outn];
		Bad.GetComponent<badguy> ().AttackingPic = BossAttack [outn];
		Bad.GetComponent<badguy> ().DamagedPic = BossHurt [outn];
		Bad.GetComponent<badguy> ().AblPic = BossAbl [outn];
	}

	public void Update ()
	{
		UpdateStats ();
		// Boss stuff here!
		if (Zone == 100 && TimeSummoned != true) {
			// summon the Time Boss
			SummonTime ();
		}
		if (Zone == 200 && EarthSummoned != true) {
			SummonEarth ();
		}
		if (Zone == BossesSummoned * 10) {
			SummonNormalBoss ();
			BossesSummoned++;
		}
	}
}

