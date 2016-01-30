using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public CellGrid gridManager;
    public Actor actualPlayer;
    public int playerEscaped = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("NextTurn"))
            gridManager.EndTurn();
        if (Input.GetButtonDown("ThrowItem"))
            actualPlayer.ThrowItem();
        
	}

    public void GameEnded() {
        if (playerEscaped > 0)
            StageClear();
        else
            GameOver();
    }

    void GameOver() {
        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu() {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<StartOptions>().ReturnToMainMenu();
    }

    void StageClear() {
        FindObjectOfType<StartOptions>().StartButtonClicked();
    }
}
