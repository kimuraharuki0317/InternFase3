using UnityEngine;

/// <summary>
/// マテリアルの画像をUVスクロールする
/// </summary>
public class UVScroll : MonoBehaviour
{
    [SerializeField, Header("背景のマテリアル取得用")]
    SpriteRenderer Background;

    [SerializeField, Header("スクロール速度")]
    float SlowMagnification;

    /// <summary>
    /// offsetの上限値
    /// </summary>
    [SerializeField]
    float OffsetMax = 1f;

    /// <summary>
    /// マテリアルの画像をdirectionに基づいてUVスクロールさせる
    /// </summary>
    /// <param name="direction">背景をスクロールする向き</param>
    public void Scroll(Vector2 direction)
        => Background.material.SetTextureOffset("_MainTex", direction * Mathf.Repeat(SlowMagnification * Time.time, OffsetMax));
}
