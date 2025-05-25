using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class Customer : MonoBehaviour
{
    // �մ� ��� ǥ�ÿ�
    public TextMeshProUGUI dialogueText;
    public GameObject orderPanel;    // �ֹ� UI ��ü
    public Button acceptButton;      // ���� ��ư
    public Button rejectButton;      // ���� ��ư
    public Image characterImageUI;

    private int currentDay = 1;           // ���� ��¥
    private int customersPerDay = 5;      // ���� ���� �մ� ��
    private int customersServed = 0;      // ���� ���� �մ� ��

    private IceCreamOrder currentOrder;

    public IceCreamOrder[] orders;   // �մ� �ֹ� ���
    private string dialogue;

    private List<IceCreamOrder> todayCustomerList = new List<IceCreamOrder>();
    private int customerIndex = 0;

    [SerializeField] private GameObject daySummaryPanel; // ��� �г�
    [SerializeField] private TextMeshProUGUI goodCountText;
    [SerializeField] private TextMeshProUGUI normalCountText;
    [SerializeField] private TextMeshProUGUI badCountText;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private Button retryButton;

    private int goodCount = 0;
    private int normalCount = 0;
    private int badCount = 0;

    void Start()
    {
        StartNewDay(); // ù�� ����

    }

    void StartNewDay()
    {
        // ���� ������ �մ� ����Ʈ �ʱ�ȭ
        todayCustomerList = orders.OrderBy(x => Random.value).Take(customersPerDay).ToList();
        customerIndex = 0;

        goodCount = 0;
        normalCount = 0;
        badCount = 0;

        ShowNextCustomer();
    }



    void ShowNextCustomer()
    {

        if (customerIndex >= todayCustomerList.Count)
            return;

        currentOrder = todayCustomerList[customerIndex];
        customerIndex++;


        characterImageUI.sprite = currentOrder.characterImage;
        characterImageUI.gameObject.SetActive(true);
        characterImageUI.color = Color.white;
        StartCoroutine(FadeInImage(characterImageUI, 0.5f));

        dialogueText.text = currentOrder.dialogue;
        orderPanel.SetActive(true);

        // ��ư �̺�Ʈ �ʱ�ȭ
        acceptButton.onClick.RemoveAllListeners();
        rejectButton.onClick.RemoveAllListeners();

        // ��ư ��� ����
        acceptButton.onClick.AddListener(AcceptOrder);
        rejectButton.onClick.AddListener(RejectOrder);
    }



    void AcceptOrder()
    {
        orderPanel.SetActive(false);
        Debug.Log("�ֹ� ����: " + currentOrder.customerName);

        // ���̽�ũ�� ����� UI �ѱ�
        iceCreamUIPanel.SetActive(true);

        // IceCreamBuilder�� ���� �մ� �ý��� ����
        iceCreamBuilder.SetCustomerSystem(this);


    }

    void RejectOrder()
    {
        orderPanel.SetActive(false);
        Debug.Log("�ֹ� ����: " + currentOrder.customerName);

        StartCoroutine(FadeOutImage(characterImageUI, 0.5f));

        customersServed++; //���� ������Ŵ

        if (customersServed >= customersPerDay)
        {
            Debug.Log("�Ϸ� ����!");
            ShowDaySummary(); //�г� �����ֱ�
            currentDay++;
            customersPerDay = 5 + (currentDay - 1);
            customersServed = 0;

            Invoke(nameof(StartNewDay), 2f);
        }
        else
        {
            Invoke(nameof(ShowNextCustomer), 2f);
        }
    }

    void StartIceCreamMaking()
    {
        // ���÷� 3�� �ڿ� ���̽�ũ���� �ϼ��ƴٰ� ����
        StartCoroutine(FinishMakingIceCreamAfterDelay(3f));
    }

    IEnumerator FinishMakingIceCreamAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("���̽�ũ�� ���� �Ϸ�!");
        CheckNextCustomer(); // ������ ���� �ڿ� ���� �մ�
    }

    void CheckNextCustomer()
    {
        customersServed++;

        if (customersServed >= customersPerDay)
        {
            Debug.Log("�Ϸ� ����!");

            ShowDaySummary(); //�Ϸ� ����� �����ְ� ��ư ���� ������ ���!
        }
        else
        {
            Invoke(nameof(ShowNextCustomer), 2f);
        }
    }



    IEnumerator FadeOutImage(Image img, float duration)
    {
        float elapsed = 0f;
        Color originalColor = img.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        img.gameObject.SetActive(false);
    }

    IEnumerator FadeInImage(Image img, float duration)
    {
        float elapsed = 0f;
        Color originalColor = img.color;

        // ������ ����
        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        img.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    public void OnIceCreamFinished(bool isSatisfied, IceCreamOrder order, List<string> playerScoops)
    {
        //�մ� �䱸 �� ���纻 ����� (�ߺ� Ȯ�� ����)
        List<string> requiredFlavors = order.scoopFlavors
            .Select(f => f.Trim().ToLower())
            .ToList();

        List<string> playerFlavors = playerScoops
            .Select(f => f.Trim().ToLower())
            .ToList();

        int matchCount = 0;

        foreach (string flavor in playerFlavors)
        {
            if (requiredFlavors.Contains(flavor))
            {
                matchCount++;
                requiredFlavors.Remove(flavor); //�ߺ� ������ ���� �ϳ��� ����
            }
        }

        //��
        if (matchCount >= 3)
            goodCount++;
        else if (matchCount == 2)
            normalCount++;
        else
            badCount++;

        //�����
        Debug.Log("�մ� �ֹ�: " + string.Join(", ", order.scoopFlavors));
        Debug.Log("�÷��̾� ����: " + string.Join(", ", playerScoops));
        Debug.Log("��ġ ��: " + matchCount);
        Debug.Log("�մ� �� ���: " + (matchCount >= 3 ? "����" : matchCount == 2 ? "����" : "����"));

        //����
        StartCoroutine(FadeOutImage(characterImageUI, 0.5f));
        Invoke(nameof(CheckNextCustomer), 2f);
    }
    public IceCreamOrder GetCurrentOrder()
    {
        return currentOrder;
    }

    
    [SerializeField] private IceCreamBuilder iceCreamBuilder;
    [SerializeField] private GameObject iceCreamUIPanel; // ���̽�ũ�� ����� UI �г�

    void ShowDaySummary()
    {
        daySummaryPanel.SetActive(true);

        goodCountText.text = $"good: {goodCount}";
        normalCountText.text = $"normal: {normalCount}";
        badCountText.text = $"bad: {badCount}";

        if (goodCount >= 3)
        {
            nextDayButton.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(false);
        }
        else
        {
            nextDayButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(true);
        }

        nextDayButton.onClick.RemoveAllListeners();
        nextDayButton.onClick.AddListener(() =>
        {
            daySummaryPanel.SetActive(false);
            currentDay++;
            customersPerDay = 5 + (currentDay - 1);
            customersServed = 0;
            goodCount = normalCount = badCount = 0;
            StartNewDay();
        });

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() =>
        {
            daySummaryPanel.SetActive(false);
            customersServed = 0;
            goodCount = normalCount = badCount = 0;
            StartNewDay();
        });
    }


    







    // Update is called once per frame
    void Update()
    {
        
    }
}
