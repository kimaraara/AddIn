using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AddIn.Views
{
    /// <summary>
    /// NewWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewWindow : UserControl
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        // 시작 버튼 클릭
        private void btnMainFunction_Click(object sender, RoutedEventArgs e)
        {
            EW_MainFunction ew_MainFunction = new EW_MainFunction();
            ew_MainFunction.Show();
        }

        // 부품분리 버튼 클릭
        private void btnPartSeparate_Click(object sender, RoutedEventArgs e)
        {

        }

        // 부품이름변경 버튼 클릭
        private void btnPartNameChange_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
