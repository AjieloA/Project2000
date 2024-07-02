function string.split(_input, _delimiter)
    _input = tostring(_input)
    _delimiter = tostring(_delimiter)
    if (_delimiter == '') then return false end
    local _pos, _arr = 0, {}
    local _find = function()
        return string.find(_input, _delimiter, _pos, true)
    end
    for _st, _sp in find do
        table.insert(_arr, string.sub(_input, _pos, st - 1))
        _pos = _pos + 1
    end
    table.insert(_arr, string.sub(_input, _pos))
    return _arr
end
