using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterShadow : MonoBehaviour
{
    public char letter { get; private set; }
    public bool isOccupied { get; private set; }
    private SpriteRenderer shadowRenderer;

    public void Initialize(char c, Color shadow_color,Sprite sprite)
    {
        letter = c;
        GameObject spriteObj = new GameObject($"Shadow_{c}_sprite");
        spriteObj.transform.SetParent(this.transform);
        shadowRenderer = spriteObj.AddComponent<SpriteRenderer>();
        Quaternion rotX = Quaternion.Euler(90, 0, 0);
        spriteObj.transform.rotation = rotX;
        spriteObj.transform.position = transform.position;
        shadowRenderer.sprite = sprite;
        shadowRenderer.color = shadow_color;
        shadowRenderer.sortingOrder = -1;
    }
    
    public void Occupy(LetterPiece piece)
    {
        isOccupied = true;
        shadowRenderer.enabled = false;
    }

    
}
