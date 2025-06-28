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
        letterRenderer = GetComponentInChildren<SpriteRenderer>(); // 实现获取对应字母的Sprite
        LevelPuzzleManager.Instance.RegisterLetter(this);
    }

    public void Scatter(Vector2 force)
    {
        if (isRestored) return;
        rb.AddForce(force, ForceMode.Impulse);
    }
    public void EnterReadyRestoreArea()
    {
        rb.drag = 10f; // 增加阻力，减缓移动速度
        rb.angularDrag = 0.1f; // 增加角阻力，减缓旋转速度
    }
    public void RestoreTo(Transform shadow)
    {
        isRestored = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        transform.position = shadow.position;
        transform.rotation = shadow.rotation;

        // 可以添加还原特效
        letterRenderer.color = Color.green; // 示例：变色表示还原
    }

    
}
