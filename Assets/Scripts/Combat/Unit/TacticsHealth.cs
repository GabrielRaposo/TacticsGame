using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsHealth : MonoBehaviour {

    [SerializeField] private SpriteRenderer valueDisplay;
    [SerializeField] private Gradient gradient;

    private int maxValue;

    public void Init(int maxValue)
    {
        this.maxValue = maxValue;
        SetValue(maxValue);
    }

    public void SetValue(int value)
    {
        if (valueDisplay != null && maxValue != 0)
        {
            Vector2 size = Vector2.one;
            size.x = (float)value / maxValue;
            valueDisplay.color = gradient.Evaluate(size.x);
            valueDisplay.size = size;
        }
    }
}
