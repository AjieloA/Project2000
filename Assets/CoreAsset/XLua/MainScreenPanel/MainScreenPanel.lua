MainScreenPanel = setmetatable({}, {__index = MainScreenPanelBase})
function MainScreenPanel:OnInit(...)
    print('MainScreenPanel' .. self.Btn_Pink.name)
end
function MainScreenPanel:RegisterEvent()
    self.Btn_Pink.onClick:AddListener(function() self:OnPinkBtn() end)
end
function MainScreenPanel:UnRegisterEvent()
    -- self.Btn_Pink.onClick:AddListener(function() self:OnPinkBtn() end)
end
local _index = 0
function MainScreenPanel:OnPinkBtn(...)
    local _btn = UIMgr:OpenUI("TestPanel")
    _btn:Test(_index)
    _index = _index + 1
end
