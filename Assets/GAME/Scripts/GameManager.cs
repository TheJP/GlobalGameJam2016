using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject teleportEffect;
    public CellGrid gridManager;
    public Actor actualPlayer;
    public int playerEscaped = 0;

	// Use this for initialization
	void Start () {
        FindObjectOfType<CellGrid>().GameEnded += GameEnded;
	}

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("NextTurn"))
            gridManager.EndTurn();
        if (Input.GetButtonDown("ThrowItem"))
            actualPlayer.ThrowItem();
        if(actualPlayer != null)
        {
            Vector3? position = null;
            var horizontal = Input.GetAxis("Horizontal");
            if (System.Math.Abs(horizontal) > 0.0001)
            {
                var xScale = actualPlayer.GetComponent<SpriteRenderer>().bounds.size.x;
                position = actualPlayer.transform.position + (horizontal < 0 ? -1 : 1) * Vector3.right * xScale;
            }
            else
            {
                var vertical = Input.GetAxis("Vertical");
                if(System.Math.Abs(vertical) > 0.0001)
                {
                    var yScale = actualPlayer.GetComponent<SpriteRenderer>().bounds.size.y;
                    position = actualPlayer.transform.position + (vertical < 0 ? -1 : 1) * Vector3.up * yScale;
                }
            }
            if(position.HasValue)
            {
                var cell = gridManager.Cells.OrderBy(h => Math.Abs((h.transform.position - position.Value).magnitude)).First();
                if (!cell.IsTaken) { actualPlayer.Move(cell, new List<Cell>() { cell }); }
            }
        }
	}

    public void GameEnded(object sender, System.EventArgs e) {
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

    public void SpawnTeleport(Vector3 pos) {
        Instantiate(teleportEffect, pos, Quaternion.identity);
    }
}
