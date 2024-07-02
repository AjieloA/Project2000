Object = {}
function Object:new()
    local _obj = {}
    self.__index = self
    setmetatable(_obj, self)
    return _obj
end
function Object:subClass(_name)
    _G[_name] = {}
    local _obj = _G[_name]
    _obj.base = self
    self.__index = self
    setmetatable(_obj, self)
end
