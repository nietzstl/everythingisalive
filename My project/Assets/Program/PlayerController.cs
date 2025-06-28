using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Force Settings")]
    public float forceRadius = 5f;    // 力的作用半径
    public float forceMagnitude = 10f; // 力的大小
    public LayerMask floorLayer;      // Floor的层级（需在Unity编辑器中设置）
    public LayerMask affectedLayers;  // 受力的物体层级（如Rigidbody物体）

    private Camera mainCamera;
    private InputAction mouseClickAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        // 初始化Input Action
        mouseClickAction = new InputAction(binding: "<Mouse>/leftButton");
        mouseClickAction.performed += OnMouseClick;
    }

    private void OnEnable() => mouseClickAction.Enable();
    private void OnDisable() => mouseClickAction.Disable();

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        print("ok");
        // 从摄像机发射射线到鼠标位置
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // 检测是否命中Floor层
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
        {
            ApplyForceToNearbyObjects(hit.point);
        }
    }

    private void ApplyForceToNearbyObjects(Vector3 hitPoint)
    {
        // 获取作用半径内的所有Rigidbody
        Collider[] colliders = Physics.OverlapSphere(hitPoint, forceRadius, affectedLayers);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 计算力的方向（从命中点指向物体）
                Vector3 forceDirection = (collider.transform.position - hitPoint).normalized;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }

    // 可视化力的作用范围（调试用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, forceRadius);
    }
}
