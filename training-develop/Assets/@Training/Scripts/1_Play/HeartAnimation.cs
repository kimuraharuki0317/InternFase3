using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 体力アニメーションクラス
/// </summary>
public class HeartAnimation : MonoBehaviour
{
    /// <summary>
    /// 画像編集用コンポーネント
    /// </summary>
    Image IMGHeart;

    [SerializeField, Header("体力を確認するプレイヤー")]
    PlayerComponent Player;

    [SerializeField, Header("担当するHP数")]
    int HP;

    [SerializeField, Header("ハート(破壊)画像")]
    Sprite[] HeartBreak;

    /// <summary>
    /// アニメーション間隔を管理する変数
    /// </summary>
    float intervalAnimation;

    /// <summary>
    /// 現在の画像の添字
    /// </summary>
    uint heartIndex;

    [SerializeField, Header("アニメーション間隔")]
    float IntervalAnimationMax;

    void Start()
    {
        // 画像の初期化
        IMGHeart=GetComponent<Image>();
        heartIndex = 0;
        IMGHeart.sprite = HeartBreak[heartIndex];

        // アニメーション間隔の初期化
        intervalAnimation = 0f;
    }

    void Update()
    {
        if (Player.HitPoint < HP) {
            AnimationHeart(HeartBreak);
        }
    }

    /// <summary>
    /// ハートのアニメーションを行う
    /// </summary>
    /// <param name="spritesHeart">アニメーションするハート画像</param>
    void AnimationHeart(Sprite[] spritesHeart)
    {
        if (intervalAnimation < IntervalAnimationMax) {
            // アニメーションのインターバル中
            intervalAnimation += Time.deltaTime;
            return;
        }

        if (heartIndex == spritesHeart.Length - 1) {
            Destroy(gameObject);
        } else {
            heartIndex++;
            IMGHeart.sprite = spritesHeart[heartIndex];
        }
        intervalAnimation = 0f;
    }
}
