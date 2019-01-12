using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAnimationController : MonoBehaviour {

	public Animator anim;

	public void AnimationReset() {
		anim.Play ("Pressed", -1, 0f);
	}

}
