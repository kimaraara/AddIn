using System;
using System.Windows;
using System.Windows.Forms;
using AddIn.ViewModels;
using System.Diagnostics;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Controls;
using ClosedXML.Excel;
using TextBox = System.Windows.Controls.TextBox;
using System.Windows.Media;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq; // 이게 없으면 SaveFileDialog 가 오류남 ㅠ

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
        
        // 새로고침 (넣어야 하나?)




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
            // SearchView searchView = new SearchView();
            // searchView.ShowDialog();

            // 엑셀 그리드에 빈 행 추가
            var newRow = new Models.MD_PropertyItem
            {
                Order = 0, // 순서 초기값 설정
                Level = 0, // 레벨 초기값 설정
                PartName = string.Empty, // 빈 부품명
                Quantity = 0, // 수량 초기값
                SettingName = string.Empty, // 설정명 초기값
                PropertyType = string.Empty, // 속성 유형 초기값
                Name = string.Empty, // 속성명
                Value = string.Empty // 값
            };

            // 현재 바인딩 된 컬렉션에 새 행 추가
            var viewModel = (VM_MainFunctionExcel2)this.DataContext;
            viewModel.CombinedProperties.Insert(0, newRow); // 첫 번째 행에 빈 셀 추가

            // 검색 팝업 띄우기
            SearchPopup.IsOpen = true;
        }

        // 팝업 창이 닫힐 때 실행
        private void SearchPopup_Closed(object sender, EventArgs e)
        {
            // 검색 결과가 없을 때나 팝업이 닫히면 추가된 빈 셀을 제거
            var viewModel = (VM_MainFunctionExcel2)this.DataContext;

            // 빈 셀을 추가된 상태로 리스트에서 찾고 제거
            var firstRow = viewModel.CombinedProperties.FirstOrDefault(row => row.Name == string.Empty && row.Value == string.Empty);
            if (firstRow != null)
            {
                viewModel.CombinedProperties.Remove(firstRow); // 해당 셀 삭제
            }
        }

        // "검색" 텍스트 기본, 입력할 때 검색 제거
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "검색")
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "검색";
                textBox.Foreground = Brushes.Gray;
            }
        }


        // 검색 팝업창에서 확인 버튼 눌렀을 때
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 내용을 처리한 후 팝업을 닫음
            SearchPopup.IsOpen = false;
        }

        // 검색 팝업창에서 취소 버튼 눌렀을 때
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 내용을 처리한 후 팝업을 닫음
            SearchPopup.IsOpen = false;
        }

       
    }

}