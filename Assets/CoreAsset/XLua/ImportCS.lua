require("Object")
require("SplitTools")
Json = require("JsonUtility")
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
UI = CS.UnityEngine.UI
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect
ABMgr = CS.ABMgr.Instance
LogMgr=CS.LogMgr.Instance
Canvas = GameObject.Find("Canvas")
function Switch(_value,_caseTable)
    local _case=_caseTable[_value] or _caseTable.default
    if _case then
        return _case()
    end
end
E_UILayer={
    Main=1,
    Normal=2,
    Tips=3,
    Top=4,
}
