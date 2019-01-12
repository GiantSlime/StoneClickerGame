using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabMovementController : MonoBehaviour {

	private bool _isTabOpen;

	public float yRate;
	public float distanceCount;
	public float distanceBase;

	public Text buttonText;

	public void SwitchTabState() {
		if (_isTabOpen) {
			StartCoroutine ("CloseTab");
			_isTabOpen = false;
			buttonText.text = "Shop";
		} else {
			StartCoroutine ("OpenTab");
			_isTabOpen = true;
			buttonText.text = "Close";
		}
	}

	IEnumerator OpenTab() {
		while (true) {
			this.transform.parent.position = this.transform.parent.position + new Vector3 (0f, yRate * Time.deltaTime, 0f);
			if (this.transform.parent.position.y > distanceCount) {
				this.transform.parent.position = new Vector3 (this.transform.parent.position.x, distanceCount, this.transform.parent.position.z);
				break;
			}
			yield return new WaitForSeconds (0.016f);
		}
	}

	IEnumerator CloseTab() {
		while (true) {
			this.transform.parent.position = this.transform.parent.position - new Vector3 (0f, yRate * Time.deltaTime, 0f);
			if (this.transform.parent.position.y < distanceBase) {
				this.transform.parent.position = new Vector3 (this.transform.parent.position.x, distanceBase, this.transform.parent.position.z);
				break;
			}
			yield return new WaitForSeconds (0.016f);
		}
	}

}
