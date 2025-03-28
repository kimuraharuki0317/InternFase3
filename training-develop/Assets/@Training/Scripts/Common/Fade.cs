using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェード用クラス
/// </summary>
public class Fade : MonoBehaviour
{
    [SerializeField, Header("フェード画像")]
    Image PanelFade;

    /// <summary>
    /// フェード開始状態を保持する変数
    /// </summary>
    public bool StartedFade;

    /// <summary>
    /// 遷移先のシーンを管理する変数
    /// </summary>
    public GameManager.StateScene StateScene;

    void Start()
    {
        // フェード状態の初期化
        PanelFade.fillAmount = 1f;
        StartedFade = false;
    }

    void Update()
    {
        if (StartedFade) {
            // フェードアウト中
            PanelFade.enabled = true;
            PanelFade.fillAmount += Time.deltaTime;

            if (PanelFade.fillAmount >= 1f) {
                // 次のシーンへ
                GameManager.Instance.ChangeScene = StateScene;
            }
        } else if (PanelFade.fillAmount <= 0f) {
            PanelFade.enabled = false;
        } else {
            // フェードイン中
            PanelFade.fillAmount -= Time.deltaTime;
        }
    }
}
