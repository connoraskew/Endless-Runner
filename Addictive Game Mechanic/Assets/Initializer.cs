using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class Initializer : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		GameAnalytics.Initialize ();
	}
}
