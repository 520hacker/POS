include(".\\Plugins\\test.js.inc");
ns("clr.System", "sys");

function init() { return size_format(filesize(".\\Plugins\\test.js.inc"), 2); }
function close() { }

var plugin =
{
    name: "TestPlugin",
    version: "1.0.0.0",
    init: init,
    onClose: close
};

var req = new XMLHttpRequest();
req.open("get", "http://www.google.de/?q=c++");

req.send(null);

showHelloWorld();
ui.Info("Plugin", JSON.stringify(plugin));

ui.AddPayButton("Plugin", function (sender) { ui.Info("Bezahlt", "Es wurde mit " + plugin.name + " bezahlt!" + "\r" + base64_encode("Chris Anders")); });

plugin;