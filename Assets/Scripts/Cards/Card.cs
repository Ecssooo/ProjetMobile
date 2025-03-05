using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] CardType _cardType;

    [SerializeField] SpriteRenderer _darkenedEffect;

    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canSwap;

    public CardType CardType
    {
        get { return _cardType; }
    }

    [Tooltip("None if anything other than KnightSword")]
    [SerializeField]
    private Direction _direction;

    public Direction Direction
    {
        get => _direction;
        set => _direction = value;
    }

    [SerializeField] private Direction _attackDirection;

    public Direction AttackDirection
    {
        get => _attackDirection;
        set => _attackDirection = value;
    }

    [SerializeField] private int _foodValue;

    public int FoodValue
    {
        get => _foodValue;
    }

    [SerializeField] private Vector2Int _positionOnBoard;

    public Vector2Int PositionOnBoard
    {
        get { return _positionOnBoard; }
        set { _positionOnBoard = value; }
    }

    [SerializeField] private TextMeshProUGUI _monsterScoreTXT;


    [SerializeField] private List<Transform> _actionSlots = new List<Transform>();

    public List<Transform> ActionSlots
    {
        get { return _actionSlots; }
        set { _actionSlots = value; }
    }

    [SerializeField] private Animator _animator;

    public Animator Animator
    {
        get { return _animator; }
    }

    private void OnMouseDown()
    {
        switch (GameManager.Instance.Effect)
        {
            case Effects.NONE:
                break;
            case Effects.MOVE:
                StartCoroutine(EffectActions.Instance.MoveCardCoroutine((direction) =>
                {
                    this._direction = direction;
                    Action moveAction = EffectActions.Instance.CreateAction(this);
                    ListAction.Instance.AddAction(moveAction);
                }));
                break;
            case Effects.SWAP:
                Card card = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))
                    .GetComponent<Card>();
                if (card == null) return;
                if (EffectActions.Instance._swapFirstCard == null)
                {
                    EffectActions.Instance._swapFirstCard = card;
                }
                else
                {
                    if (EffectActions.Instance._swapFirstCard == card) return;
                    EffectActions.Instance._swapSecondCard = card;
                    Action switchAction = EffectActions.Instance.CreateAction(EffectActions.Instance._swapFirstCard,
                        EffectActions.Instance._swapSecondCard);

                    ListAction.Instance.AddAction(switchAction);

                    //EffectActions.Instance.DoEffect(switchAction);
                    EffectActions.Instance._swapFirstCard = null;
                    EffectActions.Instance._swapSecondCard = null;
                }

                break;
        }

        Collider2D effectClicked = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void DoEndOfTurnActions() // To use at end of turn to make Knights attack
    {

        switch (CardType)
        {
            case CardType.HUMAN:
                break;
            case CardType.KNIGHTSWORD:
                Card target = GameManager.Instance.Board.GetCardClose(this.PositionOnBoard, this._attackDirection);
                if (target != null)
                {
                    if (target.CardType == CardType.MINIMONSTER) PlayGamesController.Instance.UnlockAchievement("CgkImLeVnfkcEAIQCg");
                    StartCoroutine(target.OnDie());
                }
                else AudioManager.Instance.PlaySFX("swordSlash");
                break;
            case CardType.KNIGHTSHIELD:
                break;
            case CardType.MONSTER:
                break;
            case CardType.MINIMONSTER:
                break;
            case CardType.CAULDRON:
                break;
        }
    }

    public IEnumerator OnDie()
    {
        if (this._cardType == CardType.HUMAN)
        {
            _animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySFX("swordHit");
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.Board.ClearSlot(this);
            GameManager.Instance.MonsterScore += _foodValue;
            GameManager.Instance.HumanKill++;
            AudioManager.Instance.PlaySFX("death");
        }
        else if (this._cardType == CardType.KNIGHTSHIELD)
        {
            _animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySFX("swordClang");
            yield return new WaitForSeconds(0.5f);
            PlayGamesController.Instance.UnlockAchievement("CgkImLeVnfkcEAIQDg");

        }
        else if (this._cardType == CardType.MONSTER)
        {
            _animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySFX("swordHit");
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.MonsterScore = 0;
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.GameDefeatStateState);
            GameManager.Instance.Board.ClearSlot(this);
        }
        else if (this._cardType == CardType.NONE)
        {
            AudioManager.Instance.PlaySFX("swordSlash");
        }
    }

    public void ShowMonsterScore()
    {
        if(LevelManager.Instance.CurrentLevel >= GameManager.Instance.LevelDatabase.levelList.Count) return;
        _monsterScoreTXT.text = GameManager.Instance.MonsterScore.ToString() + "/" +
                                GameManager.Instance.LevelDatabase.levelList[LevelManager.Instance.CurrentLevel]
                                    .maxScore;
    }

    public void AddAction(Action action)
    {

    }

    private void Update()
    {
        if (_cardType == CardType.MONSTER) ShowMonsterScore();

        if (GameManager.Instance.GameState == GameState.Playable)
        {
            if (_cardType == CardType.KNIGHTSWORD)
            {
                if (GameManager.Instance.Effect == Effects.INVOKE)
                {
                    _animator.SetBool("Dark", true);
                }
                else
                {
                    _animator.SetBool("Dark", false);
                }
            }
            else
            {
                if (GameManager.Instance.Effect == Effects.MOVE && !_canMove)
                {
                    _darkenedEffect.gameObject.SetActive(true);
                    // Color color = _darkenedEffect.color;
                    // color.a = .8f;
                    // _darkenedEffect.color = color;
                }
                else if (GameManager.Instance.Effect == Effects.SWAP && !_canSwap)
                {
                    _darkenedEffect.gameObject.SetActive(true);
                    // Color color = _darkenedEffect.color;
                    // color.a = .8f;
                    // _darkenedEffect.color = color;
                }
                else if (GameManager.Instance.Effect == Effects.INVOKE)
                {
                    _darkenedEffect.gameObject.SetActive(true);
                    // Color color = _darkenedEffect.color;
                    // color.a = .8f;
                    // _darkenedEffect.color = color;
                }
                else
                {
                    _darkenedEffect.gameObject.SetActive(false);
                    // Color color = _darkenedEffect.color;
                    // color.a = 0f;
                    // _darkenedEffect.color = color;
                }
            }
        }
        else
        {
            if (_cardType == CardType.KNIGHTSWORD) _animator.SetBool("Dark", false);
            else _darkenedEffect.gameObject.SetActive(false);
        }
    }
}
