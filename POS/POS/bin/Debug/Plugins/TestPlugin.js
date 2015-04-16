function init() { return filesize("Std.dll"); }
function close() { }

var plugin =
{
    version: "1.0.0.0",
    init: init,
    onClose: close
};

plugin;