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
        gameObject.SetActive(false);
    }

    public void OnRefreshByIndex(int _index)
    {
        gameObject.SetActive(true);
        Text _text = transform.GetChild(0).GetComponent<Text>();
        _text.text = _index.ToString();
    }
}
