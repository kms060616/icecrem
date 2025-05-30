using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform orderViewPoint;
    public Transform kitchenViewPoint;
    public float moveSpeed = 5f;

    private Transform targetView;

    void Start()
    {
        targetView = orderViewPoint;
    }

    void Update()
    {
        if (targetView != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetView.position, moveSpeed * Time.deltaTime);
        }
    }

    public void GoToKitchen()
    {
        targetView = kitchenViewPoint;
    }

    public void BackToOrder()
    {
        targetView = orderViewPoint;
    }
}
