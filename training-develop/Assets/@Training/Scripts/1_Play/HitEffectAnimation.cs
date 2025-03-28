using UnityEngine;

/// <summary>
/// ヒットエフェクトのアニメーションクラス
/// </summary>
public class HitEffectAnimation : MonoBehaviour
{
    /// <summary>
    /// 画像編集用コンポーネント
    /// </summary>
    SpriteRenderer hitEffect;

    [SerializeField, Header("ヒットエフェクト画像")]
    Sprite[] Explosion;

    /// <summary>
    /// アニメーション間隔を管理する変数
    /// </summary>
    float intervalAnimation;

    [SerializeField, Header("アニメーション間隔")]
    float IntervalAnimationMax = 0.1f;

    /// <summary>
    /// 現在の画像の添字
    /// </summary>
    uint effectIndex;

    void Start()
    {
        // 画像の初期化
        hitEffect = GetComponent<SpriteRenderer>();
        effectIndex = 0;
        hitEffect.sprite = Explosion[effectIndex];

        // アニメーション間隔の初期化
        intervalAnimation = 0f;
    }

    void Update()
        => AnimationHitEffect();

    /// <summary>
    /// ヒットエフェクトのアニメーションを行う
    /// </summary>
    void AnimationHitEffect()
    {
        if (intervalAnimation < IntervalAnimationMax) {
            // アニメーションのインターバル中
            intervalAnimation += Time.deltaTime;
            return;
        }

        // アニメーション
        if (effectIndex == Explosion.Length - 1) {
            Destroy(gameObject);
        } else {
            effectIndex++;
            hitEffect.sprite = Explosion[effectIndex];
        }
        intervalAnimation = 0f;
    }
}
