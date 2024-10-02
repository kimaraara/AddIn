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
        public class Part
        {
            public bool Order { get; set; }
            public bool Level { get; set; }
            public bool Hyphen { get; set; }
            public string PartName { get; set; }
            public int Quantity { get; set; }

            public string SettingName { get; set; }

            public string PARTNAME { get; set; }

            public string SPEC {  get; set; }

            public string MATERIAL {  get; set; }

            public string DESIGNED { get; set; }
            
        }

        // Data Binding 설정
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var parts = new ObservableCollection<Part>
            {
                new Part { Order = true, Level = true, Hyphen = false, PartName = "Gearbox Assy", Quantity = 1},
                new Part { Order = false, Level = true, Hyphen = true, PartName = "Housing", Quantity = 2},
                // 더 많은 데이터 추가 가능
            };

            ExcelGrid.ItemsSource = parts;
        }

        // 엑셀출력 버튼 클릭
        private void btnExcelPrint_Click(object sender, RoutedEventArgs e)
        {
            // 엑셀 파일 경로 설정
            string filePath = @"C:\Users\YOUNG\Desktop\output.xlsx";

            try
            {
                // 새로운 엑셀 워크북 생성
                using (var workbook = new XLWorkbook())
                {
                    // 새로운 워크시트 생성
                    var worksheet = workbook.Worksheets.Add("Parts");

                    // 헤더 작성
                    worksheet.Cell(1, 1).Value = "순서";
                    worksheet.Cell(1, 2).Value = "레벨";
                    worksheet.Cell(1, 3).Value = "-";
                    worksheet.Cell(1, 4).Value = "부품명";
                    worksheet.Cell(1, 5).Value = "수량";
                    worksheet.Cell(1, 6).Value = "설정명";
                    worksheet.Cell(1, 7).Value = "PARTNAME";
                    worksheet.Cell(1, 8).Value = "SPEC";
                    worksheet.Cell(1, 9).Value = "MATERIAL";
                    worksheet.Cell(1, 10).Value = "DESIGNED";

                    // DataGrid의 데이터 추가
                    var parts = (ObservableCollection<Part>)ExcelGrid.ItemsSource;
                    int row = 2; // 데이터가 시작할 행 (헤더 아래)

                    foreach (var part in parts)
                    {
                        worksheet.Cell(row, 1).Value = part.Order;
                        worksheet.Cell(row, 2).Value = part.Level;
                        worksheet.Cell(row, 3).Value = part.Hyphen;
                        worksheet.Cell(row, 4).Value = part.PartName;
                        worksheet.Cell(row, 5).Value = part.Quantity;
                        // 다른 속성 추가 가능
                        row++;
                    }

                    // 엑셀 파일 저장
                    workbook.SaveAs(filePath);
                }

                // 엑셀 파일 열기
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex) {
                MessageBox.Show("오류 발생: " + ex.Message);
            } 

            
        }

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

        }


    }

}