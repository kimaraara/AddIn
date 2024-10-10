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

namespace AddIn.ViewModels
{
    // MainView.xaml 과 연결되어 있음
    public class VM_MainFunctionExcel : INotifyPropertyChanged
    {
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

            if (swModel == null)
            {
                // 활성화된 모델이 없을 때 , 여기부터 다시 
                throw new InvalidOperationException("현재 활성화된 SolidWorks 모델이 없습니다.");

                // 파일 이름 가져오기
                FileName = swModel.GetTitle();
                CustomProperties.Clear();
                ConfigurationProperties.Clear();
            }
        }
        public void LoadData()
        {
            CustomProperties = new ObservableCollection<ExcelCustomProperty>();

            // SolidWorks 애플리케이션 인스턴스 가져오기
            SldWorks swApp = Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application")) as SldWorks;
            if (swApp == null)
            {
                // SolidWorks 가 실행 중이지 않다면 예외 처리 or 로그 출력
                throw new InvalidOperationException("SolidWorks 가 실행 중이지 않습니다.");
            }


           

            // 파일명 가져오기
            string modelName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

            // 사용자 정의 속성 가져오기
            CustomPropertyManager customPropertyManager = swModel.Extension.CustomPropertyManager[""];

            object[] propertyNamesObj;
            // customPropertyManager.GetNames(out propertyNamesObj); // 속성 이름을 가져옴
            propertyNamesObj = customPropertyManager.GetNames() as object[];  // 반환값으로 처리 (out 파라미터 없음)

            string[] propertyNames = propertyNamesObj?.Cast<string>().ToArray();

            if (propertyNames != null)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    string propertyName = propertyNames[i];
                    string PropertyValue;
                    string resolveValue;

                    // 각 속성의 값을 가져옴
                    customPropertyManager.Get4(propertyNames[i], false, out PropertyValue, out resolveValue);
                    // CustomProperties에 추가
                    CustomProperties.Add(new ExcelCustomProperty
                    {
                        Order = i + 1, // Order는 1부터 시작
                        Level = i,     // Level은 0부터 시작
                        Quantity = i + 1,
                        PartName = modelName,           // 파일명을 PartName으로 설정
                        PropertyName = propertyName, // 속성 이름 추가
                        PropertyValue = resolveValue,    // 속성 값 추가
                    });
                }
            }

            OnPropertyChanged(nameof(CustomProperties));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}