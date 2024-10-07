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
    /// AttrAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AttrAdd : Window
    {
        public ObservableCollection<PropertyData> PropertyList { get; set; }

        public AttrAdd()
        {
            InitializeComponent();

            // 예시 데이터
            PropertyList = new ObservableCollection<PropertyData>
        {
            new PropertyData { VariableName = "Width"},
            new PropertyData { VariableName = "Height"}
        };

            PropertyListUp.ItemsSource = PropertyList;
        }

        // LoadingRow 이벤트 핸들러를 통해 순번 설정
        private void PropertyListUp_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString(); // 1부터 시작하는 순번
        }

        public class PropertyData
        {
            public string Sequence2 { get; set; } // 순번
            public string VariableName { get; set; } // 변수명
        }

        private void CloseAttrAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}

