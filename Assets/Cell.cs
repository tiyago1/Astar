using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Vector2 Position;

    private int fValue;
    private int gValue;
    private int hValue;

    public int F
    {
        get
        {
            return fValue;
        }
        set
        {
            fValue = value;
            FText.text = value.ToString();
        }
    }
    public int G
    {
        get
        {
            return gValue;
        }
        set
        {
            gValue = value;
            GText.text = value.ToString();
        }
    }
    public int H
    {
        get
        {
            return hValue;
        }
        set
        {
            hValue = value;
            HText.text = value.ToString();
        }
    }

    public bool IsWalkable = true;

    public Cell ParentCell;

    public Text FText;
    public Text GText;
    public Text HText;
    public Text PositionText;

    private Image mImage;

    public void Init(Vector2 position)
    {
        Position = position;
        PositionText.text = (int)position.x + ":" + (int)position.y;
        mImage = this.GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        mImage.color = color;
    }
}
