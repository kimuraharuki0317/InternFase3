using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤークラス
/// </summary>
public class PlayerComponent : MonoBehaviour
{
    /// <summary>
    /// インプットアクションを定義
    /// </summary>
    InputSystem_Actions inputSystemActions;

    /// <summary>
    /// 押下移動量
    /// </summary>
    Vector3 inputMove;

    /// <summary>
    /// 移動方向を保持する変数
    /// </summary>
    public PhaseManager.Direction MoveDirection;

    [SerializeField, Header("座標を制限する絶対値")]
    Vector2 POSLimit;

    [SerializeField, Header("移動速度")]
    float SpeedMove;

    /// <summary>
    /// 障害物のタグを照合する時に使う文字列
    /// </summary>
    const string Hurdle_Key = "Hurdle";

    [SerializeField, Header("弾の発射方向確認用")]
    PhaseManager PM;

    [SerializeField, Header("弾の複製元")]
    GameObject Bullet;

    /// <summary>
    /// プレイヤーの体力
    /// </summary>
    public int HitPoint { get; private set; }

    [SerializeField, Header("最大体力")]
    int HitPointMax;

    [SerializeField, Header("ヒットエフェクトの複製元")]
    GameObject HitEffect;

    [SerializeField, Header("ゲームオーバー時に使用するフェードパネル")]
    Fade PanelFade;

    void Start()
    {
        // インプットアクションを取得
        inputSystemActions = new InputSystem_Actions();

        // アクションにイベントを登録
        inputSystemActions.Player.Move.started += OnMove;
        inputSystemActions.Player.Move.performed += OnMove;
        inputSystemActions.Player.Move.canceled += OnMove;
        inputSystemActions.Player.Jump.started += OnSpawnBullet;

        // インプットアクションを機能させる為に有効化
        inputSystemActions.Enable();

        // 状態の初期化
        inputMove = Vector3.zero;
        HitPoint = HitPointMax;
    }

    void Update()
    {
        // 無限ヒール(デバッグ用)
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            HitPoint = HitPointMax;
        }

        switch (MoveDirection) {
        case PhaseManager.Direction.Up:
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), SpeedMove * Time.deltaTime);
            break;
        case PhaseManager.Direction.Down:
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), SpeedMove * Time.deltaTime);
            break;
        case PhaseManager.Direction.Left:
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 90), SpeedMove * Time.deltaTime);
            break;
        case PhaseManager.Direction.Right:
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 270), SpeedMove * Time.deltaTime);
            break;
        }

        Move();
    }

    // インプットアクションを他の画面で呼び出さないように無効化
    void OnDestroy()
        => inputSystemActions.Disable();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Hurdle_Key)) {
            HitPoint--;

            // ゲームオーバー
            if (HitPoint <= 0) {
                PanelFade.StateScene = GameManager.StateScene.Result;
                PanelFade.StartedFade = true;
            } else {
                // ヒットエフェクトの発生
                Instantiate(HitEffect, transform.position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// 押下状態 / スティックの倒れ具合を取得
    /// </summary>
    /// <param name="context">WASD, ↑↓←→, スティックの入力値</param>
    void OnMove(InputAction.CallbackContext context)
        => inputMove = context.ReadValue<Vector2>();

    void Move()
    {
        // 押下されていない / 十分にジョイスティックが倒れていない判定
        if (inputMove.sqrMagnitude < 0.01f) {
            return;
        }

        // 入力方向への移動
        transform.position += inputMove.normalized * SpeedMove * Time.deltaTime;

        // 移動制限
        transform.position =
            new Vector2
            (
                Mathf.Clamp(transform.position.x, -POSLimit.x, POSLimit.x),
                Mathf.Clamp(transform.position.y, -POSLimit.y, POSLimit.y)
            );
    }

    /// <summary>
    /// 弾の発射準備を行う
    /// </summary>
    /// <param name="context">Space, Button Southの入力</param>
    void OnSpawnBullet(InputAction.CallbackContext context)
    {
        var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletMover>().MoveDirection = PM.OriginDirection;
    }
}
