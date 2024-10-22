using AddIn.ViewModels;
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

            // ViewModel을 명시적으로 생성하여 DataContext에 설정
            DataContext = new VM_NewWindow(); 
        }

        // 시작 버튼 클릭
        private void btnMainView_Click(object sender, RoutedEventArgs e)
        {
            // EW_MainFunction ew_MainFunction = new EW_MainFunction();
            // ew_MainFunction.Show();

            // VM_MainFunctionExcel2 호출
            var viewModel = this.DataContext as VM_MainFunctionExcel2;


            // 사용자 정의 및 설정 특성 라디오 버튼의 선택 상태 확인
            bool isCustomPropertiesSelected = (bool)btnCustomProperties.IsChecked;
            bool isSettingPropertiesSelected = (bool)btnSettingProperties.IsChecked;

            // 선택된 속성에 따라 로직 수행
            if (isCustomPropertiesSelected)
            {
                viewModel?.LoadCustomProperties(); // 사용자 정의 속성 가져오기
            }
            else if (isSettingPropertiesSelected)
            {
                viewModel?.LoadSettingProperties(); // 설정 속성 가져오기
            }

            // ViewModel의 OnStartButtonClick 메서드를 호출
            viewModel.OnStartButtonClick();

            // 새로운 창 열기
            MainView mainView = new MainView();
            mainView.Show();
        }

        // 사용자 정의 라디오 버튼 클릭
        // 설정 특성 라디오 버튼 클릭
        // BOM 구조로 불러오기 체크박스 선택

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
