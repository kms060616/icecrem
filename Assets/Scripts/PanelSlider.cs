using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlider : MonoBehaviour
{
    public RectTransform kitchenPanel;     // �̵��� �г�
    public Vector2 hiddenPosition;         // ���� ��ġ
    public Vector2 visiblePosition;        // ���� ��ġ
    public float slideSpeed = 5f;

    private bool isSliding = false;

    private bool isSlidingOut = false;

    

    void Update()
    {
        if (isSliding)
        {
            kitchenPanel.anchoredPosition = Vector2.Lerp(
                kitchenPanel.anchoredPosition,
                visiblePosition,
                Time.deltaTime * slideSpeed
            );

            // ���� �����ϸ� ������Ű��
            if (Vector2.Distance(kitchenPanel.anchoredPosition, visiblePosition) < 0.1f)
            {
                kitchenPanel.anchoredPosition = visiblePosition;
                isSliding = false;
            }
        }
        if (isSlidingOut)
        {
            kitchenPanel.anchoredPosition = Vector2.Lerp(
                kitchenPanel.anchoredPosition,
                hiddenPosition,
                Time.deltaTime * slideSpeed
            );

            if (Vector2.Distance(kitchenPanel.anchoredPosition, hiddenPosition) < 0.1f)
            {
                kitchenPanel.anchoredPosition = hiddenPosition;
                isSlidingOut = false;

                // ���ϸ� �г� ����
                // kitchenPanel.gameObject.SetActive(false);
            }
        }

    }

    public void SlideIn()
    {
        kitchenPanel.gameObject.SetActive(true); // �����־��ٸ� ��
        isSliding = true;

        isSlidingOut = false; //�ݴ� ���� ����
    }

    public void SlideOut()
    {
        isSlidingOut = true;
        isSliding = false;
    }
}
