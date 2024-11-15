//==========================
// - FileName: IDynamicScrollViewRefresh.cs
// - Created: AjieloA
// - CreateTime: 2024-06-25 14:29:17
// - Email: 1758580256@qq.com
// - Description:
//==========================
public interface IDynamicScrollViewRefresh
{
    public void OnEnter(int i, int j, int index);
    public void OnRefresh(int i, int j, int index);
    public void OnExit(int i, int j, int index);
    public void OnClear();

}
