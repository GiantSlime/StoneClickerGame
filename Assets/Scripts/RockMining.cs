using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ages
// 0 = StartingAge
// 1 = StoneAge
// 2 = BronzeAge
// 3 = IronAge
// 4 = IndustrialAge
// 5 = SpaceAge
public enum AGE {
	START,
	STONE,
	BRONZE,
	IRON,
	INDUSTRIAL,
	SPACE
}

public enum ResourceType {
	STONE,
	COPPER,
	IRON,
	TIN,
	BRONZE,
	STEEL,
	CARBON_STEEL
}

public class RockMining : MonoBehaviour {

	// Object Control
	public Text rocksText;
	public Text currentRockText;
	public Text AgeText;

	public Transform rockGainTextObjectPool;
	public int currentObjectNumber;

	public int[] autoMiners;
	public Text[] autoMinersCost;
	public Text[] autoMinersLevel;
	public Button[] autoMinersPurchase;

	public GameObject purchasePanel;

	private float miningProductionValue;
	public Text miningProductionText;

	// Stat Control
	private float[] resources;

	[SerializeField]	
	private int toolMineEffectiveness;
	private int toolMineEfficiency;

	private AGE currentAge;
	private Dictionary<AGE, List<ResourceType>> ageRocks;

	// Use this for initialization
	void Start () {
		currentAge = AGE.STONE;
		ageRocks = new Dictionary<AGE, List<ResourceType>> () {
			{AGE.STONE, new List<ResourceType>() {ResourceType.STONE}},
			{AGE.BRONZE, new List<ResourceType>() {ResourceType.STONE, ResourceType.COPPER, ResourceType.TIN}}
		};
		resources = new float[7]; // Stored in order of ResourceType
		toolMineEffectiveness = 1;
		toolMineEfficiency = 0;
		LoadAllAutoMinerCosts ();
		LoadAMCPurchaseButtons ();
		StartCoroutine ("MiningProductionAutomation");
	}
	
	// Update is called once per frame
	void Update () {
		// Debug Log
		// P to force increment Age
		if (Input.GetKeyDown (KeyCode.P)) {
			if ((int)currentAge < ageRocks.Count) {
				currentAge++;
			}
		} else if (Input.GetKeyDown (KeyCode.O)) {
			purchasePanel.SetActive (!purchasePanel.activeInHierarchy);
		}
	}

	public void BuyAutoMiners(int amID) {
		resources [(int)ResourceType.STONE] -= GetAutoMinerCost (amID);
		UpdateRocksText ();
		autoMiners [amID] += 1;
		UpdateAMCPurchaseButtons ();
		UpdateAutoMinerCosts (amID);
		UpdateAutoMinerLevels (amID);
		UpdateMiningProduction ();
		UpdateMiningProductionText ();
	}

	public int GetAutoMinerCost(int amID) {
		switch (amID) {
		case 0:
			return 10 + autoMiners[amID];
		case 1:
			return 100 + (10 * autoMiners[amID]);
		case 2:
			return 1000 + (100 * autoMiners[amID]);
		case 3:
			return 10000 + (1000 * autoMiners[amID]);
		case 4:
			return 100000 + (10000 * autoMiners[amID]);
		}
		return 0;
	}

	public void BreakRock() {
		GenerateRockGain ();
		UpdateRocksText ();
		GenerateRockGainUsingNextInPool ();
		UpdateAMCPurchaseButtons ();
	}

	public void GenerateRockGain() {
		foreach (ResourceType a in ageRocks[currentAge]) {
			resources [(int)a] += GenerateGainOnType(a);
		}
	}

	public float GenerateGainOnType(ResourceType type) {
		if (type == ResourceType.STONE) {
			return toolMineEffectiveness;
		}
		return 0f;
	}

	public void GenerateRockGainUsingNextInPool() {
		GameObject temp = rockGainTextObjectPool.GetChild (currentObjectNumber).gameObject;
		if (!temp.activeInHierarchy) {
			temp.SetActive (true);
		} else {
			temp.GetComponent<RockGainNumberController> ().ResetFadeAway ();
		}
		temp.transform.position = Input.mousePosition;
		temp.GetComponent<RockGainNumberController> ().SetRockGain (toolMineEffectiveness);
		temp.GetComponent<RockGainNumberController> ().ResetUpTime ();
		currentObjectNumber++;
		if (currentObjectNumber >= 10) {
			currentObjectNumber = 0;
		}
	}

	private void UpdateRocksText() {
		rocksText.text = ((int) resources [(int)ResourceType.STONE]).ToString();
	}

	private void UpdateAgeText() {
		AgeText.text = currentAge.ToString ();
	}

	private void UpdateAMCPurchaseButtons() {
		for (int i = 0; i < autoMiners.Length; i++) {
			if (autoMinersCost [i] != null) {
				if (resources [(int)ResourceType.STONE] >= GetAutoMinerCost (i)) {
					autoMinersPurchase [i].interactable = true;
				} else {
					autoMinersPurchase [i].interactable = false;
				}
			}
		}
	}

	private void UpdateAutoMinerCosts(int amID) {
		autoMinersCost [amID].text = "Cost: " + GetAutoMinerCost (amID).ToString ();
	}

	private void UpdateAutoMinerLevels(int amID) {
		autoMinersLevel [amID].text = "Level: " + autoMiners [amID].ToString();
	}

	private void UpdateMiningProduction() {
		miningProductionValue = autoMiners [0] * 0.1f + autoMiners [1] * 1f + autoMiners [2] * 10f;
	}

	private void UpdateMiningProductionText() {
		miningProductionText.text = "Mining Production: " + miningProductionValue.ToString ("F1");
	}

	private void LoadAllAutoMinerCosts() {
		for (int i = 0; i < autoMiners.Length; i++) {
			if (autoMinersCost [i] != null) {
				UpdateAutoMinerCosts (i);
			}
		}
	}

	private void LoadAMCPurchaseButtons() {
		for (int i = 0; i < autoMiners.Length; i++) {
			if (autoMinersCost [i] != null) {
				if (resources [(int)ResourceType.STONE] < GetAutoMinerCost (i)) {
					autoMinersPurchase [i].interactable = false;
				}
			}
		}
	}
		
	IEnumerator MiningProductionAutomation() {
		while (true) {
			yield return new WaitForSeconds (1f);
			resources [(int)ResourceType.STONE] += miningProductionValue;
			UpdateRocksText ();
		}
	}
}
