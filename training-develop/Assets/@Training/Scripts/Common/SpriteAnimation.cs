using UnityEngine;

/// <summary>
/// 複数のスプライトでアニメーションを行うクラス
/// </summary>
public class SpriteAnimation : MonoBehaviour
{
    /// <summary>
    /// 画像編集用コンポーネント
    /// </summary>
    SpriteRenderer spriteRenderer;

    [SerializeField, Header("アニメーション画像")]
    Sprite[] AnimationSprite;
    
    /// <summary>
    /// アニメーション間隔を管理する変数
    /// </summary>
    float intervalAnimation;

    [SerializeField, Header("アニメーション間隔")]
    float IntervalAnimationMax = 0.1f;

    /// <summary>
    /// 現在の画像の添字
    /// </summary>
    uint animationIndex;

    void Start()
    {
        // 画像の初期化
        spriteRenderer = GetComponent<SpriteRenderer>();
        animationIndex = 0;
        spriteRenderer.sprite = AnimationSprite[animationIndex];

        // アニメーション間隔の初期化
        intervalAnimation = 0f;
    }

    /// <summary>
    /// スプライトのアニメーションを行う
    /// </summary>
    protected void Animation()
    {
        if (intervalAnimation < IntervalAnimationMax) {
            // アニメーションのインターバル中
            intervalAnimation += Time.deltaTime;
            return;
        }

        // animationIndexに基づいてアニメーションを行う
        // 0 <= animationIndex < AnimationSprite.Length
        if (animationIndex == AnimationSprite.Length - 1) {
            Destroy(gameObject);
        } else {
            animationIndex++;
            spriteRenderer.sprite = AnimationSprite[animationIndex];
        }
        intervalAnimation = 0f;
    }
}
