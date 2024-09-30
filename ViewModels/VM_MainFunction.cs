using AddIn.Models;
using SolidWorks.Interop.sldworks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddIn.ViewModels
{
    public class VM_MainFunction : INotifyPropertyChanged
    {
        // 데이터 영역
        private string _fileName; // 클래스 내부에서만 접근이 가능, 데이터가 실제로 저장되는 공간의 역할
        public string FileName // _fileName을 UI에 노출시키고 어떠한 사유로 
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PropertyItem> _customProperties;
        public ObservableCollection<PropertyItem> CustomProperties
        {
            get { return _customProperties; }
            set { _customProperties = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PropertyItem> _configurationProperties;
        public ObservableCollection<PropertyItem> ConfigurationProperties
        {
            get { return _configurationProperties; }
            set { _configurationProperties = value; OnPropertyChanged(); }
        }

        // 커멘드 영역
        public ICommand RefreshCommand { get; }

        // 생성자
        public VM_MainFunction()
        {
            CustomProperties = new ObservableCollection<PropertyItem>();
            ConfigurationProperties = new ObservableCollection<PropertyItem>();
            RefreshCommand = new Relay_Command(GetProperties);
            GetProperties();
        }

        // 속성 가져오기 (사용자, 설정 둘 다)
        private void GetProperties()
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel != null)
            {
                // 파일 이름 가져오기
                FileName = swModel.GetTitle();
                CustomProperties.Clear();
                ConfigurationProperties.Clear();

                // 커스텀 속성 가져오기
                CustomPropertyManager customPropMgr = swModel.Extension.CustomPropertyManager[""];
                string[] customPropNames = customPropMgr.GetNames();
                foreach (string propName in customPropNames)
                {
                    string valOut;
                    string resolvedValOut;
                    customPropMgr.Get2(propName, out valOut, out resolvedValOut);
                    CustomProperties.Add(new PropertyItem { Name = propName, Value = resolvedValOut, IsCustomProperty = true });
                }

                // 설정 속성 가져오기
                ConfigurationManager configMgr = swModel.ConfigurationManager;
                Configuration config = configMgr.ActiveConfiguration;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
                if (IsCustomProperty)
                {
                    propMgr = swModel.Extension.CustomPropertyManager[""];
                }
                else
                {
                    ConfigurationManager configMgr = swModel.ConfigurationManager;
                    Configuration config = configMgr.ActiveConfiguration;
                    propMgr = config.CustomPropertyManager;
                }

                propMgr.Set2(Name, Value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
