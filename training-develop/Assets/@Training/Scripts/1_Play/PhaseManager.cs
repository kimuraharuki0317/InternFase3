using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェーズ管理クラス
/// </summary>
public class PhaseManager : MonoBehaviour
{
    [SerializeField, Header("フェーズ間隔を表すゲージ")]
    Image IMGPhaseGauge;

    /// <summary>
    /// 毎フェーズの時間計測用変数
    /// </summary>
    float timerPhase;

    /// <summary>
    /// 障害物の出現やスクロール方向を示す列挙型
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 上方向
        /// </summary>
        Up,

        /// <summary>
        /// 下方向
        /// </summary>
        Down,

        /// <summary>
        /// 左方向
        /// </summary>
        Left,

        /// <summary>
        /// 右方向
        /// </summary>
        Right,
    }

    /// <summary>
    /// 方向を保持する変数
    /// </summary>
    public Direction OriginDirection { get; private set; }

    [SerializeField, Header("障害物の複製元")]
    GameObject[] HurdlePrefab;

    [SerializeField, Header("クリア判定後のタイマー処理用")]
    Timer TXTTimer;

    /// <summary>
    /// 障害物を方向ごとに管理する変数
    /// </summary>
    Dictionary<Direction, List<GameObject>> hurdles = new Dictionary<Direction, List<GameObject>>();

    /// <summary>
    /// CSV化する項目
    /// </summary>
    enum ItemCSV
    {
        Name,
        NumberOfGenerations,
        IntervalMoveStart,
        PositionStartX,
        PositionStartY,
        PositionSpace,
        IntervalPhase,
    }

    /// <summary>
    /// 各項目のcsv値を保持する変数
    /// </summary>
    Dictionary<ItemCSV, float[]> itemCSV = new Dictionary<ItemCSV, float[]>();

    [SerializeField, Header("方向を順応させるPlayer")]
    PlayerComponent Player;

    [SerializeField, Header("方向の付与対象(背景のスクロールクラス)")]
    BackgroundScroller Background;

    [SerializeField, Header("ゲームクリア時に使用するフェードパネル")]
    Fade PanelFade;

    void Start()
    {
        // フェーズ表示の初期化
        IMGPhaseGauge.fillAmount = 1f;
        IMGPhaseGauge.color = Color.green;

        LoadCSV();

        GenerateHurdle(Direction.Up);
        GenerateHurdle(Direction.Down);
        GenerateHurdle(Direction.Left);
        GenerateHurdle(Direction.Right);

        ResetPhase();

        SetHurdleParameter();

        // 方向付与
        Player.MoveDirection = OriginDirection;
        Background.ScrollDirection = OriginDirection;
    }

    void Update()
    {
        // 時間経過による、ゲージの減少と色の変化
        // fillAmountの上限は1なので、
        // 色の処理: 1-0.5は緑から黄、0.5-0は黄から赤
        timerPhase -= Time.deltaTime;
        var ratioPhase = timerPhase / itemCSV[ItemCSV.IntervalPhase][(int)OriginDirection];
        IMGPhaseGauge.fillAmount = ratioPhase;

        if (ratioPhase < 0.5f) {
            IMGPhaseGauge.color = Color.Lerp(Color.red, Color.yellow, ratioPhase * 2);
        } else {
            IMGPhaseGauge.color = Color.Lerp(Color.yellow, Color.green, (ratioPhase - 0.5f) * 2);
        }

        if (timerPhase <= 0f) {
            ResetPhase();
        }
    }

    /// <summary>
    /// 障害物を生成する
    /// </summary>
    /// <param name="direction">方向(障害物の役割)</param>
    void GenerateHurdle(Direction direction)
    {
        hurdles[direction] = new List<GameObject>();

        for (var i = 0; i < itemCSV[ItemCSV.NumberOfGenerations][(int)direction]; i++) {
            hurdles[direction].Add(Instantiate(HurdlePrefab[(int)direction], transform.position, Quaternion.identity));
            hurdles[direction][i].GetComponent<HurdleComponent>().enabled = false;
        }
    }

    /// <summary>
    /// 障害物のパラメータを設定する
    /// </summary>
    void SetHurdleParameter()
    {
        // タイマーリセット
        timerPhase = itemCSV[ItemCSV.IntervalPhase][(int)OriginDirection];

        switch (OriginDirection) {
        default:
            break;
        case Direction.Up:
        case Direction.Down:
            for (var i = 0; i < hurdles[OriginDirection].Count; i++) {
                var j = itemCSV[ItemCSV.PositionStartX][(int)OriginDirection] - i * itemCSV[ItemCSV.PositionSpace][(int)OriginDirection];
                hurdles[OriginDirection][i].transform.position = new Vector3(j, itemCSV[ItemCSV.PositionStartY][(int)OriginDirection]);
                hurdles[OriginDirection][i].GetComponent<HurdleComponent>().enabled = true;
                hurdles[OriginDirection][i].GetComponent<HurdleComponent>().IntervalMoveStart = itemCSV[ItemCSV.IntervalMoveStart][(int)OriginDirection] + i;
            }
            break;
        case Direction.Left:
        case Direction.Right:
            for (var i = 0; i < hurdles[OriginDirection].Count; i++) {
                var j = itemCSV[ItemCSV.PositionStartY][(int)OriginDirection] - i * itemCSV[ItemCSV.PositionSpace][(int)OriginDirection];
                hurdles[OriginDirection][i].transform.position = new Vector3(itemCSV[ItemCSV.PositionStartX][(int)OriginDirection], j);
                hurdles[OriginDirection][i].GetComponent<HurdleComponent>().enabled = true;
                hurdles[OriginDirection][i].GetComponent<HurdleComponent>().IntervalMoveStart = itemCSV[ItemCSV.IntervalMoveStart][(int)OriginDirection];
            }
            break;
        }
    }

    /// <summary>
    /// フェーズをリセットする
    /// </summary>
    void ResetPhase()
    {
        // 有効な方向の確認
        var validDirections = new List<Direction>();

        for (var i = 0; i <= (int)Direction.Right; i++) {
            var direction = (Direction)i;

            // nullではないGameObjectの確認
            hurdles[direction].RemoveAll(GO => GO == null);

            // GameObjectが全部nullではないものだけ採用
            if (hurdles[direction].Count != 0) {
                validDirections.Add(direction);
            }
        }

        // 有効な方向がない場合はゲームクリア
        if (!validDirections.Any()) {
            TXTTimer.FinishTimer();
            PanelFade.StateScene = GameManager.StateScene.Result;
            PanelFade.StartedFade = true;
            return;
        }

        Direction tmpDirection;

        // 方向の抽選
        do {
            tmpDirection = (Direction)Random.Range(0, (int)Direction.Right + 1);

            if (validDirections.Contains(tmpDirection)) {
                break;
            }
        } while (true);

        // 方向の確定
        OriginDirection = tmpDirection;

        SetHurdleParameter();

        // 方向付与
        Player.MoveDirection = OriginDirection;
        Background.ScrollDirection = OriginDirection;
    }

    /// <summary>
    /// csvの値を変数に取り込む
    /// </summary>
    void LoadCSV()
    {
        // CSVの読み込み(引用)
        var csvFile = Resources.Load("parameters") as TextAsset; // ResourcesにあるCSVファイルを格納

        using (var reader = new StringReader(csvFile.text)) { // TextAssetをStringReaderに変換
            var csvData = new List<string[]>(); // CSVファイルの中身を入れるリスト

            while (reader.Peek() != -1) {
                csvData.Add(reader.ReadLine().Split(',')); // csvDataリストに追加する
            }

            // 値の代入
            itemCSV[ItemCSV.NumberOfGenerations] = new float[(int)Direction.Right + 1];
            itemCSV[ItemCSV.IntervalMoveStart] = new float[(int)Direction.Right + 1];
            itemCSV[ItemCSV.PositionStartX] = new float[(int)Direction.Right + 1];
            itemCSV[ItemCSV.PositionStartY] = new float[(int)Direction.Right + 1];
            itemCSV[ItemCSV.PositionSpace] = new float[(int)Direction.Right + 1];
            itemCSV[ItemCSV.IntervalPhase] = new float[(int)Direction.Right + 1];

            for (var i = 0; i <= (int)Direction.Right; i++) {
                itemCSV[ItemCSV.NumberOfGenerations][i] = float.Parse(csvData[(int)ItemCSV.NumberOfGenerations][i + 1]);
                itemCSV[ItemCSV.IntervalMoveStart][i] = float.Parse(csvData[(int)ItemCSV.IntervalMoveStart][i + 1]);
                itemCSV[ItemCSV.PositionStartX][i] = float.Parse(csvData[(int)ItemCSV.PositionStartX][i + 1]);
                itemCSV[ItemCSV.PositionStartY][i] = float.Parse(csvData[(int)ItemCSV.PositionStartY][i + 1]);
                itemCSV[ItemCSV.PositionSpace][i] = float.Parse(csvData[(int)ItemCSV.PositionSpace][i + 1]);
                itemCSV[ItemCSV.IntervalPhase][i] = float.Parse(csvData[(int)ItemCSV.IntervalPhase][i + 1]);
            }
        }
    }
}
