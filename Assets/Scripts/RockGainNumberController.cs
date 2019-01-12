using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockGainNumberController : MonoBehaviour {

	[SerializeField]
	private float _upSpeed;
	[SerializeField]
	private float _upTime;
	[SerializeField]
	private float _fadeRate;

	private Text textBox;

	public void SetRockGain(int rg=1) {
		this.GetComponent<Text> ().text = "+" + rg.ToString ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3 (0f, _upSpeed * Time.deltaTime, 0f);
		_upTime -= Time.deltaTime;
		if (_upTime < 0f) {
			StartCoroutine ("fadeAwayAndDelete");
		}
	}

	public void ResetFadeAway() {
		StopAllCoroutines ();
	}

	public void ResetUpTime() {
		_upTime = 1;
		Color temp = this.GetComponent<Text> ().color;
		temp.a = 1;
		this.GetComponent<Text> ().color = temp;
	}

	IEnumerator fadeAwayAndDelete() {
		while (true) {
			Color temp = this.GetComponent<Text> ().color;
			temp.a = temp.a - _fadeRate;
			this.GetComponent<Text> ().color = temp;
			if (this.GetComponent<Text> ().color.a <= 0) {
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		this.gameObject.SetActive (false);
	}
}
