include(".\\Plugins\\test.js.inc");
ns("clr.System", "system");

function init() { return size_format(filesize(".\\Plugins\\test.js.inc"), 2); }
function close() { }

var plugin =
{
    name: "TestPlugin",
    version: "1.0.0.0",
    init: init,
    onClose: close
};

showHelloWorld();

system.Windows.Forms.MessageBox.Show("hello from clr: " + plugin.name);

plugin;