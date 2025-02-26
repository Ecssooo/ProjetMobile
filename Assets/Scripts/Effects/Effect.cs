using Unity.VisualScripting;
using UnityEngine;

public class Effect : MonoBehaviour
{


    [Header("Effect Selection")]
    [SerializeField] private Effects _effect;
    public Effects Effet { get => _effect; }

    [Header("Effect Settings")]
    [SerializeField] private float _effectDuration = 1f;

    private void OnMouseDown()
    {
        GameManager.Instance.Effect = this.Effet;
        //if (GameManager.Instance.Effect != Effects.NONE) GameManager.Instance.Effect = Effects.NONE;
        switch (GameManager.Instance.Effect)
        {
            case Effects.MOVE:
                if (!EffectList.MoveCard)
                {
                    EffectList.MoveCard = true;
                    EffectList.SwapCard = false;
                    EffectList.InvokeCard = false;
                    GameManager.Instance.Effect = Effects.MOVE;
                    return;
                }
                EffectList.MoveCard = false;
                EffectList.SwapCard = false;
                EffectList.InvokeCard = false;
                GameManager.Instance.Effect = Effects.NONE;
                break;
            case Effects.SWAP:
                if (!EffectList.SwapCard)
                {
                    EffectActions.Instance._swapFirstCard = null;
                    EffectActions.Instance._swapSecondCard = null;
                    EffectList.SwapCard = true;
                    EffectList.MoveCard = false;
                    EffectList.InvokeCard = false;
                    GameManager.Instance.Effect = Effects.SWAP;
                    return;
                }
                EffectList.SwapCard = false;
                EffectList.MoveCard = false;
                EffectList.InvokeCard = false;
                GameManager.Instance.Effect = Effects.NONE;
                break;
            case Effects.NONE:
                break;
        }
    }

    #region Drag and Drop

    //private void OnMouseDrag()
    //{
    //    _isDragging = true;
    //    //transform.position = Vector3.Lerp(transform.position, GetMousePos(), .25f);
    //    _lastMousePos = GetMousePos();
    //    _hasDragged = true;
    //}
    //private Vector2 GetMousePos()
    //{
    //    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //}

    //protected virtual void OnMouseUp()
    //{
    //    _isDragging = false;
    //}

    #endregion
}
