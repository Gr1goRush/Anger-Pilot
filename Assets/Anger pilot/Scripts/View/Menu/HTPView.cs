using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HTPView : MonoBehaviour
{
    [SerializeField] private Button _close;
    [SerializeField] private Image _helpTextOne;
    [SerializeField] private Image _helpTextTwo;

    public void SetActive(bool value)
        => gameObject.SetActive(value);

    private void Start()
    {
        _close.onClick.AddListener(OnButtonClickClose);
    }

    private void OnButtonClickClose()
    {
        if (_helpTextOne.gameObject.activeSelf)
            _helpTextOne.gameObject.SetActive(false);
        else
            SetActive(false);
    }

    private void OnEnable()
    {
        _helpTextOne.gameObject.SetActive(true);
    }
}
