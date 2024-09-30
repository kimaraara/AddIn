using AddIn.Views;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace AddIn
{
    [Guid("31F68D64-A1E2-4A8B-92B4-A2DFA5F7D935")]
    [ComVisible(true)]
    public class MyAddIn : SwAddin
    {
        private ISldWorks _swApp;
        private int _addinID;
        private ITaskpaneView taskPaneView;


        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            _swApp = (ISldWorks)ThisSW;
            _addinID = Cookie;

            // Task Pane 추가
            taskPaneView = (ITaskpaneView)_swApp.CreateTaskpaneView2("", "WPF로 만든 또다른 창!");

            // WPF 유저 컨트롤을 ElementHost에 할당
            var elementHost = new ElementHost
            {
                Child = new NewWindow(), // WPF 유저 컨트롤 설정
                Dock = DockStyle.Fill // 크기를 Task Pane에 맞게 설정
            };

            // DisplayWindowFromHandle 메서드에 IntPtr로 바로 전달
            taskPaneView.DisplayWindowFromHandle(elementHost.Handle.ToInt32());

            return true;
        }


        public bool DisconnectFromSW()
        {
            if (taskPaneView != null)
            {
                taskPaneView.DeleteView();
            }
            return true;
        }
    }
}
