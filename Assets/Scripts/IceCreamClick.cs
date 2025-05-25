using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCreamClick : MonoBehaviour, IPointerClickHandler
{
    public string flavor; // �� ������Ʈ�� �� �̸�

    public void OnPointerClick(PointerEventData eventData)
    {
        IceCreamBuilder builder = FindObjectOfType<IceCreamBuilder>();
        if (builder != null)
        {
            builder.AddScoop(flavor);
        }
    }
}
