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
    public RectTransform viewPort = null;
    public RectTransform content = null;
    public Scrollbar horizontal = null;
    public Scrollbar vertical = null;
    public GameObject generateElement = null;
    public ConditionInfo conditions = new ConditionInfo();
    public int generateCount = 0;
    [DisplayOnly]
    [SerializeField]
    protected int generateColCount = 1;
    public int generateRowCount = 1;
    public IntPaddingInfo padding = new IntPaddingInfo(0, 0, 0, 0);
    public Vector2Int spacing = Vector2Int.zero;
    public float scrollSensitivity = 1.0f;
    [SerializeField]
    public ScrollFixedInfo[] scrollFixed = new ScrollFixedInfo[] {
        new ScrollFixedInfo("Horizontal",false,1.0f),
        new ScrollFixedInfo("Vertical",false,1.0f) };
    //---------------------------------------------调试---------------------------------------------
    public NavigateInfo toInfo = new NavigateInfo();
    public int toIndex = 0;
    [DisplayOnly]
    protected Vector2 elementSize = Vector2.zero;
    [DisplayOnly]
    protected Vector2 viewportSize = Vector2.zero;
    protected Vector2Int showCount = Vector2Int.zero;
    protected Vector2 beginRectPos = Vector2.zero;
    protected Vector2 beginPos = Vector2.zero;
    protected Vector2 dragPos = Vector2.zero;
    protected Vector2 clampSize = Vector2.zero;
    protected bool isOnDrag = false;
    protected bool isForceUpdate = false;
    protected Queue<PoolItemInfo> elementPool = new Queue<PoolItemInfo>();
    protected Dictionary<string, UsePoolItemInfo> useElementPool = new Dictionary<string, UsePoolItemInfo>();
    #endregion
    #region 生命周期
    protected override void Awake()
    {
        if (horizontal)
            horizontal.onValueChanged.AddListener(OnScrollBar);
        if (vertical)
            vertical.onValueChanged.AddListener(OnScrollBar);
        DoInit();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Navigate2Pos(toInfo);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Navigate2Pos(toIndex, toInfo);
        }
    }
    #endregion
    #region UI事件监听
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (!conditions.isDrag)
            return;
        beginRectPos = content.anchoredPosition;
        beginPos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!conditions.isDrag)
            return;
        isOnDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!conditions.isDrag)
            return;
        isOnDrag = false;
        beginPos = Vector2.zero;
        dragPos = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!conditions.isDrag)
            return;
        dragPos = eventData.position;
        DoNotifyUpdateScrollBar();
        DoRefreshViewport();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!conditions.isScroll)
            return;
        DoUpdateContentPos(content.anchoredPosition.x, content.anchoredPosition.y - (eventData.scrollDelta.y * scrollSensitivity));
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
        if (!conditions.isScrollBar)
            return;
        Vector2 pos = content.anchoredPosition;
        if (horizontal && conditions.isHorzBar)
        {
            pos = new Vector2(-horizontal.value * (content.sizeDelta.x - viewportSize.x), pos.y);
        }
        if (vertical && conditions.isVertBar)
        {
            pos = new Vector2(pos.x, vertical.value * (content.sizeDelta.y - viewportSize.y));
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
        if (generateElement == null || viewPort == null)
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
        int count = Mathf.CeilToInt((showCount.x + 1) / 2) * Mathf.CeilToInt((showCount.y + 1) / 2);
        for (int i = 0; i < count; i++)
        {
            GameObject _element = Instantiate(generateElement, content);
            RectTransform _rect = _element.GetComponent<RectTransform>();
            _rect.anchorMin = new Vector2(0, 1);
            _rect.anchorMax = new Vector2(0, 1);
            _rect.pivot = new Vector2(0, 1);
            _element.SetActive(false);
            IDynamicScrollViewRefresh _script = _element.GetComponent<IDynamicScrollViewRefresh>();
            elementPool.Enqueue(new PoolItemInfo(_element, _script));
        }
    }
    /// <summary>
    /// 刷新显示区域
    /// </summary>
    protected virtual void DoRefreshViewport()
    {
        if (isOnDrag)
        {
            Vector2 DragDis = beginRectPos - (beginPos - dragPos);
            DoUpdateContentPos(DragDis);
        }
        Vector2 pos = content.anchoredPosition;
        int curX = Mathf.FloorToInt((pos.x + padding.left) / -(elementSize.x + spacing.x));
        int curY = Mathf.FloorToInt((pos.y - padding.top) / (elementSize.y + spacing.y));
        curX = Mathf.Clamp(curX, 0, generateRowCount);
        curY = Mathf.Clamp(curY, 0, generateColCount);
        int endX = Mathf.Clamp(curX + showCount.x + 1, 0, generateRowCount);
        int endY = Mathf.Clamp(curY + showCount.y + 1, 0, generateColCount);

        Dictionary<string, bool> tempDic = new Dictionary<string, bool>();
        for (int i = curX; i < endX; i++)
        {
            for (int j = curY; j < endY; j++)
            {
                int index = j * generateRowCount + i + 1;
                if (index > generateCount)
                    continue;
                string key = $"X_{i}_Y_{j}";
                tempDic.Add(key, true);
                if (useElementPool.ContainsKey(key))
                    continue;
                if (elementPool.Count == 0)
                    DoGeneratePool();
                useElementPool.Add(key, new UsePoolItemInfo(elementPool.Dequeue(), i, j, index));
                OnEnter(useElementPool[key].item.obj, i, j, index);
            }
        }
        List<string> _keys = new List<string>(useElementPool.Keys);
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
        if (horizontal)
        {
            horizontal.SetValueWithoutNotify(Mathf.Clamp(-content.anchoredPosition.x / (content.sizeDelta.x - viewportSize.x), 0f, 1.0f));
        }
        if (vertical)
        {
            vertical.SetValueWithoutNotify(Mathf.Clamp(content.anchoredPosition.y / (content.sizeDelta.y - viewportSize.y), 0f, 1.0f));
        }
    }
    /// <summary>
    /// 更新列表滑动条拖拽区域大小
    /// </summary>
    protected virtual void DoUpdateScrollSize()
    {
        if (horizontal)
        {
            if (scrollFixed[0].isFixed)
            {
                horizontal.size = scrollFixed[0].size;
            }
            else if (content)
            {
                horizontal.size = viewportSize.x / content.sizeDelta.x;
            }
            horizontal.interactable = conditions.isHorzBar && conditions.isScrollBar;
        }
        if (vertical)
        {
            if (scrollFixed[1].isFixed)
            {
                vertical.size = scrollFixed[1].size;
            }
            else if (content)
            {
                vertical.size = viewportSize.y / content.sizeDelta.y;
            }
            vertical.interactable = conditions.isVertBar && conditions.isScrollBar;
        }
    }
    /// <summary>
    /// 更新Content位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void DoUpdateContentPos(float x, float y)
    {
        Vector2 pos = new Vector2(Mathf.Clamp(x, clampSize.x, 0), Mathf.Clamp(y, 0, clampSize.y));
        if (!conditions.isHorz && !conditions.isVert)
        {
            return;
        }
        else if (!conditions.isHorz)
        {
            pos = new Vector2(content.anchoredPosition.x, y);
        }
        else if (!conditions.isVert)
        {
            pos = new Vector2(x, content.anchoredPosition.y);
        }
        content.anchoredPosition = pos;
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
        (obj.transform as RectTransform).anchoredPosition = new Vector2(padding.left + (elementSize.x + spacing.x) * i, -padding.top - (elementSize.y + spacing.y) * j);
        obj.SetActive(true);
        TestScrollElement _script = obj.GetOrAddComponent<TestScrollElement>();
        _script.OnEnter(i, j, index);
    }
    protected virtual void OnExit(string key)
    {
        UsePoolItemInfo info = useElementPool[key];
        info.item.script.OnExit(info.i, info.j, info.index);
        info.item.obj.SetActive(false);
        elementPool.Enqueue(info.item);
        useElementPool.Remove(key);
    }
    /// <summary>
    /// 获取一些基础的参数
    /// </summary>
    protected virtual void GetBaseInfo()
    {
        if (generateElement)
            elementSize = (generateElement.transform as RectTransform).sizeDelta;
        else
            elementSize = Vector2.zero;
        if (viewPort)
            viewportSize = viewPort.sizeDelta;
        else
            viewportSize = Vector2.zero;
        showCount.x = Mathf.CeilToInt(viewportSize.x / (elementSize.x + spacing.x));
        showCount.y = Mathf.CeilToInt(viewportSize.y / (elementSize.y + spacing.y));
        generateColCount = Mathf.CeilToInt(generateCount / (generateRowCount * 1.0f));
        if (content == null)
            return;
        content.sizeDelta = new Vector2(padding.left + padding.right + generateRowCount * (elementSize.x + spacing.x), padding.top + padding.bottom + generateColCount * (elementSize.y + spacing.y));
        clampSize = new Vector2(-(content.sizeDelta.x - viewportSize.x), content.sizeDelta.y - viewportSize.y);
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
        generateCount = info.generateCount;
        if (info.generateRowCount != -1)
            generateRowCount = info.generateRowCount;
        GetBaseInfo();
        if (info.is2Top)
        {
            DoUpdateContentPos(0, 0);
        }
        else
        {
            DoUpdateContentPos(content.anchoredPosition);
        }
        DoUpdateScrollSize();
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();
    }
    /// <summary>
    /// 定位移动到指定行列位置
    /// </summary>
    /// <param name="info"></param>

    protected virtual void Navigate2Pos(NavigateInfo info)
    {
        int i = Mathf.Clamp(info.x - 1, 0, generateRowCount);
        int j = Mathf.Clamp(info.y - 1, 0, generateColCount);
        float x = 0;
        float y = 0;
        if (info.rowType == NavigateRowType.Front)
            x = -padding.left - (elementSize.x + spacing.x) * i;
        else if (info.rowType == NavigateRowType.Middle)
            x = (viewportSize.x - elementSize.x) / 2 - (elementSize.x + spacing.x) * i - padding.left;
        else if (info.rowType == NavigateRowType.Back)
            x = -padding.left - (elementSize.x + spacing.x) * i + (viewportSize.x - elementSize.x);

        if (info.colType == NavigateColType.Top)
            y = padding.top + (elementSize.y + spacing.y) * j;
        else if (info.colType == NavigateColType.Middle)
            y = padding.top + (elementSize.y + spacing.y) * j - viewportSize.y / 2 + elementSize.y / 2;
        else if (info.colType == NavigateColType.Bottom)
            y = padding.top + (elementSize.y + spacing.y) * j - viewportSize.y + elementSize.y;
        Vector2 pos = new Vector2(x, y);

        DoUpdateContentPos(pos);
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();
    }
    /// <summary>
    /// 定位移动到指定下标位置
    /// </summary>
    /// <param name="info"></param>
    protected virtual void Navigate2Pos(int index, NavigateInfo info)
    {
        int _x = Mathf.FloorToInt((index * 1.0f) / generateRowCount);
        int _y = index - (_x * generateRowCount);
        _x++;
        _y++;
        NavigateInfo _info = new NavigateInfo(_x, _y, info.rowType, info.colType);
        Navigate2Pos(_info);
    }
    /// <summary>
    /// 改变间距
    /// </summary>
    /// <param name="info"></param>
    protected virtual void SetPadding(IntPaddingInfo info)
    {
        padding = info;
        DoRefreshViewport();
        DoNotifyUpdateScrollBar();
    }
    #endregion

}
[Serializable]
public struct ScrollFixedInfo
{
    [DisplayOnly]
    public string name;
    public bool isFixed;
    [Range(0f, 1.0f)]
    public float size;
    public ScrollFixedInfo(string _name, bool _isFixed, float _size)
    {
        name = _name;
        isFixed = _isFixed;
        size = _size;

    }
}
[Serializable]
public struct IntPaddingInfo
{
    public int top;
    public int bottom;
    public int left;
    public int right;
    public IntPaddingInfo(int _top, int _bottom, int _left, int _right)
    {
        top = _top;
        bottom = _bottom;
        left = _left;
        right = _right;
    }
}
[Serializable]
public struct GenerateInfo
{
    public int generateCount;
    public int generateRowCount;
    public bool isForceUpdate;
    public bool is2Top;
    public GenerateInfo(int _count, int _rowCount = -1, bool _isForce = true, bool _isTop = false)
    {
        generateCount = _count;
        generateRowCount = _rowCount;
        isForceUpdate = _isForce;
        is2Top = _isTop;
    }
}
[Serializable]
public class ConditionInfo
{
    public bool isHorz = true;
    public bool isVert = true;
    public bool isDrag = true;
    public bool isScroll = true;
    public bool isScrollBar = true;
    public bool isHorzBar = true;
    public bool isVertBar = true;
}

public struct UsePoolItemInfo
{
    public PoolItemInfo item;
    public int i;
    public int j;
    public int index;
    public UsePoolItemInfo(PoolItemInfo _item, int _i, int _j, int _index)
    {
        item = _item;
        i = _i;
        j = _j;
        index = _index;
    }
}
public struct PoolItemInfo
{
    public GameObject obj;
    public IDynamicScrollViewRefresh script;
    public PoolItemInfo(GameObject _obj, IDynamicScrollViewRefresh _script)
    {
        this.obj = _obj;
        script = _script;
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
[Serializable]
public struct NavigateInfo
{
    public int x;
    public int y;
    public NavigateRowType rowType;
    public NavigateColType colType;
    public NavigateInfo(int _x, int _y, NavigateRowType _rowType, NavigateColType _colType)
    {
        x = _x;
        y = _y;
        rowType = _rowType;
        colType = _colType;
    }
}
