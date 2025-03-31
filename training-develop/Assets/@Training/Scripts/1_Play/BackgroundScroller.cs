using System.Windows.Forms;
using UnityEngine;

/// <summary>
/// 背景のスクロールクラス
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField, Header("背景のマテリアル取得用")]
    UVScroll BackgroundScroll;

    /// <summary>
    /// スクロール方向を保持する変数
    /// </summary>
    public PhaseManager.Direction ScrollDirection;

    void Update()
    {
        // 背景のスクロール方向を決定
        var scrollVector = Vector2.zero;

        switch (ScrollDirection) {
        default:
            MessageBox.Show(gameObject.name + "BackgroundScrollerコンポーネントのScrollDirectionが未割当", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // 背景をスクロールする
        BackgroundScroll.Scroll(scrollVector);
    }
}
