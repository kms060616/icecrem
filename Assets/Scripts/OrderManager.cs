using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    private string[] iceCreamFlavors = { "�ٴҶ�", "���ݸ�", "����", "����", "�ٳ���" };
    private string[] toppings = { "�ø���", "������Ŭ" };

    private string[] orderTemplates = {
        "���� {0}���� �ֽð�, ������ {1}���� �ּ���",
        "{0} �� ���̽�ũ���� {1} ���� ��Ź�ؿ�",
        "�� {1} ���� {0} ���̽�ũ���� �����ؿ�",
        "{0} �� ���̽�ũ�� �ϳ��� {1} �߰����ּ���",
        "{0}���� �ֽð��, {1} ���ε� �־��ּ���",
        "��, {0}, {1}���� �� "
    };

    public int numberOfOrders = 1;

    void Start()
    {
        GenerateOrders();
    }

    private void GenerateOrders()
    {
        string fullOrder = "";

        for (int i = 0; i < numberOfOrders; i++)
        {
            string flavor = iceCreamFlavors[Random.Range(0, iceCreamFlavors.Length)];
            string topping = toppings[Random.Range(0, toppings.Length)];
            string template = orderTemplates[Random.Range(0, orderTemplates.Length)];

            string orderLine = string.Format(template, flavor, topping);

            fullOrder += orderLine;

            if (i < numberOfOrders - 1)
                fullOrder += "\n";
        }

        orderText.text = fullOrder;
    }
}