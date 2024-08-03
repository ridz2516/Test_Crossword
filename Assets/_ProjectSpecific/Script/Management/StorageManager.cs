using Sirenix.OdinInspector;
using UnityEngine;

public class StorageManager : Singleton<StorageManager>
{
    [ShowInInspector]
    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(CurrentLevel), 1);
        }
        set
        {
            PlayerPrefs.SetInt(nameof(CurrentLevel), value);
            if (value > HighScoreLevel)
            {
                HighScoreLevel = value;
            }
        }
    }

    [ShowInInspector]
    public int HighScoreLevel { get { return PlayerPrefs.GetInt(nameof(HighScoreLevel), 0); } set { PlayerPrefs.SetInt(nameof(HighScoreLevel), value); } }

    [ShowInInspector]
    public float PlayerMoney
    {
        get
        {
            return PlayerPrefs.GetFloat(nameof(PlayerMoney), 0);
        }
        set
        {
            PlayerPrefs.SetFloat(nameof(PlayerMoney), value); MoneyAmountChanged(value);
        }
    }

    public void MoneyAmountChanged(float i_Amount)
    {
        OnMoneyAmountChanged?.Invoke(i_Amount);
    }

    public delegate void CoinsAmountChangedEvent(float i_CoinsAmount);
    public static event CoinsAmountChangedEvent OnMoneyAmountChanged = delegate { };
}
