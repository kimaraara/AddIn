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
    /// AttrSet.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AttrSet : Window
    {
        public AttrSet()
        {
            InitializeComponent();
        }

        private void CloseAttrSet_Click(object sender, RoutedEventArgs e)
        {
            // 설정창 닫기
            Window.GetWindow(this)?.Close();
        }

        // 모델 내 속성 추가 버튼 클릭
        private void btnAttrAddModel_Click(object sender, RoutedEventArgs e)
        {
            AttrAdd attrAddModel = new AttrAdd();
            attrAddModel.Show();
        }

        // 신규추가 버튼 클릭
        private void btnNewAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        // 삭제 버튼 클릭
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        // 속성 변경 저장 버튼 클릭
        private void btnAttrChangeSave_Click(object sender, RoutedEventArgs e)
        {

        }

    }

   
}
