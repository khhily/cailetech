using System;
using System.Windows.Forms;

namespace CL.LotteryGameService.Model
{
    public class WritText
    {
        RichTextBox tb = null;
        public WritText(RichTextBox tb = null)
        {
            this.tb = tb;
        }
        public delegate void textbox_delegate(string msg);
        public void WritTextBox(string msg)
        {
            if (tb == null) return;
            if (tb.InvokeRequired)
            {
                textbox_delegate dt = new textbox_delegate(Writs);
                tb.Invoke(dt, new object[] { msg });
            }
            else
                tb.AppendText(string.Format(" {0}：{1}\n", DateTime.Now, msg));
        }
        public void Writs(string msg)
        {
            tb.AppendText(string.Format(" {0}：{1}\n", DateTime.Now, msg));
        }
    }
}
