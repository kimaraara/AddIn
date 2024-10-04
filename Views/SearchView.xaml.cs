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
    /// SearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchView : Window
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "검색")
            {
                txtSearch.Text = string.Empty; // 기본 텍스트를 지움
                txtSearch.Foreground = Brushes.Black; // 텍스트 색상을 검정색으로 변경
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "검색"; // 기본 텍스트를 다시 표시
                txtSearch.Foreground = Brushes.LightGray; // 텍스트 색상을 회색으로 변경
            }
        }


        internal void Show()
        {
            throw new NotImplementedException();
        }
    }
}
