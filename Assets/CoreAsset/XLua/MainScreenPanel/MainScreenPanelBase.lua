-- MainScreenPanelBase = {}
-- MainScreenPanelBase.__index = MainScreenPanelBase
MainScreenPanelBase = setmetatable({}, {__index = UIViewBase})
function MainScreenPanelBase:Find(...)
    self.Tra_Root = self.mView.transform:Find("tra_Root")
    self.Btn_Pink = self.Tra_Root.transform:Find("btn_Pink"):GetComponent(typeof(Button))
end