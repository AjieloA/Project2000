//==========================
// - FileName: DynamicScrollView.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 14:36:54
// - Email: 1758580256@qq.com
// - Description:
//==========================
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicScrollView : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement
{
    [Header("配置")]

    [SerializeField]
    private RectTransform mViewPort = null;
    public RectTransform ViewPort { get => mViewPort; set => mViewPort = value; }
    [SerializeField]
    private RectTransform mContent = null;
    public RectTransform Content { get => mContent; set => mContent = value; }
    [SerializeField]
    private Scrollbar mHorizontal = null;
    public Scrollbar Horizontal { get => mHorizontal; set => mHorizontal = value; }
    [SerializeField]
    private Scrollbar mVertical = null;
    public Scrollbar Vertical { get => mVertical; set => mVertical = value; }
    [Tooltip("生成元素预制体")]
    [SerializeField]
    private GameObject mElement = null;
    public GameObject Element { get => mElement; set => mElement = value; }
    [SerializeField]
    private bool mIsHorizontal = true;
    public bool IsHorizontal { get => mIsHorizontal; set => mIsHorizontal = value; }
    [SerializeField]
    private bool mIsVertical = true;
    public bool IsVertical { get => mIsVertical; set => mIsVertical = value; }

    [SerializeField]
    private int mHorizontalCount = 1;
    public int HorizontalCount { get => mHorizontalCount; set => mHorizontalCount = value; }

    [SerializeField]
    private Vector4 mPadding = Vector4.zero;
    public Vector4 Padding { get => mPadding; set => mPadding = value; }
    [SerializeField]
    private Vector2 mSpacing = Vector2.zero;
    public Vector2 Spacing { get => mSpacing; set => mSpacing = value; }

    [Header("运行时状态")]

    [DisplayOnly]
    [SerializeField]
    private int mShowCountX = 0;
    public int ShowCountX { get => mShowCountX; set => mShowCountX = value; }
    [DisplayOnly]
    [SerializeField]
    private int mShowCountY = 0;
    public int ShowCountY { get => mShowCountY; set => mShowCountY = value; }
    [ReName("元素宽高")]
    [DisplayOnly]
    [SerializeField]
    private Vector2 mElementSize = Vector2.zero;
    [ReName("需要生成的总数量")]
    //[DisplayOnly]
    [SerializeField]
    private int mAllElementCount = 10;
    public int AllElementCount { get => mAllElementCount; set => mAllElementCount = value; }
    public Vector2 ElementSize { get => mElementSize; set => mElementSize = value; }
    [DisplayOnly]
    [SerializeField]
    public int mHeadX = 0;
    public int HeadX { get => mHeadX; set => mHeadX = value; }
    [DisplayOnly]
    [SerializeField]
    public int mHeadY = 0;
    public int HeadY { get => mHeadY; set => mHeadY = value; }
    [DisplayOnly]
    [SerializeField]
    private Vector2 mStartDargPos = Vector2.zero;
    public Vector2 StartDargPos { get => mStartDargPos; set => mStartDargPos = value; }
    [DisplayOnly]
    [SerializeField]
    private Vector2 mCurDargPos = Vector2.zero;
    public Vector2 CurDargPos
    {
        get => mCurDargPos;
        set
        {
            mCurDargPos = value;
            DragDis = StartDargPos - mCurDargPos;
        }
    }
    [DisplayOnly]
    [SerializeField]
    private Vector2 mDargDis = Vector2.zero;
    public Vector2 DragDis
    {
        get => mDargDis;
        set
        {
            mDargDis = value;
            if (!IsDrag)
                return;
            mX = mDragStartPos.x;
            mY = mDragStartPos.y;
            if (IsHorizontal)
            {
                mX -= mDargDis.x;
            }
            if (IsVertical)
            {
                mY -= mDargDis.y;
            }
            mX = Mathf.Clamp(mX, mClampSize.x, 0.0f);
            mY = Mathf.Clamp(mY, 0.0f, mClampSize.y);
            if (Vertical)
                Vertical.SetValueWithoutNotify(Mathf.Clamp(mY / mClampSize.y, 0.0f, 1.0f));
            if (Horizontal)
                Horizontal.SetValueWithoutNotify(Mathf.Clamp(mX / mClampSize.x, 0.0f, 1.0f));
            OnDragRefresh(mX, mY);
        }
    }
    [DisplayOnly]
    [SerializeField]
    private bool mIsDrag = false;
    public bool IsDrag { get => mIsDrag; set => mIsDrag = value; }

    private Vector2 mDragStartPos = Vector2.zero;
    [DisplayOnly]
    [SerializeField]
    private float mX = 0.0f;
    [DisplayOnly]
    [SerializeField]
    private float mY = 0.0f;
    private Vector2 mClampSize = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        if (Vertical)
            Vertical.onValueChanged.AddListener((_val) =>
            {
                mX = Content.anchoredPosition.x;
                mY = Content.anchoredPosition.y;
                mY = Mathf.Clamp(_val * mClampSize.y, 0.0f, mClampSize.y);
                OnDragRefresh(mX, mY);
            });
        if (Horizontal)
            Horizontal.onValueChanged.AddListener((_val) =>
            {
                mX = Mathf.Clamp(_val * mClampSize.x, mClampSize.x, 0.0f);
                mY = Content.anchoredPosition.y;
                OnDragRefresh(mX, mY);

            });
        OnInit();

    }
    [DisplayOnly]
    public SerializableQueue<GameObject> mPool = new SerializableQueue<GameObject>();
    [DisplayOnly]
    public SerializableDictionary<int, SerializableDictionary<int, GameObject>> mUsePool = new SerializableDictionary<int, SerializableDictionary<int, GameObject>>();
    protected virtual void OnInit()
    {
        if (Element == null)
        {
            LogMgr.Instance.CWarn("Element Is Null");
            return;
        }
        RectTransform _rect = GetComponent<RectTransform>();
        ElementSize = (Element.transform as RectTransform).sizeDelta;
        ShowCountX = Mathf.CeilToInt(_rect.sizeDelta.x / ElementSize.x) + 1;
        ShowCountX = Mathf.Clamp(ShowCountX, 1, HorizontalCount);
        ShowCountY = Mathf.CeilToInt(_rect.sizeDelta.y / ElementSize.y) + 1;
        Content.sizeDelta = new Vector2((ElementSize.x + Spacing.x) * HorizontalCount + Padding.x + Padding.y, (ElementSize.y + Spacing.y) * Mathf.CeilToInt(AllElementCount / HorizontalCount) + Padding.z + Padding.w);
        mClampSize = new Vector2(_rect.sizeDelta.x - Content.sizeDelta.x, Content.sizeDelta.y - _rect.sizeDelta.y);
        Horizontal.size = Mathf.Clamp(_rect.sizeDelta.x / Content.sizeDelta.x, 0.05f, 1.0f);
        Vertical.size = Mathf.Clamp(_rect.sizeDelta.y / Content.sizeDelta.y, 0.05f, 1.0f);
        OnCreatePool();
        if (Vertical)
            Vertical.SetValueWithoutNotify(0);
        if (Horizontal)
            Horizontal.SetValueWithoutNotify(0);
        for (int i = 0; i < ShowCountY; i++)
        {

            for (int j = 0; j < ShowCountX; j++)
            {
                OnEnter(i, j);
            }
        }
        mCurPos = new Vector2(0, 0);
        Content.anchoredPosition = mCurPos;
    }
    protected virtual void OnCreatePool()
    {
        int _count = ShowCountX * ShowCountY;
        for (int i = 0; i < _count; i++)
        {
            GameObject _element = Instantiate(Element, Content);
            _element.SetActive(false);
            mPool.Enqueue(_element);
        }
    }
    [DisplayOnly]
    [SerializeField]
    private Vector2 mLastPos = Vector2.zero;
    [DisplayOnly]
    [SerializeField]
    private Vector2 mCurPos = Vector2.zero;
    private bool mActive = false;
    protected virtual void OnDragRefresh(float _x, float _y)
    {
        if (mActive)
            return;
        if (!Content)
        {
            LogMgr.Instance.CLog("Content Is Null");
            return;
        }
        mCurPos = new Vector2(_x, _y);
        Content.anchoredPosition = mCurPos;
        int _yClamp = Mathf.CeilToInt(AllElementCount / HorizontalCount);
        mX = Mathf.FloorToInt(Mathf.Abs(_x) / (ElementSize.x + Spacing.x));
        mX = Mathf.Clamp(mX, 0, HorizontalCount);
        mY = Mathf.FloorToInt(Mathf.Abs(_y) / (ElementSize.y + Spacing.y));
        mY = Mathf.Clamp(mY, 0, _yClamp);

        if (mX == HeadX && mY == HeadY)
            return;
        mActive = true;
        int _valX = (int)mX - HeadX;
        int _valY = (int)mY - HeadY;

        int _reStartY = HeadY;
        int _reEndY = HeadY + ShowCountY;
        int _addStartY = (int)mY;
        int _addEndY = (int)mY + ShowCountY;

        int _reStratX = HeadX;
        int _reEndX = HeadX + ShowCountX;
        int _addStratX = (int)mX;
        int _addEndX = (int)mX + ShowCountX;

        if (_valY > 0)
        {
            _reStartY = HeadY;
            _reEndY = (int)mY;
            _addStartY = HeadY + ShowCountY;
            _addEndY = (int)mY + ShowCountY;
        }
        else if (_valY < 0)
        {
            _reStartY = (int)mY + ShowCountY;
            _reEndY = HeadY + ShowCountY;
            _addStartY = (int)mY;
            _addEndY = HeadY;
        }

        if (_valX > 0)
        {
            _reStratX = HeadX;
            _reEndX = (int)mX;
            _addStratX = HeadX + ShowCountX;
            _addEndX = (int)mX + ShowCountX;
        }
        else if (_valX < 0)
        {
            _reStratX = (int)mX + ShowCountX;
            _reEndX = HeadX + ShowCountX;
            _addStratX = (int)mX;
            _addEndX = HeadX;
        }

        _reStartY = Mathf.Clamp(_reStartY, 0, _yClamp);
        _reEndY = Mathf.Clamp(_reEndY, 0, _yClamp);
        _addStartY = Mathf.Clamp(_addStartY, 0, _yClamp);
        _addEndY = Mathf.Clamp(_addEndY, 0, _yClamp);

        _reStratX = Mathf.Clamp(_reStratX, 0, HorizontalCount);
        _reEndX = Mathf.Clamp(_reEndX, 0, HorizontalCount);
        _addStratX = Mathf.Clamp(_addStratX, 0, HorizontalCount);
        _addEndX = Mathf.Clamp(_addEndX, 0, HorizontalCount);



        //移除
        for (int i = _reStartY; i < _reEndY; i++)
        {
            if (!mUsePool.ContainsKey(i))
                continue;
            for (int j = _reStratX; j < _reEndX; j++)
            {
                OnExit(i, j);
            }
        }
        //新增
        for (int i = _addStartY; i < _addEndY; i++)
        {
            for (int j = _addStratX; j < _addEndX; j++)
            {
                OnEnter(i, j);
            }
        }
        HeadX = (int)mX;
        HeadY = (int)mY;
        mActive = false;
    }
    protected virtual void OnExit(int i, int j)
    {
        if (mUsePool[i].TryGetValue(j, out GameObject _element))
        {
            _element = mUsePool[i][j];
            TestScrollElement _script = _element.GetOrAddComponent<TestScrollElement>();
            _script.OnExit(i, j);
            mPool.Enqueue(_element);
            mUsePool[i].Remove(j);
        }
    }
    protected virtual void OnEnter(int i, int j)
    {
        if (!mUsePool.ContainsKey(i))
            mUsePool.Add(i, new SerializableDictionary<int, GameObject>());
        if (mUsePool[i].TryGetValue(j, out GameObject _obj))
        {
            return;
        }
        GameObject _element = mPool.Dequeue();
        if (_element == null)
            return;
        TestScrollElement _script = _element.GetOrAddComponent<TestScrollElement>();
        RectTransform _rectT = _element.transform as RectTransform;
        _rectT.anchoredPosition = new Vector2(j * (ElementSize.x + Spacing.x), -i * (ElementSize.y + Spacing.y));
        _script.OnEnter(i, j);
        mUsePool[i].Add(j, _element);
    }

    public void GraphicUpdateComplete()
    {
        LogMgr.Instance.CLog("GraphicUpdateComplete");
    }

    public void LayoutComplete()
    {
        LogMgr.Instance.CLog("LayoutComplete");
    }





    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        LogMgr.Instance.CLog("OnInitializePotentialDrag");
        IsDrag = true;
        StartDargPos = eventData.position;
        mDragStartPos = Content.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        LogMgr.Instance.CLog("OnBeginDrag");
        CurDargPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        LogMgr.Instance.CLog("OnDrag");
        CurDargPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LogMgr.Instance.CLog("OnEndDrag");
        IsDrag = false;
        StartDargPos = Vector2.zero;
        CurDargPos = Vector2.zero;
    }

    public void OnScroll(PointerEventData eventData)
    {
        LogMgr.Instance.CLog("OnScroll");
    }






    public void Rebuild(CanvasUpdate executing)
    {
        LogMgr.Instance.CLog("Rebuild");
    }
}
