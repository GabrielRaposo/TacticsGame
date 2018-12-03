using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDataDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleDisplay;
    [SerializeField] TextMeshProUGUI levelDisplay;
    [SerializeField] Image iconDisplay;

    [Header("Combat Stats")]
    [SerializeField] TextMeshProUGUI hpCurrentDisplay;
    [SerializeField] TextMeshProUGUI hpMaxDisplay;
    [SerializeField] TextMeshProUGUI strengthDisplay;
    [SerializeField] TextMeshProUGUI intelligenceDisplay;
    [SerializeField] TextMeshProUGUI defenseDisplay;
    [SerializeField] TextMeshProUGUI resistenceDisplay;
    [SerializeField] TextMeshProUGUI speedDisplay;
    [SerializeField] TextMeshProUGUI luckDisplay;
    [SerializeField] TextMeshProUGUI movementDisplay;

    private string format = "00";

    public void SetUnit(TacticsUnit tacticsUnit)
    {
        titleDisplay.text = tacticsUnit.title;
        levelDisplay.text = "LV " + tacticsUnit.level;

        hpCurrentDisplay.text = tacticsUnit.hp.ToString(format);
        hpMaxDisplay.text = tacticsUnit.maxHp.ToString(format);
        strengthDisplay.text = tacticsUnit.strength.ToString(format);
        intelligenceDisplay.text = tacticsUnit.intelligence.ToString(format);
        defenseDisplay.text = tacticsUnit.defense.ToString(format);
        resistenceDisplay.text = tacticsUnit.resistence.ToString(format);
        speedDisplay.text = tacticsUnit.speed.ToString(format);
        luckDisplay.text = tacticsUnit.luck.ToString(format);

        movementDisplay.text = tacticsUnit.movement.ToString();
    }
}
