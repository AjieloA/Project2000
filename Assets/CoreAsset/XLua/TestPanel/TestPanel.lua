-- TestPanel = {}
-- TestPanel.__index = TestPanel
TestPanel = setmetatable({}, {__index = TestPanelBase})
function TestPanel:RegisterEvent()
    self.mBtn_A.onClick:AddListener(function() self:OnBtn() end)
end
function TestPanel:UnRegisterEvent() end
local mIndex= -1
function TestPanel:OnBtn(...) 
    print("mBtn_A:"..mIndex) 
end
function TestPanel:Test(_index)
    mIndex=_index
end

