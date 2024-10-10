using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AddIn.Models;
using DocumentFormat.OpenXml.Wordprocessing;

 // MainView.xaml 과 연결되어 있음
namespace AddIn.ViewModels
{
    public class VM_MainFunctionExcel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 데이터 영역
        // _fileName : 클래스 내부에서만 접근이 가능, 데이터가 실제로 저장되는 공간의 역할 (데이터 보관)
        private string _fileName;

        // _fileName을 UI에 노출시키고 어떠한 사유로든 값이 변경 되었을 때, 업데이트 하는 역할 (데이터 변경 사실 알림)
        // 프로그램 내부(백엔드)와 외부(프론트엔드)에서 사용할 각각의 값
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); }
        }

        // fileName은 하나의 데이터만 저장되는 반면 CustomProperties와 ConfigurationProperties는 여러개의 데이터를 저장함
        // 사용자 속성 값
        private ObservableCollection<PropertyItem> _customProperties;
        public ObservableCollection<PropertyItem> CustomProperties
        {
            get { return _customProperties; }
            set { _customProperties = value; OnPropertyChanged(); }
        }

        // 설정 속성 값
        private ObservableCollection<PropertyItem> _configurationProperties;
        public ObservableCollection<PropertyItem> ConfigurationProperties
        {
            get { return _configurationProperties; }
            set { _configurationProperties = value; OnPropertyChanged(); }
        }

        // 커멘드 영역 : 뷰모델에서 뷰에 이벤트를 전달하는 용도
        public ICommand RefreshCommand { get; }

        public VM_MainFunctionExcel()
        {
            CustomProperties = new ObservableCollection<PropertyItem>();
            ConfigurationProperties = new ObservableCollection<PropertyItem>();
            RefreshCommand = new Relay_Command(GetProperties);
            GetProperties();
        }

        // 현재 활성화된 모델 가져오기
        private void GetProperties()
        {
            // SldWorks : 솔리드 웍스 프로그램 자체를 의미
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            // 솔리드 웍스 프로그램 인터페이스를 새로 생성 (현재 열려있는 솔리드 웍스와 직접 연결)
            // ActiveDoc : 활성화 된 문서 (호환&데이터 타입을 변환해주기 위해 붙임)

            // 솔리드 웍스 부품, 어셈블리, 도면 중 할당하기 위해 ModelDoc2 키워드 사용
            // 부품 명시적 할당 : PartDoc
            // 어셈블리 명시적 할당 : AssemblyDoc
            // 도면 명시적 할당 : DrawingDoc 키워드 사용

            // 만약 열려져 있는 파일이 없다면 swModel 은 null 이 될 것이기 때문에 그 이후 에러를 막기 위해 조건 추가
            if (swModel == null)
            {
                // 활성화된 모델이 없을 때
                // throw new InvalidOperationException("현재 활성화된 SolidWorks 모델이 없습니다.");

                // 파일 이름 가져오기
                FileName = swModel.GetTitle(); // swModel : 현재 열려 있는 파일 GetTitle : 파일 이름을 가져옴
                CustomProperties.Clear();
                ConfigurationProperties.Clear();

                // 커스텀 속성 가져오기 (사용자 정의 속성)
                // Extension :  확장 기능을 위한 부분 (Custom 속성은 확장 기능에 속하기 때문에 해당 메서드를 통해서만 접근 가능)
                // [""] : 커스텀 속성의 기본 구성을 가져오기 위해 사용
                CustomPropertyManager customPropMgr = swModel.Extension.CustomPropertyManager[""];
                string[] customPropNames = customPropMgr.GetNames(); //  사용자 정의 속성의 이름들을 가져와 문자열 배열로 할당

                // 이름들을 순회하면서 customPropMgr의 Get2메서드를 사용하여 값을 가져옴
                foreach (string propName in customPropNames)
                {
                    string valOut;
                    string resolvedValOut;
                    customPropMgr.Get2(propName, out valOut, out resolvedValOut);
                    // 값을 가져온 다음 위에서 Clear 했던 CustomProperties에 새로운 요소로 추가
                    // CustomProperties는 앞서 View파일의 데이터 그리드에 바인딩 된 값
                    CustomProperties.Add(new PropertyItem { Name = propName, Value = resolvedValOut, IsCustomProperty = true });
                }

                // 설정 속성 가져오기
                ConfigurationManager configMgr = swModel.ConfigurationManager;
                // ConfigurationManager : 솔리드 웍스 모델의 모든 구성을 관리하는 키워드
                Configuration config = configMgr.ActiveConfiguration;
                // Configuration : 솔리드 웍스 모델의 특정 구성을 나타내기 위한 클래스

                CustomPropertyManager configPropMgr = config.CustomPropertyManager;
                string[] configPropNames = configPropMgr.GetNames();

                foreach (string propName in configPropNames)
                {
                    string valOut;
                    string resolvedValOut;
                    configPropMgr.Get2(propName, out valOut, out resolvedValOut);
                    ConfigurationProperties.Add(new PropertyItem { Name = propName, Value = resolvedValOut, IsCustomProperty = false });
                }
            }
            else
            {
                FileName = "열린 파일이 없습니다.";
            }

        }
        /*
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
     }

    public class PropertyItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
                UpdateProperty();
            }
        }

        public bool IsCustomProperty { get; set; }

        private void UpdateProperty()
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel != null)
            {
                CustomPropertyManager propMgr;
                if (IsCustomProperty) // 변경된 값이 사용자 정의 값인지, 설정 속성값인지 구분
                {
                    propMgr = swModel.Extension.CustomPropertyManager[""];
                }
                else
                {
                    ConfigurationManager configMgr = swModel.ConfigurationManager;
                    Configuration config = configMgr.ActiveConfiguration;
                    propMgr = config.CustomPropertyManager;
                }

                propMgr.Set2(Name, Value); // 실제 값을 할당하는 부분 (Set2 함수 사용)
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
