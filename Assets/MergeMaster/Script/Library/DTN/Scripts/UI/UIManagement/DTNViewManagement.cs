using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTNViewManagement : MonoBehaviour
{
    private static DTNViewManagement _instance;
    [SerializeField] private DTNView _startingView;
    [SerializeField] private DTNView[] _views;

    private readonly Stack<DTNView> _history = new Stack<DTNView>();
    public DTNView _currentView;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // DontDestroyOnLoad(gameObject);
        _instance = this;
    }

    public static T GetView<T>() where T : DTNView
    {

        for (int i = 0; i < _instance._views.Length; i++)
        {
            if (_instance._views[i] is T tView)
            {
                return tView;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = false) where T : DTNView
    {
        //hot fix
        remember = false;
        for (int i = 0; i < _instance._views.Length; i++)
        {
            if (_instance._views[i] is T)
            {
                if (_instance._currentView != null)
                {
                    if (remember)
                    {
                        _instance._history.Push(_instance._currentView);
                    }

                    _instance._currentView.InitIfNeed();
                    _instance._currentView.WillHide();
                    _instance._currentView.Hide();
                }

                _instance._views[i].InitIfNeed();
                _instance._views[i].WillShow();
                _instance._views[i].Show();
                _instance._currentView = _instance._views[i];
            }
        }
    }

    public static void Show(DTNView view, bool remember = false)
    {
        if (_instance._currentView != null)
        {
            if (remember)
            {
                _instance._history.Push(_instance._currentView);
            }

            _instance._currentView.InitIfNeed();
            _instance._currentView.WillHide();
            _instance._currentView.Hide();

        }
        // Initialize
        view.InitIfNeed();
        view.WillShow();
        view.Show();
        _instance._currentView = view;
    }

    public static void ShowLast()
    {
        if (_instance._history.Count != 0)
        {
            Show(_instance._history.Pop(), false);
        }
    }

    private void Start()
    {
        Show(_startingView, false);
    }
}
