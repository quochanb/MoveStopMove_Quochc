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

    [SerializeField] private Image loading;
    [SerializeField] private int countDownTime;
    [SerializeField] private TextMeshProUGUI countDownDisplay;

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
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        GameManager.Instance.OnFail();
    }

    //xu ly khi hoi sinh player
    private void OnRevive()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        int playerCoin = UserDataManager.Instance.GetUserCoin();

        if (playerCoin >= reviveFee)
        {
            Close(0);
            UIManager.Instance.OpenUI<UIGamePlay>();
            playerCoin -= reviveFee;
            UserDataManager.Instance.UpdateUserCoin(playerCoin);
            GameManager.Instance.OnGamePlay();
            reviveEvent?.Invoke();
        }
    }

    //count down
    IEnumerator CountDown()
    {
        countDownTime = 5;
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownTime.ToString();
            SoundManager.Instance.PlaySound(SoundType.Count);
            yield return Cache.GetWFS(1f);
            countDownTime--;
        }
        countDownDisplay.text = "0";
        SoundManager.Instance.PlaySound(SoundType.Count);

        yield return Cache.GetWFS(0.5f);
        OnClose();
    }
}
