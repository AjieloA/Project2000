UIMgr = {}
local AllUIQue = {}
local mUIAllBaseTable = {}
local mUIAllView = {}
function UIMgr:OpenUI(_scriptName)
    local _base = _scriptName .. "Base"
    local _basePath = _scriptName .. "/" .. _scriptName .. "Base"
    if (mUIAllBaseTable[_base] == nil) then
        require(_basePath)
        mUIAllBaseTable[_base] = _G[_base]
    end
    if (mUIAllView[_scriptName] == nil) then
        require(_scriptName .. "/" .. _scriptName)
        mUIAllView[_scriptName] = _G[_scriptName]
    end
    local _script = mUIAllView[_scriptName]:New()
    AllUIQue[_scriptName] = _script
    _script:Init(_scriptName)
    _script:SetLayer(E_UILayer.Normal)
    return _script
end
