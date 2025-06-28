using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPuzzleManager : MonoBehaviour
{
    public static LevelPuzzleManager Instance;
    [Header("Settings")]
    public float positionThreshold = 0.3f;
    public float rotationThreshold = 15f;
    public float forceRadius = 2f;
    public float forceMagnitude = 5f;
    public LayerMask floorLayer;

    [Header("Letter Setup")]
    public Color shadow_color; // 半透明材质

    public List<GameObject> letterPieces = new List<GameObject>();
    private Dictionary<char, List<LetterShadow>> letterShadows = new Dictionary<char, List<LetterShadow>>();
    // Start is called before the first frame update
    private bool start_resume_letter = false;
    private void Awake() => Instance = this;

    private void Start()
    {
        GeneratePuzzle();
        ScatterLetters();
        StartCoroutine(StartResumLetterCoroutine());
    }
    
    IEnumerator StartResumLetterCoroutine()
    {
        yield return new WaitForSeconds(1f);
        start_resume_letter = true;
    }
    private void GeneratePuzzle()
    {
        
        // 生成字母和自动创建印子
        for (int i = 0; i < letterPieces.Count; i++)
        {
            char c = letterPieces[i].GetComponent<LetterPiece>().letter;
            // 自动创建印子
            CreateShadow(c, letterPieces[i].transform, letterPieces[i].GetComponentInChildren<SpriteRenderer>().sprite);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateShadow(char c, Transform t, Sprite sprite)
    {
        GameObject shadowObj = new GameObject($"Shadow_{c}");
        shadowObj.transform.position = t.position;
        shadowObj.transform.rotation = t.rotation;
        shadowObj.transform.localScale = t.localScale;


        var shadow = shadowObj.AddComponent<LetterShadow>();
        
        shadow.Initialize(c, shadow_color,sprite);
        if (!letterShadows.ContainsKey(shadow.letter))
            letterShadows.Add(shadow.letter, new List<LetterShadow>());
        letterShadows[shadow.letter].Add(shadow);
    }

    

    void ScatterLetters()
    {
        foreach (var letter in letterPieces)
        {
            Vector3 randomDir= UnityEngine.Random.insideUnitCircle.normalized;
            letter.GetComponent<LetterPiece>().Scatter(randomDir * 100f);
        }
    }

    public void RegisterLetter(LetterPiece piece)
    {
        letterPieces.Add(piece.gameObject);
    }

    public void ApplyForceAtPosition(Vector2 worldPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, forceRadius);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody && col.GetComponent<LetterPiece>())
            {
                Vector2 forceDir = (col.transform.position - (Vector3)worldPos).normalized;
                col.attachedRigidbody.AddForce(forceDir * forceMagnitude, ForceMode2D.Impulse);
            }
        }
    }

    public bool TryRestore(LetterPiece piece)
    {
        if (!letterShadows.ContainsKey(piece.letter)) return false;

        if(start_resume_letter)
        {
            foreach (var shadow in letterShadows[piece.letter])
            {
                if(shadow.letter == piece.letter)
                {
                    if (!shadow.isOccupied &&
                    Vector3.Distance(piece.transform.position, shadow.transform.position) < positionThreshold &&
                    Quaternion.Angle(piece.transform.rotation, shadow.transform.rotation) < rotationThreshold 
                )
                    {
                        shadow.Occupy(piece);
                        piece.RestoreTo(shadow.transform);
                        return true;
                    }
                }
                
            }
        }
        return false;
    }
}
