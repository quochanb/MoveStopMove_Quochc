using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRevive : UICanvas
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button reviveBtn;

    [SerializeField] private int countDownTime = 5;
    [SerializeField] private TextMeshProUGUI countDownDisplay;
    [SerializeField] private Image loading;

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        reviveBtn.onClick.AddListener(OnRevive);

        if (loading == null)
        {
            loading = FindObjectOfType<Image>();
        }

        StartCoroutine(CountDown());
    }

    private void Update()
    {
        loading.rectTransform.Rotate(0, 0, -360 * Time.deltaTime);
    }

    //xu ly khi nhan nut Close
    private void OnClose()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIFail>();
        GameManager.Instance.OnFinish();
        //UNDONE
    }

    //xu ly khi hoi sinh player
    private void OnRevive()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>();
        UIManager.Instance.OpenUI<UIGamePlay>();
        GameManager.Instance.OnGamePlay();
        //UNDONE
    }

    //count down
    IEnumerator CountDown()
    {
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownTime.ToString();

            yield return Cache.GetWFS(1f);
            countDownTime--;
        }
        countDownDisplay.text = "0";

        yield return Cache.GetWFS(0.3f);
        OnClose();
    }
}
