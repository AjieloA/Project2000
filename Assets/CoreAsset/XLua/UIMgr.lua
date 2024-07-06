UIMgr = {}
local AllUIQue = {}
function UIMgr:OpenUI(_scriptName)
    local _script = require(_scriptName)
    _script:Init(_scriptName)
    _script:SetLayer(E_UILayer.Normal)
    LogMgr:CError("UIMgr")
end
