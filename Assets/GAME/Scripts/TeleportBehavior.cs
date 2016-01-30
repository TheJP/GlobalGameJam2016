using UnityEngine;
using System.Collections;

public class TeleportBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyObject() {
        Destroy(this.gameObject);
    }
}
