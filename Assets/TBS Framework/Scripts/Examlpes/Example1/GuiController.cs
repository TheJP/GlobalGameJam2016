using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    public CellGrid CellGrid;
    public GameObject UnitsParent;
    public Button NextTurnButton;

    public Image UnitImage;
    public Text InfoText;
    public Text StatsText;

    void Start()
    {
        UnitImage.color = Color.gray;

        CellGrid.TurnEnded += OnNextTurn;
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.GameEnded += OnGameEnded;

        foreach (Transform unit in UnitsParent.transform)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
        }

        foreach (Transform cell in CellGrid.transform)
        {
            cell.GetComponent<Cell>().CellHighlighted += OnCellHighlighted;
            cell.GetComponent<Cell>().CellDehighlighted += OnCellDehighlighted;
        }
    }

    private void OnGameStarted(object sender, EventArgs e)
    {
        OnNextTurn(sender,e);
    }

    private void OnGameEnded(object sender, EventArgs e)
    {
        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + " wins!";
    }
    private void OnNextTurn(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);

        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber +1);
    }
    private void OnCellDehighlighted(object sender, EventArgs e)
    {
        UnitImage.color = Color.gray;
        StatsText.text = "";
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        UnitImage.color = Color.gray;
        StatsText.text = "Movement Cost: " + (sender as Cell).MovementCost;
    }
    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;
        OnUnitDehighlighted(sender, new EventArgs());

        if ((sender as Unit).HitPoints <= 0) return;

        OnUnitHighlighted(sender, e);
    }
    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        StatsText.text = "";
        UnitImage.color = Color.gray;
    }
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        var unit = sender as MyUnit;
        StatsText.text = unit.UnitName + "\nHit Points: " + unit.HitPoints +"/"+unit.TotalHitPoints + "\nAttack: " + unit.AttackFactor + "\nDefence: " + unit.DefenceFactor + "\nRange: " + unit.AttackRange;
        UnitImage.color = unit.PlayerColor;

    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
