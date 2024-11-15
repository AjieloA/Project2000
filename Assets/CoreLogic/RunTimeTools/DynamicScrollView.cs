//==========================
// - FileName: DynamicScrollView.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 14:36:54
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicScrollView : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement
{
    #region 参数定义
    [Header("配置")]
    public RectTransform ViewPort = null;
    public RectTransform Content = null;
    public Scrollbar Horizontal = null;
    public Scrollbar Vertical = null;
    public GameObject GenerateElement = null;
    public ConditionInfo Conditions = new ConditionInfo();
    public int GenerateCount = 0;
    [DisplayOnly]
    [SerializeField]
    protected int GenerateColCount = 1;
    public int GenerateRowCount = 1;
    public IntPaddingInfo Padding = new IntPaddingInfo(0, 0, 0, 0);
    public Vector2Int Spacing = Vector2Int.zero;
    public float ScrollSensitivity = 1.0f;
    [SerializeField]
    public ScrollFixedInfo[] ScrollFixed = new ScrollFixedInfo[] {
        new ScrollFixedInfo("Horizontal",false,1.0f),
        new ScrollFixedInfo("Vertical",false,1.0f) };
    //---------------------------------------------调试---------------------------------------------
    public int toI = 0;
    public int toJ = 1;
    public NavigateRowType toRowType = NavigateRowType.Front;
    public NavigateColType toColType = NavigateColType.Top;
    public Vector2 toPos = Vector2Int.zero;
    [DisplayOnly]
    protected Vector2 ElementSize = Vector2.zero;
    [DisplayOnly]
    protected Vector2 ViewportSize = Vector2.zero;
    protected Vector2Int ShowCount = Vector2Int.zero;
    protected Vector2 BeginRectPos = Vector2.zero;
    protected Vector2 BeginPos = Vector2.zero;
    protected Vector2 DragPos = Vector2.zero;
    protected Vector2 ClampSize = Vector2.zero;
    protected bool IsOnDrag = false;
    protected bool IsForceUpdate = false;
    protected Queue<PoolItemInfo> ElementPool = new Queue<PoolItemInfo>();
    protected Dictionary<string, UsePoolItemInfo> UseElementPool = new Dictionary<string, UsePoolItemInfo>();
    #endregion
    #region 生命周期
    protected override void Awake()
    {
        if (Horizontal)
            Horizontal.onValueChanged.AddListener(OnScrollBar);
        if (Vertical)
            Vertical.onValueChanged.AddListener(OnScrollBar);
        DoInit();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Navigate2Pos(toI, toJ, toRowType, toColType);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            DoUpdateContentPos(toPos);
            DoRefreshViewport();
        }
    }
    #endregion
    #region UI事件监听
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (!Conditions.IsDrag)
            return;
        BeginRectPos = Content.anchoredPosition;
        BeginPos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Conditions.IsDrag)
            return;
        IsOnDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Conditions.IsDrag)
            return;
        IsOnDrag = false;
        BeginPos = Vector2.zero;
        DragPos = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Conditions.IsDrag)
            return;
        DragPos = eventData.position;
        DoNotifyUpdateScrollBar();
        DoRefreshViewport();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!Conditions.IsScroll)
            return;
        DoUpdateContentPos(Content.anchoredPosition.x, Content.anchoredPosition.y - (eventData.scrollDelta.y * ScrollSensitivity));
        DoNotifyUpdateScrollBar();
        DoRefreshViewport();
    }

    public void Rebuild(CanvasUpdate executing)
    {
    }

    public void LayoutComplete()
    {
    }
    protected override void OnValidate()
    {
        DoValidate();
    }
    public void GraphicUpdateComplete()
    {
    }
    protected void OnScrollBar(float _val)
    {
        if (!Conditions.IsScrollBar)
            return;
        Vector2 pos = Content.anchoredPosition;
        if (Horizontal && Conditions.IsHorzBar)
        {
            pos = new Vector2(-Horizontal.value * (Content.sizeDelta.x - ViewportSize.x), pos.y);
        }
        if (Vertical && Conditions.IsVertBar)
        {
            pos = new Vector2(pos.x, Vertical.value * (Content.sizeDelta.y - ViewportSize.y));
        }
        DoUpdateContentPos(pos);
        DoRefreshViewport();
    }
    #endregion
    #region 列表刷新逻辑
    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void DoInit()
    {
        if (GenerateElement == null || ViewPort == null)
            return;
        GetBaseInfo();
        DoUpdateScrollSize();
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();

    }
    /// <summary>
    /// 生成元素对象池
    /// </summary>
    protected virtual void DoGeneratePool()
    {
        int count = Mathf.CeilToInt((ShowCount.x + 1) / 2) * Mathf.CeilToInt((ShowCount.y + 1) / 2);
        for (int i = 0; i < count; i++)
        {
            GameObject _element = Instantiate(GenerateElement, Content);
            RectTransform _rect = _element.GetComponent<RectTransform>();
            _rect.anchorMin = new Vector2(0, 1);
            _rect.anchorMax = new Vector2(0, 1);
            _rect.pivot = new Vector2(0, 1);
            _element.SetActive(false);
            IDynamicScrollViewRefresh _script = _element.GetComponent<IDynamicScrollViewRefresh>();
            ElementPool.Enqueue(new PoolItemInfo(_element, _script));
        }
    }
    /// <summary>
    /// 刷新显示区域
    /// </summary>
    protected virtual void DoRefreshViewport()
    {
        if (IsOnDrag)
        {
            Vector2 DragDis = BeginRectPos - (BeginPos - DragPos);
            DoUpdateContentPos(DragDis);
        }
        Vector2 pos = Content.anchoredPosition;
        int curX = Mathf.FloorToInt((pos.x + Padding.Left) / -(ElementSize.x + Spacing.x));
        int curY = Mathf.FloorToInt((pos.y - Padding.Top) / (ElementSize.y + Spacing.y));
        curX = Mathf.Clamp(curX, 0, GenerateRowCount);
        curY = Mathf.Clamp(curY, 0, GenerateColCount);
        int endX = Mathf.Clamp(curX + ShowCount.x + 1, 0, GenerateRowCount);
        int endY = Mathf.Clamp(curY + ShowCount.y + 1, 0, GenerateColCount);

        Dictionary<string, bool> tempDic = new Dictionary<string, bool>();
        for (int i = curX; i < endX; i++)
        {
            for (int j = curY; j < endY; j++)
            {
                int index = j * GenerateRowCount + i + 1;
                if (index > GenerateCount)
                    continue;
                string key = $"X_{i}_Y_{j}";
                tempDic.Add(key, true);
                if (UseElementPool.ContainsKey(key))
                    continue;
                if (ElementPool.Count == 0)
                    DoGeneratePool();
                UseElementPool.Add(key, new UsePoolItemInfo(ElementPool.Dequeue(), i, j, index));
                OnEnter(UseElementPool[key].Item.Obj, i, j, index);
            }
        }
        List<string> _keys = new List<string>(UseElementPool.Keys);
        for (int i = 0; i < _keys.Count; i++)
        {
            if (!tempDic.ContainsKey(_keys[i]))
            {
                OnExit(_keys[i]);

            }
        }

    }
    /// <summary>
    /// 更新scroll位置表现，但不触发scroll回调
    /// </summary>
    protected virtual void DoNotifyUpdateScrollBar()
    {
        if (Horizontal)
        {
            Horizontal.SetValueWithoutNotify(Mathf.Clamp(-Content.anchoredPosition.x / (Content.sizeDelta.x - ViewportSize.x), 0f, 1.0f));
        }
        if (Vertical)
        {
            Vertical.SetValueWithoutNotify(Mathf.Clamp(Content.anchoredPosition.y / (Content.sizeDelta.y - ViewportSize.y), 0f, 1.0f));
        }
    }
    /// <summary>
    /// 更新列表滑动条拖拽区域大小
    /// </summary>
    protected virtual void DoUpdateScrollSize()
    {
        if (Horizontal)
        {
            if (ScrollFixed[0].isFixed)
            {
                Horizontal.size = ScrollFixed[0].Size;
            }
            else if (Content)
            {
                Horizontal.size = ViewportSize.x / Content.sizeDelta.x;
            }
            Horizontal.interactable = Conditions.IsHorzBar && Conditions.IsScrollBar;
        }
        if (Vertical)
        {
            if (ScrollFixed[1].isFixed)
            {
                Vertical.size = ScrollFixed[1].Size;
            }
            else if (Content)
            {
                Vertical.size = ViewportSize.y / Content.sizeDelta.y;
            }
            Vertical.interactable = Conditions.IsVertBar && Conditions.IsScrollBar;
        }
    }
    /// <summary>
    /// 更新Content位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void DoUpdateContentPos(float x, float y)
    {
        Vector2 pos = new Vector2(Mathf.Clamp(x, ClampSize.x, 0), Mathf.Clamp(y, 0, ClampSize.y));
        if (!Conditions.IsHorz && !Conditions.IsVert)
        {
            return;
        }
        else if (!Conditions.IsHorz)
        {
            pos = new Vector2(Content.anchoredPosition.x, y);
        }
        else if (!Conditions.IsVert)
        {
            pos = new Vector2(x, Content.anchoredPosition.y);
        }
        Content.anchoredPosition = pos;
    }
    /// <summary>
    /// 更新Content位置
    /// </summary>
    /// <param name="vec"></param>
    protected virtual void DoUpdateContentPos(Vector2 vec)
    {
        DoUpdateContentPos(vec.x, vec.y);
    }
    protected virtual void OnEnter(GameObject obj, int i, int j, int index)
    {
        (obj.transform as RectTransform).anchoredPosition = new Vector2(Padding.Left + (ElementSize.x + Spacing.x) * i, -Padding.Top - (ElementSize.y + Spacing.y) * j);
        obj.SetActive(true);
        TestScrollElement _script = obj.GetOrAddComponent<TestScrollElement>();
        _script.OnEnter(i, j, index);
    }
    protected virtual void OnExit(string key)
    {
        UsePoolItemInfo info = UseElementPool[key];
        info.Item.Script.OnExit(info.I, info.J, info.Index);
        info.Item.Obj.SetActive(false);
        ElementPool.Enqueue(info.Item);
        UseElementPool.Remove(key);
    }
    /// <summary>
    /// 获取一些基础的参数
    /// </summary>
    protected virtual void GetBaseInfo()
    {
        if (GenerateElement)
            ElementSize = (GenerateElement.transform as RectTransform).sizeDelta;
        else
            ElementSize = Vector2.zero;
        if (ViewPort)
            ViewportSize = ViewPort.sizeDelta;
        else
            ViewportSize = Vector2.zero;
        ShowCount.x = Mathf.CeilToInt(ViewportSize.x / (ElementSize.x + Spacing.x));
        ShowCount.y = Mathf.CeilToInt(ViewportSize.y / (ElementSize.y + Spacing.y));
        GenerateColCount = Mathf.CeilToInt(GenerateCount / (GenerateRowCount * 1.0f));
        Content.sizeDelta = new Vector2(Padding.Left + Padding.Right + GenerateRowCount * (ElementSize.x + Spacing.x), Padding.Top + Padding.Bottom + GenerateColCount * (ElementSize.y + Spacing.y));
        ClampSize = new Vector2(-(Content.sizeDelta.x - ViewportSize.x), Content.sizeDelta.y - ViewportSize.y);
    }

    protected virtual void DoValidate()
    {
        GetBaseInfo();
        DoUpdateScrollSize();
    }
    #endregion
    #region 工具接口
    /// <summary>
    /// 更新生成总数
    /// </summary>
    /// <param name="count"></param>
    /// <param name="rowCount"></param>
    /// <param name="isToTop"></param>
    protected virtual void SetGenerateCount(GenerateInfo info)
    {
        GenerateCount = info.GenerateCount;
        if (info.GenerateRowCount != -1)
            GenerateRowCount = info.GenerateRowCount;
        GetBaseInfo();
        if (info.Is2Top)
        {
            DoUpdateContentPos(0, 0);
        }
        else
        {
            DoUpdateContentPos(Content.anchoredPosition);
        }
        DoUpdateScrollSize();
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();
    }

    protected virtual void Navigate2Pos(int i, int j, NavigateRowType rowType, NavigateColType colType)
    {
        i = Mathf.Clamp(i - 1, 0, GenerateRowCount);
        j = Mathf.Clamp(j - 1, 0, GenerateColCount);
        float x = 0;
        float y = 0;
        if (rowType == NavigateRowType.Front)
            x = -Padding.Left - (ElementSize.x + Spacing.x) * i;
        else if (rowType == NavigateRowType.Middle)
            x = (ViewportSize.x - ElementSize.x) / 2 - (ElementSize.x + Spacing.x) * i - Padding.Left;
        else if (rowType == NavigateRowType.Back)
            x = -Padding.Left - (ElementSize.x + Spacing.x) * i + (ViewportSize.x - ElementSize.x);

        if (colType == NavigateColType.Top)
            y = Padding.Top + (ElementSize.y + Spacing.y) * j;
        else if (colType == NavigateColType.Middle)
            y = Padding.Top + (ElementSize.y + Spacing.y) * j - ViewportSize.y / 2 + ElementSize.y / 2;
        else if (colType == NavigateColType.Bottom)
            y = Padding.Top + (ElementSize.y + Spacing.y) * j - ViewportSize.y + ElementSize.y;
        Vector2 pos = new Vector2(x, y);

        DoUpdateContentPos(pos);
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();
    }
    protected virtual void Navigate2Pos(int index)
    {

    }
    #endregion

}
[Serializable]
public struct ScrollFixedInfo
{
    [DisplayOnly]
    public string Name;
    public bool isFixed;
    [Range(0f, 1.0f)]
    public float Size;
    public ScrollFixedInfo(string name, bool isFixed, float size)
    {
        Name = name;
        this.isFixed = isFixed;
        Size = size;

    }
}
[Serializable]
public struct IntPaddingInfo
{
    public int Top;
    public int Bottom;
    public int Left;
    public int Right;
    public IntPaddingInfo(int top, int bottom, int left, int right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }
}
[Serializable]
public struct GenerateInfo
{
    public int GenerateCount;
    public int GenerateRowCount;
    public bool IsForceUpdate;
    public bool Is2Top;
    public GenerateInfo(int count, int rowCount = -1, bool isForce = true, bool isTop = false)
    {
        GenerateCount = count;
        GenerateRowCount = rowCount;
        IsForceUpdate = isForce;
        Is2Top = isTop;
    }
}
[Serializable]
public class ConditionInfo
{
    public bool IsHorz = true;
    public bool IsVert = true;
    public bool IsDrag = true;
    public bool IsScroll = true;
    public bool IsScrollBar = true;
    public bool IsHorzBar = true;
    public bool IsVertBar = true;
}

public struct UsePoolItemInfo
{
    public PoolItemInfo Item;
    public int I;
    public int J;
    public int Index;
    public UsePoolItemInfo(PoolItemInfo item, int i, int j, int index)
    {
        Item = item;
        I = i;
        J = j;
        Index = index;
    }
}
public struct PoolItemInfo
{
    public GameObject Obj;
    public IDynamicScrollViewRefresh Script;
    public PoolItemInfo(GameObject obj, IDynamicScrollViewRefresh script)
    {
        Obj = obj;
        Script = script;
    }
}
public enum NavigateRowType
{
    Front,
    Middle,
    Back,
}
public enum NavigateColType
{
    Top,
    Middle,
    Bottom
}
