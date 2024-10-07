using AddIn.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            InitializeComponent(); //  XAML에서 정의한 UI 요소들을 초기화
            DataContext = new PropertyViewModel(); // ViewModel 인스턴스를 DataContext에 설정
        }

        public class PropertyViewModel
        {
            public ObservableCollection<PropertyItem> Properties { get; set; } // PropertyItem 객체를 담는 컬렉션(데이터가 수정되면 UI에 자동 업데이트)

            public PropertyViewModel() //  샘플 데이터
            {
                Properties = new ObservableCollection<PropertyItem> // 초기 데이터 설정
        {
            new PropertyItem { Sequence = 1, VariableName = "변수1", PropertyName = "속성1", DefaultValue = "기본값1", Width = 100, IsSynchronized = true },
            new PropertyItem { Sequence = 2, VariableName = "변수2", PropertyName = "속성2", DefaultValue = "기본값2", Width = 200, IsSynchronized = false }
        };
            }
        }

        public class PropertyItem
        {
            public int Sequence { get; set; } // 데이터 항목의 순서
            public string VariableName { get; set; } // 변수명
            public string PropertyName { get; set; } // 속성명
            public string DefaultValue { get; set; } // 기본값
            public double Width { get; set; } // 너비
            public bool IsSynchronized { get; set; } // 동기화 여부 (체크박스)
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
            var viewModel = DataContext as PropertyViewModel; // DataContext를 PropertyViewModel로 캐스팅
            if (viewModel != null)
            {
                viewModel.Properties.Add(new PropertyItem { Sequence = viewModel.Properties.Count + 1 }); // 새 아이템 추가
            }
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
