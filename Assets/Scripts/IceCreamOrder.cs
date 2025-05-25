using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]

public class IceCreamOrder
{
    public string customerName;      // �մ� �̸�
    public string dialogue;          // �մ� ���
    public string[] requiredFlavors; // �ֹ��� ���̽�ũ�� ��
    public Sprite characterImage;    //�մ� �̹���

    public string coneType;      // ��: "Cone" or "Cup"
    public string[] scoopFlavors;

}
