include(".\\Plugins\\test.js.inc");

function init() { return size_format(filesize("Std.dll"), 2); }
function close() { }

var plugin =
{
    version: "1.0.0.0",
    init: init,
    onClose: close
};

showHelloWorld();

plugin;