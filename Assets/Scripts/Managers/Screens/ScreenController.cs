using UnityEngine;

public class ScreenController : MonoBehaviour
{
    
    #region Instance

    private static ScreenController _instance;

    public static ScreenController Instance { get => _instance; }

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
    #endregion
    
    [SerializeField] private Transform _parents;
    
    [Header("Main Screens")]
    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private GameObject _levelSelectorScreen;
    [SerializeField] private GameObject _gameScreen;
    
    [Header("Second Screens")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _defeatScreen;
    [SerializeField] private GameObject _PopUpScreen;
    [SerializeField] private GameObject _CreditScreen;
    [SerializeField] private GameObject _optionsScreen;
    
    private MainScreenActive _currentMainScreenActive;
    private SecondScreenActive _currentSecondScreenActive;

    private GameObject GO_currentMainScreenActive;
    private GameObject GO_currentSecondScreenActive;
    
    #region Getter

    public GameObject GO_CurrentMainScreenActive { get => GO_currentMainScreenActive; }
    public GameObject GO_CurrentSecondScreenActive { get => GO_currentSecondScreenActive; }
    
    #endregion
    
    
    public GameObject GetPrefab(MainScreenActive screen)
    {
        switch (screen)
        {
            case(MainScreenActive.Board): return _gameScreen;
            case(MainScreenActive.Start): return _mainScreen;
            case(MainScreenActive.LevelSelect): return _levelSelectorScreen;
        }
        return null;
    }

    public GameObject GetPrefab(SecondScreenActive screen)
    {
        switch (screen)
        {
            case(SecondScreenActive.None): return null;
            case(SecondScreenActive.Pause): return _pauseScreen;
            case(SecondScreenActive.Win): return _winScreen;
            case(SecondScreenActive.Defeat): return _defeatScreen;
            case(SecondScreenActive.PopUp): return _PopUpScreen;
            case(SecondScreenActive.Options): return _optionsScreen;
            case(SecondScreenActive.Credits): return _CreditScreen;
        }
        return null;
    }

    public void LoadScreen(MainScreenActive screen)
    {
        if(GO_currentMainScreenActive != null) Destroy(GO_currentMainScreenActive);
        _currentMainScreenActive = screen;
        
        GO_currentMainScreenActive = Instantiate(GetPrefab(screen), _parents);
        GO_currentMainScreenActive.GetComponent<SetScreen>().OnLoad();
    }

    public void LoadScreen(SecondScreenActive screen)
    {
        if(GO_currentSecondScreenActive != null) Destroy(GO_currentSecondScreenActive);
        _currentSecondScreenActive = screen;

        GO_currentSecondScreenActive = Instantiate(GetPrefab(screen), _parents);
        GO_currentSecondScreenActive.GetComponent<SetScreen>().OnLoad();
    }

    public void UnloadMainScreen() { if(GO_currentMainScreenActive != null) Destroy(GO_currentSecondScreenActive); }
    public void UnloadSecondScreen() { if(GO_currentSecondScreenActive != null) Destroy(GO_currentSecondScreenActive); }
}











