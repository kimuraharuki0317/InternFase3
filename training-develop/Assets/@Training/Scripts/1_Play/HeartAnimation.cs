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

    [SerializeField, Header("アニメーション間隔")]
    float IntervalAnimationMax;

    void Start()
    {
        // 画像の初期化
        IMGHeart = GetComponent<Image>();
        IMGHeart.sprite = HeartBreak[0];

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

        // アニメーション
        intervalAnimation = 0f;
        for (var i = 0; i < spritesHeart.Length; i++) {
            if (IMGHeart.sprite == spritesHeart[i]) {
                if (i == spritesHeart.Length - 1) {
                    if (spritesHeart == HeartBreak) {
                        Destroy(gameObject);
                        break;
                    }

                    // 最初の画像に戻す
                    IMGHeart.sprite = spritesHeart[0];
                    break;
                } else {
                    // 次の画像へ
                    IMGHeart.sprite = spritesHeart[i + 1];
                    break;
                }
            } else if (i == spritesHeart.Length - 1) {
                // 画像を変更する
                IMGHeart.sprite = spritesHeart[0];
                break;
            }
        }
    }
}
