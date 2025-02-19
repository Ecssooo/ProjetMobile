using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    [SerializeField] private List<Transform> _effectSlots = new List<Transform>();
    [SerializeField] private LevelDatabase _levelDatabase;
    [SerializeField] private GameObject _effectParent;

    
    //Private field
    private GameObject[] _effectGO = new GameObject[3];
    private Card[,] _board = new Card[4, 3];
    private Transform[,] _slotsTab = new Transform[4, 3];

    //Get
    public Card[,] CardList => _board;
    public Transform[,] SlotsTab => _slotsTab;
    
    
    #region Clear
    
    
    public void ClearSlot(Card card)
    {
        InitSlotTab();  
        if (_slotsTab[card.PositionOnBoard.x, card.PositionOnBoard.y].childCount > 0)
            DestroyImmediate(_slotsTab[card.PositionOnBoard.x, card.PositionOnBoard.y].GetChild(0).gameObject);
        _board[card.PositionOnBoard.x, card.PositionOnBoard.y] = null;
    }

    public void ClearSlot(Vector2Int position)
    {
        InitSlotTab();  
        if (PositionInBounds(position))
        {
            if (_slotsTab[position.x, position.y].childCount > 0)
                DestroyImmediate(_slotsTab[position.x, position.y].GetChild(0).gameObject);
            _board[position.x, position.y] = null;
        }
    }
    
    public void ClearEffect()
    {
        foreach (var go in _effectGO)
        {
            if (go != null)
            {
                DestroyImmediate(go);
            }
        }
    }

    /// <summary>
    /// Clear board
    /// </summary>
    public void ResetBoard()
    {
        InitSlotTab();
        if (_board == null) return;

        foreach (var card in _slots)
        {
            if (card.childCount > 0)
            {
                ClearSlot(card.GetComponentInChildren<Card>());
            }
        }
        
        ClearEffect();
    }
    
    
    
    #endregion

    #region Set
    
    /// <summary>
    /// Transform slot list into 2D array
    /// </summary>
    public void InitSlotTab()
    {
        int index = 0;

        for (int i = 0; i < _slotsTab.GetLength(0); i++)
        {
            for (int j = 0; j < _slotsTab.GetLength(1); j++)
            {
                _slotsTab[i, j] = _slots[index];
                index++;
            }
        }
    }

    
    /// <summary>
    /// Add a Card to a slot in board
    /// </summary>
    /// <param name="card">Cards type, Can be null</param>
    public void SetSlots(CardParams cardParams)
    {
        InitSlotTab();

        var prefab = _levelDatabase.GetPrefab(cardParams.cardType);
        if (prefab == null) return;

        var GO = Instantiate(prefab, this.transform);
        var card = GO.GetComponent<Card>();
        if (card == null) return;
        card.PositionOnBoard = cardParams.positionOnBoard;
        card.AttackDirection = cardParams.direction;
        if (PositionInBounds(card.PositionOnBoard))
        {
            _board[card.PositionOnBoard.x, card.PositionOnBoard.y] = card;
            card.transform.parent = _slotsTab[card.PositionOnBoard.x, card.PositionOnBoard.y];
            card.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SetSlots(Card card)
    {
        if (PositionInBounds(card.PositionOnBoard))
        {
            _board[card.PositionOnBoard.x, card.PositionOnBoard.y] = card;
            card.transform.parent = _slotsTab[card.PositionOnBoard.x, card.PositionOnBoard.y];
            card.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SetEffect(int index)
    {
        switch (index)
        {
            case(0): _effectGO[0] = Instantiate(_levelDatabase.moveEffectPrefab, _effectSlots[0]); break;
            case(1): _effectGO[1] = Instantiate(_levelDatabase.switchEffectPrefab,_effectSlots[1]); break;
            case(2): _effectGO[2] = Instantiate(_levelDatabase.invocationEffectPrefab, _effectSlots[2]); break;
        }
    }
    
    //Delete comment to activate in SetLevel();
    public void SetKnightDirection()
    {
        foreach (var card in _board)
        {
            if (card.CardType == CardType.KNIGHTSWORD)
            {
                switch (card.AttackDirection)
                {
                    case(Direction.UP):
                        card.GetComponentInChildren<SpriteRenderer>().sprite = _levelDatabase.KS_Up;
                        break;
                    case(Direction.RIGHT):
                        card.GetComponentInChildren<SpriteRenderer>().sprite = _levelDatabase.KS_Right;
                        break;
                    case(Direction.DOWN):
                        card.GetComponentInChildren<SpriteRenderer>().sprite = _levelDatabase.KS_Down;
                        break;
                    case(Direction.LEFT):
                        card.GetComponentInChildren<SpriteRenderer>().sprite = _levelDatabase.KS_Left;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Setup card on board
    /// </summary>
    /// <param name="level">Level to setup</param>
    public void SetLevel(Level level)
    {
        InitSlotTab();
        ResetBoard();
        foreach (var card in level.CardsList)
        {
            SetSlots(card);
        }
        //SetKnightDirection();
        for (int i = 0; i < 3; i++)
        {
            if (level.effects[i])
            {
                SetEffect(i);
            }
        }
    }
    #endregion
    
    #region Test

    /// <summary>
    ///  Check if position is in board
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <returns>Is in board</returns>
    bool PositionInBounds(Vector2Int position)
    {
        return position.x < _board.GetLength(0) &&
               position.x >= 0 &&
               position.y < _board.GetLength(1) &&
               position.y >= 0;
    }

    
    /// <summary>
    /// Get card in slot close
    /// </summary>
    /// <param name="position">Initial position</param>
    /// <param name="direction">Direction</param>
    /// <returns></returns>
    public Card GetCardClose(Vector2Int position, Direction direction)
    {
        InitSlotTab();
        Card card = null;
        switch (direction)
        {
            case (Direction.RIGHT):
                if (PositionInBounds(position + new Vector2Int(0, 1)))
                {
                    card = _board[position.x + 0, position.y + 1];
                }

                break;
            case (Direction.LEFT):
                if (PositionInBounds(position + new Vector2Int(0, -1)))
                {
                    card = _board[position.x, position.y - 1];
                }

                break;
            case (Direction.UP):
                if (PositionInBounds(position + new Vector2Int(-1, 0)))
                {
                    card = _board[position.x - 1, position.y];
                }

                break;
            case (Direction.DOWN):
                if (PositionInBounds(position + new Vector2Int(1, 0)))
                {
                    card = _board[position.x + 1, position.y];
                }

                break;
        }

        return card;
    }

    /// <summary>
    ///  Check if slot is empty
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    public bool SlotEmpty(Vector2Int newPos)
    {
        InitSlotTab();
        return _board[newPos.x, newPos.y] == null;
    }

    /// <summary>
    ///  Get position
    /// </summary>
    /// <param name="position">Initial position</param>
    /// <param name="direction">Direction position you want</param>
    /// <returns></returns>
    public Vector2Int GetPositionNextTo(Vector2Int position, Direction direction)
    {
        InitSlotTab();
        switch (direction)
        {
            case (Direction.RIGHT):
                if (PositionInBounds(position + new Vector2Int(0, 1)))
                {
                    return position + new Vector2Int(0, 1);
                }

                break;
            case (Direction.LEFT):
                if (PositionInBounds(position + new Vector2Int(0, -1)))
                {
                    return position + new Vector2Int(0, -1);
                }

                break;
            case (Direction.UP):
                if (PositionInBounds(position + new Vector2Int(-1, 0)))
                {
                    return position + new Vector2Int(-1, 0);
                }

                break;
            case (Direction.DOWN):
                if (PositionInBounds(position + new Vector2Int(1, 0)))
                {
                    return position + new Vector2Int(1, 0);
                }

                break;
        }

        return position;
    }


    
    #endregion
    
    #region Action
    /// <summary>
    ///  Move card to slots
    /// </summary>
    /// <param name="card"></param>
    /// <param name="newPos"></param>
    public void MoveCard(Card card, Vector2Int newPos)
    {
        InitSlotTab();
        if (PositionInBounds(newPos) && SlotEmpty(newPos))
        {
            _board[card.PositionOnBoard.x, card.PositionOnBoard.y] = null;
            card.PositionOnBoard = newPos;
            SetSlots(card);
        }
    }

    /// <summary>
    /// Switch card position
    /// </summary>
    /// <param name="c1">First card</param>
    /// <param name="c2">Second card</param>
    public void SwitchCard(Card c1, Card c2)
    {
        InitSlotTab();

        if (c1 == null || c2 == null) return;
        Vector2Int temp = c1.PositionOnBoard;
        c1.PositionOnBoard = c2.PositionOnBoard;
        c2.PositionOnBoard = temp;
        _board[c1.PositionOnBoard.x, c1.PositionOnBoard.y] = null;
        _board[c2.PositionOnBoard.x, c2.PositionOnBoard.y] = null;

        SetSlots(c1);
        SetSlots(c2);
    }

    public IEnumerator DoAllEndAction()
    {
        yield return new WaitForSeconds(1);
        foreach (var card in _board)
        {
            if(card != null) card.DoEndOfTurnActions();
        }
        GameStateManager.Instance.CurrentState.ExitState(GameStateManager.Instance);
    }

    public void StartEndAction()
    {
        StartCoroutine(DoAllEndAction());
    }
    
    #endregion
    
    
}