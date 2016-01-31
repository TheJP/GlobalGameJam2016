using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Text[] gameStats;
    public GameObject teleportEffect;
    public GameObject gameOverPanel;
    public CellGrid gridManager;
    public ShadowWorldUnit actualPlayer;
    public int playerEscaped = 0;
    public GameObject flameItemIndicatorPrefab;
    public GameObject flameItemIndicatorPanel;
    public AudioClip attackSoundEffect;
    public AudioClip stepSoundEffect;
    public AudioClip teleportSoundEffect;

    private HashSet<Targets> acquiredTargets = new HashSet<Targets>();

    void Awake()
    {
        //Remove unneeded HumanPlayers
        var playersNumber = PlayerPrefs.GetInt("Players_Number", 1);
        var players = gridManager.PlayersParent;
        for (int i = playersNumber; i < players.childCount - 1; ++i)
        {
            Destroy(players.GetChild(i).gameObject);
        }
        //Create shadow lord player
        var enemy = players.GetChild(players.childCount - 1);
        if (PlayerPrefs.GetInt("Enemy_Controlled") == 1)
        {
            enemy.gameObject.AddComponent<ShadowLordActor>();
            var player = enemy.GetComponent<ShadowLordActor>();
            player.PlayerNumber = 6;
            player.cameraController = FindObjectOfType<CameraController>();
            player.manager = this;
        }
        else {
            enemy.gameObject.AddComponent<NaiveAiPlayer>();
            var player = enemy.GetComponent<Player>();
            player.PlayerNumber = 6;
        }
    }

    // Use this for initialization
    void Start () {
        gridManager.GameEnded += GameEnded;
        gridManager.TurnEnded += TurnEnded;
	}

    /// <summary>Plays the given sound and loops it, if the flag is set.</summary>
    /// <param name="sound"></param>
    /// <param name="loop"></param>
    public void PlaySound(AudioClip sound, bool loop = false)
    {
        if (!loop) { GetComponent<AudioSource>().PlayOneShot(sound); }
        else {
            var source = GetComponent<AudioSource>();
            source.loop = true;
            source.clip = sound;
            source.Play();
        }
    }

    /// <summary>Stops all playing sound effects.</summary>
    public void StopSound() { GetComponent<AudioSource>().Stop(); }

    private void TurnEnded(object sender, EventArgs e)
    {
        var add = gridManager.CurrentPlayer is NaiveAiPlayer;
        foreach (var unit in gridManager.Units.Where(u => u.PlayerNumber == 6))
        {
            if (add) { unit.UnitMoved += AiUnitMoved; }
            else { unit.UnitMoved -= AiUnitMoved; }
        }
    }

    private void AiUnitMoved(object sender, MovementEventArgs e)
    {
        var camera = FindObjectOfType<CameraController>();
        if (sender is ShadowWorldUnit)
        {
            camera.target = (sender as ShadowWorldUnit).transform;
        }
    }

    // Update is called once per frame
    void Update () {
        var currentPlayer = gridManager.CurrentPlayer;
        if (Input.GetButtonDown("NextTurn"))
        {
            if (currentPlayer is PlayerActors) { gridManager.EndTurn(); }
            else if (currentPlayer is ShadowLordActor)
            {
                if(!(currentPlayer as ShadowLordActor).NextMonster()) { gridManager.EndTurn(); }
            }
        }
        if (Input.GetButtonDown("ThrowItem"))
        {
            if (actualPlayer is Actor) { (actualPlayer as Actor).ThrowItem(); }
        }
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
                else
                {
                    //Attack units if it's occupied
                    var attackedUnit = gridManager.Units.Where(u => u.Cell == cell).FirstOrDefault();
                    if(attackedUnit != null) { actualPlayer.DealDamage(attackedUnit); }
                }
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
                if (floor != null && floor.Rune != null && floor.Rune.Value == target)
                {
                    FindObjectOfType<GameManager>().playerEscaped++;
                    player.RemoveLater();
                }
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
            flameIndicator.transform.SetParent(flameItemIndicatorPanel.transform, false);
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
        gameOverPanel.SetActive(true);
        gameStats[0].text = PlayerPrefs.GetInt("Worlds_Escaped").ToString();
        gameStats[1].text = PlayerPrefs.GetInt("Enemies_Killed").ToString();
        gameStats[2].text = PlayerPrefs.GetInt("Killed_Players").ToString();
        gameStats[3].text = PlayerPrefs.GetInt("Damage_Dealt").ToString();
        gameStats[4].text = PlayerPrefs.GetInt("Damage_Taken").ToString();
    }

    public void ReturnToMainMenu() {
        FindObjectOfType<StartOptions>().ReturnToMainMenu();
    }

    void StageClear() {
        PlayerPrefs.SetInt("Worlds_Escaped", PlayerPrefs.GetInt("Worlds_Escaped") + 1);
        FindObjectOfType<StartOptions>().StartButtonClicked();
    }

    public void SpawnTeleport(Vector3 pos) {
        Instantiate(teleportEffect, pos, Quaternion.identity);
    }
}
