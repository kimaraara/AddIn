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
using ClosedXML.Excel;
using System.Collections.ObjectModel;
using System.Data;
// using System.Windows.Forms;
using AddIn.ViewModels;
using AddIn.Models;

namespace AddIn.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new VM_MainFunctionExcel2();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (VM_MainFunctionExcel2)this.DataContext;
            if (viewModel != null)
            {
                viewModel.LoadData();
            }
        }

        // 기본기능 버튼 클릭
        private void btnBasicFunction_Click(object sender, RoutedEventArgs e)
        {
            context1.PlacementTarget = sender as Button;
            context1.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            context1.IsOpen = true;
        }

        // 파일관리 버튼 클릭
        private void btnFileMgmt_Click(object sender, RoutedEventArgs e)
        {
            context2.PlacementTarget = sender as Button;
            context2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            context2.IsOpen = true;
        }

        // 동기화 버튼 클릭
        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            context3.PlacementTarget = sender as Button;
            context3.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            context3.IsOpen = true;
        }

        // 작업관리 버튼 클릭
        private void btnWorkMgmt_Click(object sender, RoutedEventArgs e)
        {
            context4.PlacementTarget = sender as Button;
            context4.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            context4.IsOpen = true;
        }


        // 엑셀 화면
        // Data Binding 설정

        // 10/16 오후 추가
        // 엑셀출력 버튼 클릭
        private void btnExcelPrint_Click(object sender, RoutedEventArgs e)
        {
            // 경고창 표시
            MessageBoxResult result = MessageBox.Show(
                "현재 화면을 엑셀 출력 하시겠습니까?",  // 메시지 내용
                "엑셀 출력",                                          // 메시지 상자 제목
                MessageBoxButton.YesNo,                 // 버튼 옵션 (예/아니오)
                MessageBoxImage.Warning);             // 경고 아이콘

            // 사용자가 "예"를 선택했을 때만 엑셀 출력 동작 실행
            if (result == MessageBoxResult.Yes)
            {
                // 엑셀 출력 로직
                // 예: 엑셀 파일 생성 및 저장
            }
        }

        // 새로고침



        // 미리보기 축소 버튼 클릭
        // 미리보기 확장 버튼 클릭

        // 속성저장 버튼 클릭
        private void btnAttrSave_Click(object sender, RoutedEventArgs e)
        {

        }

        // 속성설정 버튼 클릭
        private void btnAttrSet_Click(object sender, RoutedEventArgs e)
        {
            AttrSet attrSet = new AttrSet();
            attrSet.Show();
        }

        // 검색 버튼 클릭
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchView searchView = new SearchView();
            searchView.ShowDialog();
        }

        
    }

}