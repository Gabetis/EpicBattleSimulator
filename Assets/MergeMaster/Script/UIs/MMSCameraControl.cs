using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSCameraControl : MonoBehaviour
{
    public static MMSCameraControl Instance;
    public Transform posPlay;
    public Transform posDefault;
    public Transform posDragAndDrop;
    public Transform posView3;

    public int viewIndex = 1;
    private List<Transform> listView;
    public List<int> listViewType;

    public void Awake()
    {
        Instance = this;
        listView = new List<Transform>();
        
        listView.Add(posDragAndDrop);
        listView.Add(posDefault);
        listView.Add(posView3);
    }
    public void CameraMoveToPosStart()
    {
        transform.DOMove(posDefault.position, 1f).SetEase(Ease.OutQuad);
        transform.DORotate(posDefault.rotation.eulerAngles, 1f).SetEase(Ease.OutQuad);
    }
    public void CameraMoveToPosPlay()
    {
        transform.DOMove(posPlay.position, 1f).SetEase(Ease.OutQuad);
        transform.DORotate(posPlay.rotation.eulerAngles, 1f).SetEase(Ease.OutQuad);
    }

    public void CameraOnBeginDrag()
    {
        _SwitchView(0);
    }

    public void DefaultViewMode()
    {
        CameraMoveToPosStart();
    }

    public void _SwitchView(int view)
    {
        viewIndex = view%listView.Count;

        Transform _transform = listView[viewIndex];
        transform.DOMove(_transform.position, 0.5f).SetEase(Ease.OutQuad);
        transform.DORotate(_transform.rotation.eulerAngles, 0.5f).SetEase(Ease.OutQuad);
    }

    public void SwithcView()
    {
        _SwitchView(viewIndex + 1);
    }

    public void ShakeCamera()
    {
        transform.DOShakeRotation(0.5f, new Vector3(5f,0.25f,0.25f), 0, 0, true);
    }
}
