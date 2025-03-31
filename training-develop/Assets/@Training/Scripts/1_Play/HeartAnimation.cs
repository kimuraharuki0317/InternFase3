using UnityEngine;

/// <summary>
/// 体力アニメーションクラス
/// </summary>
public class HeartAnimation : SpriteAnimation
{
    [SerializeField, Header("体力を確認するプレイヤー")]
    PlayerComponent Player;

    [SerializeField, Header("担当するHP数")]
    int HP;

    void Update()
    {
        // プレイヤーのHPが担当するHP数を下回ったらハートが壊れるアニメーションを再生
        if (Player.HitPoint < HP) {
            PlayAnimation();
        }
    }
}
