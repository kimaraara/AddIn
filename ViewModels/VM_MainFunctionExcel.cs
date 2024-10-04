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
    public class VM_MainFunctionExcel : INotifyPropertyChanged
    {


        private ObservableCollection<ExcelCustomProperty> _customProperties;
        public ObservableCollection<ExcelCustomProperty> CustomProperties
        {
            get => _customProperties;
            set
            {
                _customProperties = value;
                OnPropertyChanged(nameof(CustomProperties));
            }
        }

        public VM_MainFunctionExcel()
        {
            LoadData();
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

            // 현재 활성화된 모델 가져오기
            ModelDoc2 swModel = swApp.ActiveDoc;
            if (swModel == null)
            {
                // 활성화된 모델이 없을 때
                throw new InvalidOperationException("현재 활성화된 SolidWorks 모델이 없습니다.");
            }

            // 파일명 가져오기
            string modelName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

            // 사용자 정의 속성 가져오기
            CustomPropertyManager customPropertyManager = swModel.Extension.CustomPropertyManager[""];
            string[] propertyNames;
            customPropertyManager.GetNames(out propertyNames); // 속성 이름을 가져옴

            for (int i = 0; i < propertyNames.Length; i++) 
            {
                string propertyName = propertyNames[i];
                string PropertyValue;
                string resolveValue;

                // 각 속성의 값을 가져옴
                customPropertyManager.Get4(propertyNames, false, out PropertyValue, out resolveValue, out _);
                // CustomProperties에 추가
                CustomProperties.Add(new ExcelCustomProperty
                {
                    Order = i + 1, // Order는 1부터 시작
                    Level = i,     // Level은 0부터 시작
                    Quantity = i + 1,
                    PartName = modelName,        // 파일명을 PartName으로 설정
                    PropertyName = propertyName, // 속성 이름 추가
                    PropertyValue = resolveValue // 속성 값 추가
                });
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