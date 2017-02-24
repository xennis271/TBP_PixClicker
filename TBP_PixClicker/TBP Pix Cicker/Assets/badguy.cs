using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class badguy : MonoBehaviour {
	[Header("API's")]
	public GameObject _Core;
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
	[Header("Abilty(s)")]
	public bool Invs = true; // hidden from attacks
	public bool Rage = false; // +1 to all attacks (adds on per rage)
	public bool Pos = false; // applys posion to player (-0.01hp/tick)
	public int PosTime = 500; // how long it applys the pos effect
	//public bool Clone = false; // the idea is that there are two monsters you must kill
	//public bool Grow = false; // the idea is that the monster hp *2/abl

	IEnumerator damw()
	{
		yield return new WaitForSeconds(1); // add a little bit of time for the user to read the last text!
		//Hold.sprite = NormalPic;
	
	}
	IEnumerator Abl()
	{
		// you cant hit during abl
		if (Invs) {
			but.interactable = false;
		}
		if (Rage && Damage <= 3) {
			Damage++;
		}
		if (Pos) {
		// apply pos
			Posed = true;
		}
		Hold.sprite = AblPic;
		yield return new WaitForSeconds(5); // add a little bit of time for the user to read the last text!
		if (Invs) {
			but.interactable = true;
		}
		if (Rage || Damage > 2) {
			Rage = false;
		}
		Hold.sprite = NormalPic;
		t = 0;
	}
	IEnumerator Attack()
	{
		_Core.GetComponent<_Core> ().Removehp (Damage);
		// you cant hit during abl
		//but.interactable = false;
		Hold.sprite = AttackingPic;
		yield return new WaitForSeconds(1); // add a little bit of time for the user to read the last text!
		//but.interactable = true;
		Hold.sprite = NormalPic;


	}
	public void dam() {
		BadHpBar.value = BadHpBar.value - DamPerHit;
		Hold.sprite = DamagedPic;
		StartCoroutine ("damw");

	}
	public void Update() {
		t++;
		t++;
		t2++;
//		AblPic = AblPic;
		if (Posed) {
			t3++;
			_Core.GetComponent<_Core> ().Removehp (0.1f * ((Mathf.RoundToInt(_Core.GetComponent<_Core> ().Zone/4f))/4));
			if (t3 >= PosTime) {
				t3 = 0;
				Posed = false;
			}
		}
		if (BadHpBar.value == BadHpBar.minValue) {
			// so the _core handles almost all of this so lets use the api built in.
			_Core.GetComponent<_Core> ().SummonNewBad ();
			_Core.GetComponent<_Core>().KilledMob();
			_Core.GetComponent<_Core> ().UpdateStats ();
			Damage = Damage + Mathf.RoundToInt(_Core.GetComponent<_Core> ().Zone/4f);
		}
		if (t >= maxTime) {
			// run ablit
			but.interactable = true;
			Hold.sprite = AblPic;
			StartCoroutine("Abl");

		}
		if (t2 >= TimeTellAttack && but.interactable == true) {
		
			t2 = 0;
			StartCoroutine ("Attack");

			}
	}

}
