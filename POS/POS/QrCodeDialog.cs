using System;
using System.Windows.Forms;
using POS.Internals;
using Telerik.WinControls.UI;

namespace POS
{
    public class QrCodeDialog
    {
        public static void Show(string content, IWin32Window owner, string title = "Bitte QR-Code einscannen")
        {
            var frm = new RadForm();

            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowIcon = false;
            frm.Text = title;
            frm.BackgroundImageLayout = ImageLayout.Zoom;

            var qrGenerator = new QRCodeGenerator();
            var qrCode = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.L);

            frm.BackgroundImage = qrCode.GetGraphic(20);

            frm.Show(owner);
        }
    }
}