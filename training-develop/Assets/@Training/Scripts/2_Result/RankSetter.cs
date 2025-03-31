using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ランキング設定クラス
/// </summary>
public class RankSetter : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのスコア")]
    TextMeshProUGUI TXTPlayerScore;

    [SerializeField, Header("ランキングのスコア")]
    TextMeshProUGUI[] TXTRankingScore;

    /// <summary>
    /// プレイヤーのスコア
    /// </summary>
    float scorePlayer;

    /// <summary>
    /// ランクそれぞれのスコア
    /// </summary>
    List<float> scoreRank = new List<float>();

    /// <summary>
    /// プレイヤースコアを取得、設定するためのキー
    /// </summary>
    const string Player_Score_Key = "PlayerScore";

    /// <summary>
    /// ランキングの1位スコアを取得、設定するためのキー
    /// </summary>
    const string Rank1_Score_Key = "Rank1";

    void Start()
    {
        CallRankData();
        UpdateRank();
    }

    void Update()
    {
        // escでランキングリセット
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            scoreRank.Clear();

            // テキスト初期化
            for (var i = 0; i < TXTRankingScore.Length; i++) {
                TXTRankingScore[i].text = $"{i + 1} | _._s";
                TXTPlayerScore.color = Color.white;
            }

            // データ領域のリセット
            PlayerPrefs.DeleteAll();
        }
    }

    /// <summary>
    /// ランクのスコアが
    /// 保存されている場合: データ領域から呼び出す
    /// 保存されていない場合: データ領域を初期化する
    /// </summary>
    void CallRankData()
    {
        // プレイヤーのスコアを呼び出す
        scorePlayer = PlayerPrefs.GetFloat(Player_Score_Key);
        TXTPlayerScore.text = $"{scorePlayer}s";
        TXTPlayerScore.color = Color.red;

        // リスト初期化
        scoreRank.Clear();

        if (PlayerPrefs.HasKey(Rank1_Score_Key)) {
            // データ領域の読み込み
            for (var i = 1; i <= TXTRankingScore.Length; i++) {
                scoreRank.Add(PlayerPrefs.GetFloat("Rank" + i));
            }
        } else {
            // データ領域の初期化
            for (var i = 1; i < TXTRankingScore.Length; i++) {
                PlayerPrefs.SetFloat("Rank" + i, 0f);
            }
        }
    }

    /// <summary>
    /// ランク付けをして、表示する
    /// </summary>
    void UpdateRank()
    {
        // プレイヤーのスコアを追加し、0を除いて昇順ソート
        scoreRank.Add(scorePlayer);
        scoreRank = scoreRank.Where(x => x != 0).OrderBy(x => x).ToList();

        // データ領域に保存
        for (var i = 1; i <= scoreRank.Count && i <= TXTRankingScore.Length; i++) {
            PlayerPrefs.SetFloat("Rank" + i, scoreRank[i - 1]);
        }

        for (var i = 0; i < TXTRankingScore.Length; i++) {
            // テキストを更新
            if (i < scoreRank.Count) {
                TXTRankingScore[i].text = $"{i + 1} | {scoreRank[i]}s";
            } else {
                TXTRankingScore[i].text = $"{i + 1} | _._s";
            }
        }

        for (var i = scoreRank.Count - 1; 0 <= i; i--) {
            // 今回のスコアを赤字にする
            if (i < TXTRankingScore.Length && scoreRank[i] == scorePlayer) {
                TXTRankingScore[i].color = Color.red;
                break;
            }
        }
    }
}
