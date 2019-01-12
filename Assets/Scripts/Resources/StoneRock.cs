using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRock {

	private BaseRock rock;

	public StoneRock () {
		rock = new BaseRock ("Stone", 5, 1);
	}

	public string GetName() {
		return rock.GetName ();
	}

	public void Mine(int toolStrength) {
		rock.MineRock (toolStrength);
	}


}
