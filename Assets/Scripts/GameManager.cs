using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Effects _effect;
    public Effects Effect
    {
        get => _effect;
        set => _effect = value;
    }
    #region Instance

    private static GameManager _instance;

    public static GameManager Instance { get => _instance; }

    public void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EffectList.Effects = Effects.NONE;
        EffectList.MoveCard = false;
        EffectList.SwapCard = false;

    }
    #endregion

    [SerializeField] private Board _board;
    public Board Board { get => _board; }

    [SerializeField] private LevelDatabase _levelDatabase;
    public LevelDatabase LevelDatabase {get => _levelDatabase; }

    [SerializeField] private ListAction _listAction;
    public ListAction ListActions { get => _listAction; }

    [SerializeField] private ActionCount _actionCount;
    public ActionCount ActionCount { get => _actionCount; }

    private int _monsterScore;
    public int MonsterScore { get => _monsterScore; set => _monsterScore = value; }


    // [SerializeField] private TextMeshProUGUI levelCountText;
    // [SerializeField] private TextMeshProUGUI ActionCountText;
    //
    // private int actionCount;
    //
    // private int mogscore;
    //
    // private Vector2 cardposition;
    //
    // private bool Win = false;
    //
    // void Start()
    // {
    //     UpdateLevelCountText();
    // }
    //
    // void Update()
    // {
    //
    // }
    //
    // private void UpdateLevelCountText()
    // {
    //     // Level level = new Level(new List<Card>());
    //     if (Win==true)
    //     {
    //         //level.level++;
    //     }
    //     if (levelCountText != null)
    //     {
    //         // levelCountText.text = "Level: " + level.level;
    //     }
    // }
    //
    // private void Reset()
    // {
    //     // Level level = new Level(new List<Card>());
    //     // actionCount = level.maxActionCount;
    //     // mogscore = level.maxScore;
    // }
}