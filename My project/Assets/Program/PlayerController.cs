using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Force Settings")]
    public float forceRadius = 5f;    // �������ð뾶
    public float forceMagnitude = 10f; // ���Ĵ�С
    public LayerMask floorLayer;      // Floor�Ĳ㼶������Unity�༭�������ã�
    public LayerMask affectedLayers;  // ����������㼶����Rigidbody���壩

    private Camera mainCamera;
    private InputAction mouseClickAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        // ��ʼ��Input Action
        mouseClickAction = new InputAction(binding: "<Mouse>/leftButton");
        mouseClickAction.performed += OnMouseClick;
    }

    private void OnEnable() => mouseClickAction.Enable();
    private void OnDisable() => mouseClickAction.Disable();

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        print("ok");
        // ��������������ߵ����λ��
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // ����Ƿ�����Floor��
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
        {
            ApplyForceToNearbyObjects(hit.point);
        }
    }

    private void ApplyForceToNearbyObjects(Vector3 hitPoint)
    {
        // ��ȡ���ð뾶�ڵ�����Rigidbody
        Collider[] colliders = Physics.OverlapSphere(hitPoint, forceRadius, affectedLayers);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // �������ķ��򣨴����е�ָ�����壩
                Vector3 forceDirection = (collider.transform.position - hitPoint).normalized;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }

    // ���ӻ��������÷�Χ�������ã�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, forceRadius);
    }
}
