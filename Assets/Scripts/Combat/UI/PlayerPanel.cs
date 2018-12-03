using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{    
    private enum PanelState
    {
        None, //Painel fechado, basicamente
        Main, //Tela com a seleção de botões de ação
        Info, //Tela somente com botão de cancelar e o painel da unidade

        Movement,
        Attack,
        Items
    }
    private PanelState state;
    private TacticsUnit currentUnit;

    public UnitDataDisplay unitDataDisplay;

    [Header("Layout Buttons")]
    [SerializeField] private Button moveButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button itemsButton;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button waitButton;
    [Space(10)]
    [SerializeField] private Button cancelButton;

    void Start ()
    {
        SetPanelState(PanelState.None);
    }

    public void StartMainPanel(TacticsUnit currentUnit)
    {
        this.currentUnit = currentUnit;
        currentUnit.playerPanel = this;
        SetPanelState(PanelState.Main);
    }

    public void StartInfoPanel(TacticsUnit currentUnit)
    {
        this.currentUnit = currentUnit;
        SetPanelState(PanelState.Info);
    }

    public void ClosePanel()
    {
        currentUnit = null;
        SetPanelState(PanelState.None);
    }

    private void SetPanelState(PanelState state)
    {
        this.state = state;
        switch (state)
        {
            case PanelState.None:
                DisablePanel();
                break;

            case PanelState.Main:
                if (currentUnit)
                {
                    //Funciona quando volta da tela de ação para a tela principal
                    currentUnit.GetComponent<TacticsMove>().RemoveSelectedTiles();

                    moveButton.gameObject.SetActive(currentUnit.MovementPoint);
                    attackButton.gameObject.SetActive(currentUnit.ActionPoint);
                    itemsButton.gameObject.SetActive(currentUnit.ActionPoint);

                    unitDataDisplay.SetUnit(currentUnit);
                    unitDataDisplay.gameObject.SetActive(true);
                }
                statsButton.gameObject.SetActive(true);
                waitButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(true);

                break;

            case PanelState.Info:
                DisablePanel();
                if (currentUnit)
                {
                    if (currentUnit.MovementPoint)
                    {
                        currentUnit.GetComponent<TacticsMove>().FindSelectableTiles();
                    }
                    else
                    {
                        currentUnit.GetComponent<TacticsMove>().RemoveSelectedTiles();
                    }

                    unitDataDisplay.SetUnit(currentUnit);
                    unitDataDisplay.gameObject.SetActive(true);
                }
                cancelButton.gameObject.SetActive(true);

                break;
            
            case PanelState.Movement:
                currentUnit.GetComponent<TacticsMove>().FindSelectableTiles();

                DisablePanel();
                cancelButton.gameObject.SetActive(true);
                break;
        }
    }

    private void DisablePanel()
    {
        moveButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        itemsButton.gameObject.SetActive(false);
        statsButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        waitButton.gameObject.SetActive(false);

        unitDataDisplay.gameObject.SetActive(false);
    }

    public void CancelButton()
    {
        switch (state)
        {
            case PanelState.Main:
            case PanelState.Info:
                SetPanelState(PanelState.None);
                break;

            case PanelState.Movement:
            case PanelState.Attack:
                SetPanelState(PanelState.Main);
                break;
        }
    }

    public void MovementButton()
    {
        SetPanelState(PanelState.Movement);
    }

    public void SkipTurn()
    {
        if (currentUnit)
        {
            currentUnit.MovementPoint = false;
            currentUnit.ActionPoint = false;

            SetPanelState(PanelState.None);
        }
    }
}
