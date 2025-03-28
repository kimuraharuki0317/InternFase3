using UnityEngine;

/// <summary>
/// 背景のスクロールクラス
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField, Header("背景のマテリアル取得用")]
    SpriteRenderer Background;

    /// <summary>
    /// offsetの上限値
    /// </summary>
    const float OffsetMax = 1f;

    [SerializeField, Header("遅くする倍率(小数点単位で記入)")]
    float SlowMagnification;

    /// <summary>
    /// スクロール方向を保持する変数
    /// </summary>
    public PhaseManager.Direction ScrollDirection;

    void Update()
    {
        // 背景のスクロール方向を決定
        Vector2 scrollVector;

        switch (ScrollDirection) {
        default:
            scrollVector = Vector2.zero;
            break;
        case PhaseManager.Direction.Up:
            scrollVector = Vector2.up;
            break;
        case PhaseManager.Direction.Down:
            scrollVector = Vector2.down;
            break;
        case PhaseManager.Direction.Left:
            scrollVector = Vector2.left;
            break;

        case PhaseManager.Direction.Right:
            scrollVector = Vector2.right;
            break;
        }

        ScrollBackground(scrollVector);
    }

    /// <summary>
    /// 背景をスクロールする
    /// </summary>
    /// <param name="scrollVector">背景をスクロールする向き</param>
    void ScrollBackground(Vector2 scrollVector)
        => Background.material.SetTextureOffset("_MainTex", scrollVector * Mathf.Repeat(SlowMagnification * Time.time, OffsetMax));
}
