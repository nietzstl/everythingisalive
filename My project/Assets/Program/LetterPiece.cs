using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPiece : MonoBehaviour
{
    public char letter;
    private SpriteRenderer letterRenderer;
    private Rigidbody rb;
    private bool isRestored;
    private void Start()
    {
        Initialize(letter);
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    private void Update()
    {
        LevelPuzzleManager.Instance.TryRestore(this);
    }
    public void Initialize(char c)
    {
        letter = c;
        rb = GetComponent<Rigidbody>();
        letterRenderer = GetComponentInChildren<SpriteRenderer>(); // ʵ�ֻ�ȡ��Ӧ��ĸ��Sprite
        LevelPuzzleManager.Instance.RegisterLetter(this);
    }

    public void Scatter(Vector2 force)
    {
        if (isRestored) return;
        rb.AddForce(force, ForceMode.Impulse);
    }
    public void EnterReadyRestoreArea()
    {
        rb.drag = 10f; // ���������������ƶ��ٶ�
        rb.angularDrag = 0.1f; // ���ӽ�������������ת�ٶ�
    }
    public void RestoreTo(Transform shadow)
    {
        isRestored = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        transform.position = shadow.position;
        transform.rotation = shadow.rotation;

        // ������ӻ�ԭ��Ч
        letterRenderer.color = Color.green; // ʾ������ɫ��ʾ��ԭ
    }

    
}
