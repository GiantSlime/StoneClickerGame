using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseRock {

	private string _name;
	private int _durablity;
	private int _density;

	public BaseRock () {
		_name = "UNNAMED";
		_durablity = 1;
		_density = 1;
	}

	public BaseRock (string name, int durability=1, int density=1) {
		_name = name;
		_durablity = durability;
		_density = density;
	}

	public string GetName() {
		return _name;
	}

	public void MineRock(int toolStrength=1) {
		_durablity -= (int)Mathf.Floor(toolStrength / _density);
	}
}
