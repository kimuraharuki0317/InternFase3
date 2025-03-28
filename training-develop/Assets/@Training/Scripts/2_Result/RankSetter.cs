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
    float[] scoreRank = new float[6];

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
            for (var i = 0; i < TXTRankingScore.Length; i++) {
                scoreRank[i + 1] = 0f;
                TXTRankingScore[i].text = $"{i + 1} | _._s";
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
        TXTPlayerScore.text = $"{scorePlayer:f1}s";
        TXTPlayerScore.color = Color.red;

        if (PlayerPrefs.HasKey(Rank1_Score_Key)) {
            // データ領域の読み込み
            for (var i = 1; i < scoreRank.Length; i++) {
                scoreRank[i] = PlayerPrefs.GetFloat("Rank" + i);
            }
        } else {
            // データ領域の初期化
            for (var i = 1; i < scoreRank.Length; i++) {
                scoreRank[i] = 0f;
                PlayerPrefs.SetFloat("Rank" + i, scoreRank[i]);
            }
        }
    }

    /// <summary>
    /// ランク付けをして、表示する
    /// </summary>
    void UpdateRank()
    {
        var rankNew = 0; // 今回のスコアを最下位と仮定する

        for (int i = scoreRank.Length - 1; i > 0; i--) {
            // 昇順 1...5
            if (scorePlayer != 0f) {
                if (scoreRank[i] == 0f || scoreRank[i] >= scorePlayer) {
                    // ランク番号の記録
                    rankNew = i;
                }
            }
        }

        // 同じスコアがなく、または新しいランクが見つかったら
        if (rankNew != 0) {
            // 0位のままでなかったらランクイン確定
            for (int i = scoreRank.Length - 1; i > rankNew; i--) {
                // 繰り下げ処理
                scoreRank[i] = scoreRank[i - 1];
            }

            // 新ランクに登録
            scoreRank[rankNew] = scorePlayer;

            for (var i = 1; i < scoreRank.Length; i++) {
                // データ領域に保存
                PlayerPrefs.SetFloat("Rank" + i, scoreRank[i]);
            }
        }

        // テキストに表示
        for (var i = 0; i < TXTRankingScore.Length; i++) {
            if (scoreRank[i + 1] == 0f) {
                TXTRankingScore[i].text = $"{i + 1} | _._s";
            } else {
                TXTRankingScore[i].text = $"{i + 1} | {scoreRank[i + 1]:f1}s";

                if (i + 1 == rankNew) {
                    TXTRankingScore[i].color = Color.red;
                }
            }
        }
    }
}
