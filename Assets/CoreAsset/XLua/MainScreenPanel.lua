MainScreenPanel = {}
MainScreenPanel.__index = MainScreenPanel
MainScreenPanel = setmetatable(UIViewBase, MainScreenPanel)
-- return MainScreenPanel:New(MainScreenPanel)
function MainScreenPanel:OnInit(...)
    self.super:OnHide()
    print('MainScreenPanel')
end
return MainScreenPanel
