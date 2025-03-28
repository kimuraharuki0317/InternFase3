using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面遷移イベントクラス
/// </summary>
public class SetNextScene : MonoBehaviour
{
    [SerializeField, Header("画面遷移アクション用ボタン")]
    Button BTNNextScene;

    [SerializeField, Header("次に遷移したい画面")]
    GameManager.StateScene StateScene;

    [SerializeField, Header("ゲームオーバー時に使用するフェードパネル")]
    Fade PanelFade;

    void Start()
        => BTNNextScene.onClick.AddListener(OnStartSelectScene);

    /// <summary>
    /// 画面遷移を開始する
    /// </summary>
    void OnStartSelectScene()
    {
        PanelFade.StateScene = StateScene;
        PanelFade.StartedFade = true;
    }
}
