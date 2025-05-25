using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IceCreamBuilder : MonoBehaviour
{

    public List<string> selectedScoops = new List<string>();
    public string selectedConeType = "";

    private Customer customerSystem;

    [Header("���� ���� ������Ʈ")]
    public Transform scoopSpawnPoint; // ������ ���̴� ��ġ ������
    public GameObject[] scoopPrefabs; // �� ���� ���� ������ (����, �ٴҶ� ��)

    private int currentScoopCount = 0; // ���� ���� ����

    

    public GameObject scoopPrefab; // UI �̹��� ������
    public Transform[] scoopSpawnPoints; // �� �� 3�� ��ġ
     
    public Transform scoopParent; // ĵ���� ��, ������� �ö� UI ������Ʈ

    private int scoopCount = 0;



    public void SetCone(string coneType)
    {
        selectedConeType = coneType;
        Debug.Log("������ ��: " + coneType);
    }

    public void AddScoop(string flavor)
    {
        if (scoopCount >= scoopSpawnPoints.Length)
        {
            Debug.LogWarning("���� 3�� �̻� �߰� �Ұ�");
            return;
        }

        GameObject scoop = Instantiate(scoopPrefab, scoopParent);
        scoop.transform.position = scoopSpawnPoints[scoopCount].position;

        Image img = scoop.GetComponent<Image>();
        if (img != null)
            img.sprite = GetSpriteForFlavor(flavor);

        selectedScoops.Add(flavor);
        scoopCount++;
    }

    public void SubmitIceCream()
    {
        IceCreamOrder order = customerSystem.GetCurrentOrder();

        bool coneMatch = order.coneType == selectedConeType; // �� vs �� �� (������ �Ÿ� ���� true�� ���� ����)
        int matchCount = order.scoopFlavors.Intersect(selectedScoops).Count();

        bool isSatisfied = matchCount >= 3;

        customerSystem.OnIceCreamFinished(
            isSatisfied,
            order,
            new List<string>(selectedScoops)
        );

        ClearCup(); // �� ����

        //���̽�ũ�� UI �г� �ݱ�
        if (iceCreamUIPanel != null)
            iceCreamUIPanel.SetActive(false);
    }

    [SerializeField] private GameObject iceCreamUIPanel;


    public void SetCustomerSystem(Customer cs)
    {
        customerSystem = cs;
    }

    private Sprite GetSpriteForFlavor(string flavor)
    {
        // ���� ���� �ٸ� ��������Ʈ ��ȯ (�̸� �����ص־� ��)
        switch (flavor.ToLower())
        {
            case "vanilla": return vanillaSprite;
            case "chocolate": return chocolateSprite;
            case "strawberry": return strawberrySprite;
            default: return defaultScoopSprite;
        }
    }

    private void ClearCup()
    {
        // ���� ����鸸 ����
        List<Transform> toRemove = new List<Transform>();

        foreach (Transform child in scoopParent)
        {
            if (!child.name.StartsWith("ScoopPoint")) //��������Ʈ ����
                toRemove.Add(child);
        }

        foreach (Transform child in toRemove)
        {
            Destroy(child.gameObject);
        }

        selectedScoops.Clear();
        scoopCount = 0;
    }






    public Sprite vanillaSprite;
    public Sprite chocolateSprite;
    public Sprite strawberrySprite;
    public Sprite defaultScoopSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
