using TMPro;
using UnityEngine;

/// <summary>
/// タイマー用クラス
/// </summary>
public class Timer : MonoBehaviour
{
    /// <summary>
    /// クリアまでの時間を計測する変数
    /// </summary>
    float timer;

    [SerializeField, Header("時間表記用")]
    TextMeshProUGUI TXTTimer;

    /// <summary>
    /// プレイヤースコアを取得、設定するためのキー
    /// </summary>
    const string Player_Score_Key = "PlayerScore";

    void Start()
    {
        // 時間計測の開始処理
        PlayerPrefs.SetFloat(Player_Score_Key, 0f);
        timer = 0f;
    }

    void Update()
    {
        // プレイ開始からの時間を計測
        timer += Time.deltaTime;
        TXTTimer.text = timer.ToString("f1") + "s";
    }

    /// <summary>
    /// タイマーを終了する
    /// </summary>
    public void FinishTimer()
        => PlayerPrefs.SetFloat(Player_Score_Key, timer); // ランキング用にタイムを保存し、ゲーム終了
}
