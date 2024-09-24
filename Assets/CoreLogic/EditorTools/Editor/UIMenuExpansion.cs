//==========================
// - FileName: UIMenuExpansion.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 15:34:33
// - Email: 1758580256@qq.com
// - Description:
//==========================
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuExpansion : Editor
{
    [MenuItem("GameObject/UI/DynamicScrollView")]
    static void CreateDynamicScrollView()
    {
        bool _isSelectCanvas = false;
        Canvas _canvas = null;
        GameObject _selection = Selection.activeGameObject;
        if (_selection != null)
        {
            _canvas = _selection.transform.GetComponentInParent<Canvas>(true);
            _isSelectCanvas = _canvas != null;
        }
        else
            _canvas = Object.FindObjectOfType<Canvas>();
        if (_canvas == null)
        {
            LogMgr.Instance.CError("Canvas is Null");
            return;
        }
        GameObject _scrollView = new GameObject("DynamicScrollView", typeof(RectTransform));
        _scrollView.transform.SetParent(_isSelectCanvas ? _selection.transform : _canvas.transform);
        RectTransform _scrollViewRect = (_scrollView.transform as RectTransform);
        _scrollViewRect.anchoredPosition = Vector2.zero;
        DynamicScrollView _dynamicScrollView = _scrollView.GetOrAddComponent<DynamicScrollView>();
        Image _imgA = _scrollView.GetOrAddComponent<Image>();
        _imgA.color = new Color(1, 1, 1, 0.5f);

        GameObject _viewport = new GameObject("Viewport", typeof(RectTransform));
        _viewport.transform.SetParent(_scrollView.transform);
        RectTransform _viewportRect = (_viewport.transform as RectTransform);
        _viewportRect.anchorMin = Vector2.zero;
        _viewportRect.anchorMax = new Vector2(1, 1);
        _viewportRect.anchoredPosition = Vector2.zero;
        _viewportRect.sizeDelta = Vector2.zero;
        Image _imgB = _viewport.GetOrAddComponent<Image>();
        _imgB.color = new Color(1, 1, 1, 0);
        _viewport.AddComponent<RectMask2D>();
        _dynamicScrollView.ViewPort = _viewportRect;

        GameObject _content = new GameObject("Content", typeof(RectTransform));
        _content.transform.SetParent(_viewport.transform);
        RectTransform _contentRect = (_content.transform as RectTransform);
        _contentRect.anchorMin = new Vector2(0, 1);
        _contentRect.anchorMax = new Vector2(0, 1);
        _contentRect.pivot = new Vector2(0, 1);
        _contentRect.anchoredPosition = Vector2.zero;
        _contentRect.sizeDelta = new Vector2(0, _scrollViewRect.sizeDelta.y);
        _dynamicScrollView.Content = _content.transform as RectTransform;

        Scrollbar _horizontal = CreateBar(_scrollView.transform, "Horizontal", Vector2.zero, new Vector2(1, 0), Vector2.zero, Vector2.zero, new Vector2(-10, 10));
        Scrollbar _vertical = CreateBar(_scrollView.transform, "Vertical", new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 1), Vector2.zero, new Vector2(10, -10), false);
        _dynamicScrollView.Horizontal = _horizontal;
        _dynamicScrollView.Vertical = _vertical;
    }

    private static Scrollbar CreateBar(Transform _parent, string _name, Vector2 _min, Vector2 _max, Vector2 _pivot, Vector2 _pos, Vector2 _size, bool _isH = true)
    {
        GameObject _bar = new GameObject(_name, typeof(RectTransform));
        _bar.transform.SetParent(_parent);
        RectTransform _barRect = (_bar.transform as RectTransform);
        Scrollbar _scrollBar = _bar.GetOrAddComponent<Scrollbar>();
        if (!_isH)
            _scrollBar.SetDirection(Scrollbar.Direction.TopToBottom, true);
        _barRect.anchorMin = _min;
        _barRect.anchorMax = _max;
        _barRect.pivot = _pivot;
        _barRect.anchoredPosition = _pos;
        _barRect.sizeDelta = _size;
        _bar.AddComponent<Image>();

        GameObject _area = new GameObject("Sliding Area", typeof(RectTransform));
        _area.transform.SetParent(_bar.transform);
        RectTransform _areaRect = (_area.transform as RectTransform);
        _areaRect.anchorMin = Vector2.zero;
        _areaRect.anchorMax = new Vector2(1, 1);
        _areaRect.pivot = new Vector2(0.5f, 0.5f);
        _areaRect.anchoredPosition = Vector2.zero;
        _areaRect.sizeDelta = Vector2.zero;

        GameObject _handle = new GameObject("Handle", typeof(RectTransform));
        _handle.transform.SetParent(_area.transform);
        RectTransform _handleRect = (_handle.transform as RectTransform);
        _handleRect.anchorMin = Vector2.zero;
        _handleRect.anchorMax = new Vector2(1, 1);
        _handleRect.pivot = new Vector2(0.5f, 0.5f);
        _handleRect.anchoredPosition = Vector2.zero;
        _handleRect.sizeDelta = Vector2.zero;
        _scrollBar.handleRect = _handleRect;
        Image _img = _handle.GetOrAddComponent<Image>();
        _img.color = Color.black;
        return _scrollBar;
    }
}
