-- TestPanelBase = {}
-- TestPanelBase.__index = TestPanelBase
TestPanelBase = setmetatable({}, {__index = UIViewBase})
function TestPanelBase:Find(...)
self.mTra_Root=self.mView.transform:Find("tra_Root"):GetComponent(typeof(RectTransform))
self.mBtn_A=self.mView.transform:Find("tra_Root/btn_A"):GetComponent(typeof(Button))

end