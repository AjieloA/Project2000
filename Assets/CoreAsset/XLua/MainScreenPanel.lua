print('MainScreenPanel')
MainScreenPanel = {}
MainScreenPanel.mView = nil
MainScreenPanel.btn_Pink = nil
MainScreenPanel.btn_Blue = nil
MainScreenPanel.btn_Orange = nil
function MainScreenPanel:Init()
    local _res = ABMgr:SyncLoadRes("ui", "MainScreenPanel", typeof(GameObject))
    self.mView = GameObject.Instantiate(_res,Canvas.transform)
    self.btn_Pink=self.mView.transform:GetChild(0):Find("btn_Pink"):GetComponent(typeof(Button))
    self.btn_Pink.onClick:AddListener(function ()
        self:OnPinkBtn()
    end)
end
function MainScreenPanel:OnPinkBtn(...) print("OnPinkBtn") end
