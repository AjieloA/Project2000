//==========================
// - FileName: DynamicScrollView.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 14:36:54
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicScrollView : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement
{
    [SerializeField]
    private GameObject mElement = null;
    public GameObject Element { get => mElement; set => mElement = value; }

    [SerializeField]
    private int mElementCount = 0;
    public int ElementCount { get => mElementCount; set => mElementCount = value; }
    [SerializeField]
    private int mPoolCount = 0;
    public int PoolCount { get => mPoolCount; set => mPoolCount = value; }

    [SerializeField]
    private bool mIsHorizontal = true;
    public bool IsHorizontal { get => mIsHorizontal; set => mIsHorizontal = value; }

    [SerializeField]
    private bool mIsVertical = true;
    public bool IsVertical { get => mIsVertical; set => mIsVertical = value; }

    [SerializeField]
    private Vector2 mOffsetCount = Vector2.zero;
    public Vector2 OffsetCount { get => mOffsetCount; set => mOffsetCount = value; }

    private List<ElementInfo> mPool = new List<ElementInfo>();
    public List<ElementInfo> Pool { get => mPool; set => mPool = value; }

    private List<ElementInfo> mUsePool = new List<ElementInfo>();
    public List<ElementInfo> UsePool { get => mUsePool; set => mUsePool = value; }

    [Tooltip("x-top y-bottom z-left w-right")]
    [SerializeField]
    private Vector4 mMargin = Vector4.zero;
    public Vector4 Margin { get => mMargin; set => mMargin = value; }

    [Tooltip("x-left and right y-up and down")]
    [SerializeField]
    private Vector2 mOffset = Vector2.zero;
    public Vector2 Offset { get => mOffset; set => mOffset = value; }

    [SerializeField]
    private bool mIsDrag = false;
    public bool IsDrag { get => mIsDrag; set => mIsDrag = value; }
    [SerializeField]
    private bool mIsScroll = false;
    public bool IsScroll { get => mIsScroll; set => mIsScroll = value; }

    public RectTransform mContent = null;
    public Scrollbar mHorizontal = null;
    public Scrollbar mVertical = null;

    public Vector2 mInitMouseVec = Vector2.zero;
    public Vector2 mCurMouseVec = Vector2.zero;
    public bool mIsOnDrag = false;

    private float mInitH = 0.0f;
    private float mInitV = 0.0f;
    public Vector2 mContentSize = Vector2.zero;
    protected override void Awake()
    {
        mContentSize.y += mMargin.x;
        mContentSize.y += mMargin.y;
        mContentSize.y += (mElementCount * (mElement.transform as RectTransform).sizeDelta.y);
        mContentSize.y += (mElementCount - 1) * mOffset.y;

    }
    protected override void Start()
    {
        if (mElement != null)
            for (int i = 0; i < mPoolCount; i++)
            {
                GameObject _gam = Instantiate(mElement, mContent);
                RectTransform _rect = _gam.GetComponent<RectTransform>();
                _rect.anchoredPosition = Vector2.zero;
                _rect.anchorMin = new Vector2(0, 1);
                _rect.anchorMax = new Vector2(0, 1);
                _rect.pivot = new Vector2(0, 1);
                ElementInfo _elemet = new ElementInfo();
                _elemet.Element = _gam;
                _elemet.Script = _gam.GetComponent<IDynamicScrollViewRefresh>();
                if (i < mElementCount)
                {
                    UsePool.Add(_elemet);
                    _elemet.Element.transform.position = new Vector2(0, (-Margin.x - i * 50));
                    _elemet.Script.OnRefreshByIndex(i);

                }
                else
                    mPool.Add(_elemet);
            }
    }

    public virtual void LateUpdate()
    {

    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        mInitMouseVec = eventData.position;
        mInitH = mContent.anchoredPosition.x;
        mInitV = mContent.anchoredPosition.y;
        LogMgr.Instance.CWarn("OnInitializePotentialDrag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mCurMouseVec = eventData.position;
        mIsOnDrag = true;
        LogMgr.Instance.CWarn("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mIsOnDrag = false;
        LogMgr.Instance.CWarn("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mIsDrag)
            return;
        mCurMouseVec = eventData.position;
        if (!mIsHorizontal && !mIsVertical)
            return;
        Vector2 _vec = new Vector2(0, 0);
        if (mIsHorizontal)
            _vec.x = Mathf.Clamp((mInitH + mCurMouseVec.x - mInitMouseVec.x), 0, mContent.sizeDelta.x);
        if (mIsVertical)
            _vec.y = Mathf.Clamp((mInitV + mCurMouseVec.y - mInitMouseVec.y), 0, (mContentSize.y - (transform as RectTransform).sizeDelta.y));
        mContent.anchoredPosition = _vec;

    }

    public void OnScroll(PointerEventData eventData)
    {
        if (mIsScroll)
            return;
        LogMgr.Instance.CWarn($"OnScroll:{eventData.scrollDelta}");

    }

    public void Rebuild(CanvasUpdate executing)
    {
        LogMgr.Instance.CWarn("Rebuild");
    }

    public void LayoutComplete()
    {
        LogMgr.Instance.CWarn("LayoutComplete");
    }

    public void GraphicUpdateComplete()
    {
        LogMgr.Instance.CWarn("GraphicUpdateComplete");
    }
}
public struct ElementInfo
{
    private GameObject mElement;
    public GameObject Element { get => mElement; set => mElement = value; }

    private IDynamicScrollViewRefresh mScript;
    public IDynamicScrollViewRefresh Script { get => mScript; set => mScript = value; }
}
