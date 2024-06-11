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

    [SerializeField] private int countDownTime;
    [SerializeField] private TextMeshProUGUI countDownDisplay;
    [SerializeField] private Image loading;

    private int reviveFee = 150;

    public static event Action reviveEvent;

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        reviveBtn.onClick.AddListener(OnRevive);
    }

    private void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        loading.rectTransform.Rotate(0, 0, -360 * Time.deltaTime);
    }

    //xu ly khi nhan nut Close
    private void OnClose()
    {
        GameManager.Instance.OnFail();
        //UNDONE
    }

    //xu ly khi hoi sinh player
    private void OnRevive()
    {
        int playerCoin = UserDataManager.Instance.GetUserCoin();

        if (playerCoin >= reviveFee)
        {
            Close(0);
            UIManager.Instance.OpenUI<UIJoystick>();
            UIManager.Instance.OpenUI<UIGamePlay>();
            playerCoin -= reviveFee;
            UserDataManager.Instance.UpdateUserCoin(playerCoin);
            GameManager.Instance.OnGamePlay();
            reviveEvent?.Invoke();
        }
        //TODO: Tru coin, hoi sinh player
    }

    //count down
    IEnumerator CountDown()
    {
        countDownTime = 5;
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownTime.ToString();

            yield return Cache.GetWFS(1f);
            countDownTime--;
        }
        countDownDisplay.text = "0";

        yield return Cache.GetWFS(1f);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIFail>();
    }
}
