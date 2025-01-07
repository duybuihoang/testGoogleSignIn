using UnityEngine;
using TMPro;
using System.Collections;

public class UIMoneyDisplayer : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    [SerializeField] private MoneyManager manager;

    private void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    /*    protected virtual IEnumerator Start()
        {
            yield return new WaitUntil(() => MoneyManager.Instance != null);
            MoneyManager.Instance.OnMoneyChanged += SetText;
        }*/

    protected virtual void OnEnable()
    {
        Debug.Log("UIMoneyDisplayer");
        MoneyManager.Instance.OnMoneyChanged += SetText;
        //manager.OnMoneyChanged += SetText;
    }

    protected virtual void OnDisable()
    {
        manager.OnMoneyChanged -= SetText;
    }

    private void SetText(float amount)
    {
        moneyText.text = amount.ToString();
    }    
}
