//==========================
// - FileName: TestScrollElement.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 17:46:34
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEngine;
using UnityEngine.UI;

public class TestScrollElement : MonoBehaviour, IDynamicScrollViewRefresh
{
    public void OnClear()
    {
        LogMgr.Instance.CLog("OnClear");
        gameObject.SetActive(false);
    }

    public void OnEnter(int i, int j)
    {
        gameObject.SetActive(true);
        Text _text = transform.GetChild(0).GetComponent<Text>();
        _text.text = $"{i}|||{j}";
        LogMgr.Instance.CLog("OnEnter");
    }

    public void OnExit(int i, int j)
    {
        gameObject.SetActive(false);
        LogMgr.Instance.CLog("OnExit");
    }

    public void OnRefresh(int i, int j)
    {

    }
}
