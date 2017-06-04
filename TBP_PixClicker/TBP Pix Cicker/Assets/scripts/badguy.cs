using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class badguy : MonoBehaviour {
	[Header("API's")]
	public GameObject _Core;
	public GameObject _Abl;
	[Header("TypeOfMonster")]
	public bool Normal = true;
	public bool Boss = false;
	[Header("List")]
	public List<GameObject> D_Buttons;
	[Header("pics and things")]
	public Slider BadHpBar;
	public Button but;
	public Image Hold;
	public Sprite DamagedPic;
	public Sprite NormalPic;
	public Sprite AblPic;
	public Sprite AttackingPic;
	public int DamPerHit;
	public int Damage;
	[Header("Times and things")]
	public int t = 0;
	public int t2 = 0;
	public int t3 = 0;
	public int TimeTellAttack;
	public int maxTime;
	public bool Posed = false;
	[Header("Monster Abilty Related stuff")]
	public bool InAbilty = true;
	public int PosTime = 500; // how long it applys the pos effect
	[Header("Monster Abilty(s)")]
	public bool Invs = true; // hidden from attacks
	public bool Rage = false; // +1 to all attacks (adds on per rage)
	public bool Pos = false; // applys posion to player (-0.01hp/tick)
	public bool Healing = false; // heals the badguy (offen combo with Invs
	public bool Flash = false; // turns the game off and back on.
	public bool Scream = false; // turns off all player skills.
	public bool Drain = false; // drains player for HP
	[Header("Boss Abilty(s)")]
	public bool Adv_Block = false; // turns on the adv_block layor
	public bool Adv_Grow = false; // Starts the staging prossess.
	[Header("Player Bools")]
	public bool P_Invs = false;
	//public bool Clone = false; // the idea is that there are two monsters you must kill
	//public bool Grow = false; // the idea is that the monster hp *2/abl
	[Header("GO's")]
	public GameObject Game;
	public List<GameObject> BlockScreen;

	IEnumerator damw()
	{
		yield return new WaitForSeconds(1); // add a little bit of time for the user to read the last text!
		//Hold.sprite = NormalPic;
	
	}
	IEnumerator Abl()
	{
		// you cant hit during abl
		if (Drain) { // due to drain -hp + hp /t ... we have those.
			Drain = false;
			Healing = true;
			Pos = true;
		}
		if (Invs) {
			but.interactable = false;
			InAbilty = true;
		}
		if (Rage && Damage <= 3) {
			Damage++;
			InAbilty = true;
		}
		if (Pos) {
		// apply pos
			Posed = true;
			InAbilty = true;
		}
		//Hold.sprite = AblPic;
		if (Healing == true) {
			BadHpBar.value = BadHpBar.value + _Core.GetComponent<_Core>().Zone/100 + 0.5f;
			// boss edit...
			if (Boss) {
				// if its a boss its a % of total hp.
				BadHpBar.value = BadHpBar.value * 1.001f; // set to (0.001% + zone)/tick (realy fast tho!)
			}
			InAbilty = true;
		}
		if (Flash) {
			// we need to kill the game and start up a white screen. for about 1s or somthing then run the game.
			Game.SetActive(false);
		}
		if (Adv_Block) {
			foreach (var block in BlockScreen) {
				block.SetActive (true);
			}
		}
		if (Scream) {
			_Abl.GetComponent<Abl> ().LockSkills (); // turns on the cd for all skills.
		}
		yield return new WaitForSeconds(5); // add a little bit of time for the user to read the last text!
		if (Invs) {
			but.interactable = true;
			InAbilty = false;
		}
		if (Rage || Damage > 2) {
			Rage = false;
			InAbilty = false;
		}
		if (Flash) {
			// we need to kill the game and start up a white screen. for about 1s or somthing then run the game.
			Game.SetActive(true);

		}



		//Hold.sprite = AttackingPic;
		t = 0;
	}
	IEnumerator Attack()
	{
		if(InAbilty){
			Debug.Log ("Attack Blocked due to it being in abl.");
		}else{
		Hold.sprite = AttackingPic;
		_Core.GetComponent<_Core> ().Removehp (Damage);
		Hold.sprite = AttackingPic;
		yield return new WaitForSeconds (1); // add a little bit of time for the user to read the last text!
		//but.interactable = true;
		Hold.sprite = NormalPic;
		}
	}


	public void dam() {
		BadHpBar.value = BadHpBar.value - DamPerHit;
		// are we in anyother sprite?
		Hold.sprite = DamagedPic;
	StartCoroutine ("damw");
	}
	public void Update() {
		t++;
		t++;
		t2++;
//		AblPic = AblPic;
		if (Boss) { // we are not a normal monster.
			Normal = false;
		} else {
			Normal = true;
		}
		if (Posed) {
			t3++;
			_Core.GetComponent<_Core> ().Removehp (0.1f * ((Mathf.RoundToInt(_Core.GetComponent<_Core> ().Zone/4f))/8));
			if (t3 >= PosTime) {
				t3 = 0;
				Posed = false;
			}
		}
		if (BadHpBar.value == BadHpBar.minValue) {
			if (Normal) { // we dont handle non-normal deaths.
				// so the _core handles almost all of this so lets use the api built in.
				Debug.Log ("before:" + BadHpBar.maxValue);
				BadHpBar.maxValue = BadHpBar.maxValue + _Core.GetComponent<_Core> ().Zone / 4;
				Debug.Log ("after:" + BadHpBar.maxValue);
				_Core.GetComponent<_Core> ().SummonNewBad ();
				_Core.GetComponent<_Core> ().KilledMob ();
				_Core.GetComponent<_Core> ().UpdateStats ();
				//_Core.GetComponent<_Core> ().Zone++;
				Damage = Damage + Mathf.RoundToInt (_Core.GetComponent<_Core> ().Zone / 8f);
			} else {
			// tell core that what ever is going on its dead.
				_Core.GetComponent<_Core>().BossKilled();
				// WE DONT SET IT BACK we just tell them its over.
			}

		}
		if (t >= maxTime) {
			// run ablit
			but.interactable = true;
			Hold.sprite = AblPic;
			StartCoroutine("Abl");


		}
		if (t2 >= TimeTellAttack && but.interactable == true) {
		
			t2 = 0;
			Hold.sprite = AttackingPic;
			if (P_Invs != true) {
				StartCoroutine ("Attack");
				int outcome = Random.Range (0, 100);
				if (outcome <= 25) {
					foreach (var thing in D_Buttons) {
						thing.SetActive (false);
					}
					outcome = Random.Range (0, D_Buttons.Count);
					D_Buttons [outcome].SetActive (true);

				}
			}

			}
	}

}
