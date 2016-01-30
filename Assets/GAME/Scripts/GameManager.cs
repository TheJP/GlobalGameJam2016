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
    public GameObject flameItemIndicatorPrefab;
    public GameObject flameItemIndicatorPanel;

    private HashSet<Targets> acquiredTargets = new HashSet<Targets>();

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
        //Add player movement with keyboard inputs
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

    /// <summary>Adds the given target to the set of acquired targets in this room.</summary>
    public void AcquiredTarget(Targets target)
    {
        if (Enum.IsDefined(typeof(Targets), target))
        {
            foreach(var player in gridManager.Units.Where(u => u is Actor).Select(u => u as Actor))
            {
                var floor = player.Cell.GetComponent<FloorTile>();
                if(floor != null && floor.Rune != null && floor.Rune.Value == target) { player.RemoveLater(); }
            }
            acquiredTargets.Add(target);
            UpdateTargets();
        }
    }

    /// <summary>Removes the given target from the set of acquired targets in this room.</summary>
    public void LostTarget(Targets target) { acquiredTargets.Remove(target); UpdateTargets(); }

    /// <summary>Checks if the specified target was yet acquired.</summary>
    public bool HasAcquiredTarget(Targets target) { return acquiredTargets.Contains(target); }

    private void UpdateTargets()
    {
        //Remove existing indicators
        var children = new List<GameObject>();
        foreach (Transform child in flameItemIndicatorPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        //Add new indicators to show the progress
        var scale = 0f;
        foreach (var target in acquiredTargets)
        {
            var flameIndicator = Instantiate(flameItemIndicatorPrefab);
            var indicatorScript = flameIndicator.GetComponent<FlameItemIndicator>();
            if (indicatorScript != null) { indicatorScript.Target = target; }
            flameIndicator.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            flameIndicator.transform.position += scale * Vector3.left;
            scale += 40;
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
