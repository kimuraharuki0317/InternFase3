using UnityEngine;

/// <summary>
/// 障害物クラス
/// </summary>
public class HurdleComponent : MonoBehaviour
{
    [SerializeField, Header("移動の役割")]
    PhaseManager.Direction MoveDirection;

    [SerializeField, Header("自由落下用")]
    Rigidbody2D RB2D;

    [SerializeField, Header("目標地点の絶対値")]
    Vector2 POSGoal;

    /// <summary>
    /// 開始時のx座標
    /// </summary>
    float POSStartX;

    /// <summary>
    /// 開始時のy座標
    /// </summary>
    float POSStartY;

    [SerializeField, Header("sin波の幅")]
    float WidthSin;

    [SerializeField, Header("sin波の周期")]
    float SpeedSin;

    [SerializeField, Header("sin波の基準を決めるx座標")]
    float POSDecideX;

    [SerializeField, Header("sin波の基準を決めるy座標")]
    float POSDecideY;

    [SerializeField, Header("移動速度")]
    float SpeedMove;

    /// <summary>
    /// 動き出すまでの時間(生成時、個体の順番別に設定)
    /// </summary>
    public float IntervalMoveStart;

    /// <summary>
    /// 障害物の体力
    /// </summary>
    int hitPoint;

    [SerializeField, Header("最大体力")]
    int HitPointMax;

    /// <summary>
    /// 弾のタグを照合する時に使う文字列
    /// </summary>
    const string Bullet_Key = "Bullet";

    [SerializeField, Header("ヒットエフェクトの複製元")]
    GameObject HitEffect;

    void Start()
    {
        MakeWeightless();

        // 状態の初期化
        hitPoint = HitPointMax;
    }

    void Update()
        => Move();

    void OnEnable()
        => MakeWeightless();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Bullet_Key)) {
            Destroy(collision.gameObject);
            hitPoint--;

            if (hitPoint <= 0) {
                // ヒットエフェクトの発生
                Instantiate(HitEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 役割ごとに移動処理を行う
    /// </summary>
    void Move()
    {
        // 移動開始するまで、インターバルを持たせる
        if (IntervalMoveStart > 0f) {
            IntervalMoveStart -= Time.deltaTime;
            return;
        }

        switch (MoveDirection) {
        default:
            break;
        case PhaseManager.Direction.Up:
            if (RB2D.bodyType == RigidbodyType2D.Dynamic) {
                break;
            }

            // 自由落下運動開始
            RB2D.bodyType = RigidbodyType2D.Dynamic;
            break;
        case PhaseManager.Direction.Down:
            // sin波の基準x座標決定
            if (Mathf.Abs(transform.position.x) == POSDecideX) {
                POSStartX = transform.position.x;
            }

            // sin波移動中
            transform.Translate(SpeedMove * Vector3.up * Time.deltaTime);
            transform.position = new Vector3(
                POSStartX + (WidthSin * Mathf.Sin(SpeedSin * Time.time)),
                transform.position.y);
            break;
        case PhaseManager.Direction.Left:
            transform.Translate(SpeedMove * Vector3.right * Time.deltaTime);
            break;
        case PhaseManager.Direction.Right:
            // sin波の基準y座標決定
            if (Mathf.Abs(transform.position.y) == POSDecideY) {
                POSStartY = transform.position.y;
            }

            // sin波移動中
            transform.Translate(SpeedMove * Vector3.left * Time.deltaTime);
            transform.position = new Vector3(
                transform.position.x,
                POSStartY + (WidthSin * Mathf.Sin(SpeedSin * Time.time)));
            break;
        }

        // 画面の枠外なら、移動終了
        if (Mathf.Abs(transform.position.x) >= POSGoal.x || Mathf.Abs(transform.position.y) >= POSGoal.y) {
            MakeWeightless();
            GetComponent<HurdleComponent>().enabled = false;
        }
    }

    /// <summary>
    /// 無重力化
    /// </summary>
    void MakeWeightless()
    {
        RB2D.linearVelocity = new Vector2(0, 0);
        RB2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
