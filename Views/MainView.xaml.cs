using System;
using System.Windows;
using System.Windows.Forms;
using AddIn.ViewModels;
using System.Diagnostics;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Controls;
using ClosedXML.Excel; // 이게 없으면 SaveFileDialog 가 오류남 ㅠ

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

        // 10/16 오후 추가 + 10/17 추가 및 수정
        // 엑셀출력 버튼 클릭
        private void btnExcelPrint_Click(object sender, RoutedEventArgs e)
        {
            // 파일 이름 지정
            string fileName = @"엑셀파일 저장_" + DateTime.Now.ToString("yyyy/MM/dd/ddd");
            string filePath = string.Empty;

            // 경고창 표시
            MessageBoxResult result = MessageBox.Show(
                "현재 화면을 엑셀 출력 하시겠습니까?",  // 메시지 내용
                "엑셀 출력",                                          // 메시지 상자 제목
                MessageBoxButton.YesNo,                 // 버튼 옵션 (예/아니오)
                                                        //MessageBoxImage.Warning);             // 경고 아이콘
                MessageBoxImage.Question);


            // 사용자가 "예"를 선택했을 때만 엑셀 출력 동작 실행
            if (result == MessageBoxResult.Yes)
            {
                // 엑셀 출력 로직
                // 예: 엑셀 파일 생성 및 저장
                // string fileName = "";

                // 엑셀 애플리케이션 시작
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true; // 엑셀 프로그램 보이기

                // 새 워크북 생성
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                // DataGrid의 데이터를 엑셀 워크시트로 복사
                var data = ExcelGrid.ItemsSource;  // ExcelGrid 데이터

                // 헤더 설정
                for (int i = 0; i < ExcelGrid.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = ExcelGrid.Columns[i].Header.ToString();
                }

                // 데이터 추가
                int row = 2;  // 2번째 행부터 시작

                foreach (var item in data)
                {
                    for (int col = 0; col < ExcelGrid.Columns.Count; col++)
                    {
                        var cellValue = ExcelGrid.Columns[col].GetCellContent(item) as TextBlock;
                        worksheet.Cells[row, col + 1] = cellValue?.Text ?? "";
                    }
                    row++;
                }
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