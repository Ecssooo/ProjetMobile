using System.Collections;
using UnityEngine;

public class TutoLevel1 : TutoLevelBase
{
    private int action = -1;

    [SerializeField] private GameObject scene1;
    [SerializeField] private GameObject scene2;
    [SerializeField] private GameObject scene3;
    [SerializeField] private GameObject scene4;
    [SerializeField] private GameObject scene5;
    [SerializeField] private GameObject scene6;
    
    private Coroutine _coroutine;
    
    public void Update()
    {
        if(_coroutine == null) _coroutine = StartCoroutine(DoTuto());
    }
    
    public override IEnumerator DoTuto()
    {
        switch (action)
        {
            case(-1):
                scene1.SetActive(true);
                action = 0;
                break;
            case(0):
                if (GameManager.Instance.Effect == Effects.MOVE)
                {
                    scene1.SetActive(false);
                    scene2.SetActive(true);
                    action = 1;
                }
                break;
            case(1):
                if (ListAction.Instance.ListActions.Count <= 0) break;
                if(ListAction.Instance.ListActions[0]._card.CardType == CardType.KNIGHTSWORD)
                {
                    scene2.SetActive(false);
                    scene3.SetActive(true);
                    action = 2;
                    yield return new WaitForSeconds(0.01f);
                    if(GameManager.Instance.ButtonReady != null)
                    {
                        GameManager.Instance.ButtonReady.interactable = false;
                        Color color = GameManager.Instance.ButtonReady.GetComponent<UnityEngine.UI.Image>().color;
                        color.a = 0.5f;
                        GameManager.Instance.ButtonReady.GetComponent<UnityEngine.UI.Image>().color = color;
                    }
                }
                break;
            case(2):
                if (ScreenController.Instance.CurrentSecondScreenActive == SecondScreenActive.None)
                {
                    scene3.SetActive(false);
                    scene4.SetActive(true);
                    GameManager.Instance.ButtonReady.interactable = true;
                    Color color = GameManager.Instance.ButtonReady.GetComponent<UnityEngine.UI.Image>().color;
                    color.a = 1f;
                    GameManager.Instance.ButtonReady.GetComponent<UnityEngine.UI.Image>().color = color;
                    GameManager.Instance.Effect = Effects.NONE;
                    action = 3;
                }
                break;
            case(3):
                if (ListAction.Instance.ListActions.Count <= 0)
                {
                    scene4.SetActive(false);
                    scene5.SetActive(true);
                    action = 4;
                }
                break;
            case(4):
                if (GameManager.Instance.Effect == Effects.MOVE)
                {
                    scene5.SetActive(false);
                    scene6.SetActive(true);
                    action = 5;
                }
                break;
            case(5):
                if (ListAction.Instance.ListActions.Count <= 0) break;
                if (ListAction.Instance.ListActions[0]._card.CardType == CardType.KNIGHTSWORD)
                {
                    scene6.SetActive(false);
                    action = 7;
                }
                break;
            case(7):
                Destroy(this.gameObject);
                break;
        }
        _coroutine = null;
    }
}
