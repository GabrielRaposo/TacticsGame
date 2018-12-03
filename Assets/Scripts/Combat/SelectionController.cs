using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    [Header("UI References")]
    [SerializeField] private PlayerPanel playerPanel;

    private Tile currentTile;
    private GameObject currentSelection;

    public static bool disabled;

    public void CallClick()
    {
        if (disabled) return;

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.down * .2f);
        if (hit && hit.collider.tag == "Tile")
        {
            Tile t = hit.collider.GetComponent<Tile>();
            if (t)
            {
                //Se o bloco tiver ocupado por algo, interage com o que está em cima dele
                if (t.ocupation != null)
                {
                    SetTile(t);
                    ExitCurrentSelection();
                    SetSelection(t.ocupation);
                }
                //Se não, verifica se o bloco é selecionável
                else if (t.Selectable)
                {
                    if(currentSelection != null)
                    {
                        // passar pro PlayerPanel porque vai ter as infos de skills e opções de movimento selecionadas
                        currentSelection.GetComponent<TacticsUnit>().SetActionTarget(t);
                        playerPanel.ClosePanel();
                    }
                }
                //Se clicar em um bloco com nada, cancela a ação atual
                else
                {
                    playerPanel.CancelButton();
                }
            }
        }
    }

    private void SetTile(Tile tile)
    {
        if (currentTile != null)
        {
            currentTile.Current = false;
        }
        tile.Current = true;
        currentTile = tile;
    }

    private void ExitCurrentSelection()
    {
        if (currentSelection)
        {
            switch (currentSelection.tag)
            {
                case "Enemy":
                case "Player":
                    PlayerMove previousPlayerMove = currentSelection.GetComponent<PlayerMove>();
                    if (previousPlayerMove)
                    {
                        previousPlayerMove.RemoveSelectedTiles();
                    }

                    break;
            }
        }
    }

    private void SetSelection(GameObject selection)
    {
        currentSelection = selection;
        TacticsUnit tacticsUnit = selection.GetComponent<TacticsUnit>();
        switch (selection.tag)
        {
            case "Player":
                if (tacticsUnit)
                {
                    if (tacticsUnit.CheckStamina())
                    {
                        playerPanel.StartMainPanel(tacticsUnit);
                    }
                    else
                    {
                        playerPanel.StartInfoPanel(tacticsUnit);
                    }
                }
                break;

            case "Enemy":
            case "Other":
                if (tacticsUnit)
                {
                    playerPanel.StartInfoPanel(tacticsUnit);
                }
                break;
        }

        TacticsCamera.FocusOn(selection);
    }
}
