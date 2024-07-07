UIViewBase = {}

UIViewBase.mLayer = E_UILayer.Normal
function UIViewBase:New(_base)
    _base = _base or {}
    setmetatable(_base, self)
    self.__index = self
    _base.base = self
    return _base
end
function UIViewBase:Init(_scriptName, ...)
    self.mScriptName = _scriptName;
    if self.mView then
        self:Show()
        return
    end
    local _res = ABMgr:SyncLoadRes("ui", _scriptName, typeof(GameObject))
    self.mView = GameObject.Instantiate(_res, Canvas.transform)
    self.mView.name = _scriptName
    self:Find()
    self:RegisterEvent()
    self:OnCreate()
    self:OnInit()
end
function UIViewBase:Find()
    
end
function UIViewBase:OnInit(...) end
function UIViewBase:RegisterEvent() end
function UIViewBase:UnRegisterEvent() end
function UIViewBase:Create(...) end
function UIViewBase:OnCreate(...) end
function UIViewBase:Show(...)
    if self.mView then
        self.mView.gameObject:SetActive(true)
        self:OnShow()
        return
    end
    LogMgr:CError("UI不存在")
end
function UIViewBase:OnShow(...) end

function UIViewBase:Hide(...)
    if self.mView then
        self.mView.gameObject:SetActive(false)
        self:OnHide()
        return
    end
    LogMgr:CError("UI不存在")
end
function UIViewBase:OnHide(...) end
function UIViewBase:Destory(...)
    if self.mView then
        self:Hide()
        self:UnRegisterEvent()
        GameObject.Destroy(self.mView)
        return
    end
    LogMgr:CError("UI不存在")
end
function UIViewBase:SetLayer(_layer)
    local _layerTra = nil
    Switch(_layer, {
        [E_UILayer.Main] = function()
            _layerTra = Canvas.transform:Find("Main")
        end,
        [E_UILayer.Normal] = function()
            _layerTra = Canvas.transform:Find("Normal")
        end,
        [E_UILayer.Tips] = function()
            _layerTra = Canvas.transform:Find("Tips")
        end,
        [E_UILayer.Top] = function()
            _layerTra = Canvas.transform:Find("Top")
        end,
        default = function(...) _layerTra = nil; end
    })
    if _layerTra == nil then
        LogMgr:CError("设置层级不存在")
        return
    end
    self.mView.transform:SetParent(_layerTra)
    self.mLayer = _layer
end
