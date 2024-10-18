using AddIn.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddIn.ViewModels
{
    public class VM_MainFunctionExcel2 : INotifyPropertyChanged
    {
        private string _fileName;
        private ObservableCollection<MD_PropertyItem> _customProperties;
        private ObservableCollection<MD_PropertyItem> _configurationProperties;
        private ObservableCollection<MD_PropertyItem> _combinedProperties; // 통합된 속성
        public ObservableCollection<MD_PropertyItem> Properties { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public VM_MainFunctionExcel2()
        {
            // 속성 초기화
            CustomProperties = new ObservableCollection<MD_PropertyItem>();
            ConfigurationProperties = new ObservableCollection<MD_PropertyItem>();
            CombinedProperties = new ObservableCollection<MD_PropertyItem>(); // 통합된 속성 
            Properties = new ObservableCollection<MD_PropertyItem>();

            // SolidWorks 속성 불러오기
            GetProperties();

            // 검색 버튼 눌렀을 때 팝업창에 조회되는 파일 목록 (10/18)
            // 실제 파일명 로드 예시 (예: 디렉토리에서 파일 목록을 가져오는 방식)
            string directoryPath = @"C:\Path\To\Your\Files";
            var fileNames = Directory.GetFiles(directoryPath, "*.sldprt") // SolidWorks 부품 파일만 예시로
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToList();

            foreach (var fileName in fileNames)
            {
                _fileNames.Add(fileName);  // 실제 파일명 추가
            }

            // 초기 필터링 (파일명에 대한 필터링)
            FilterFileNames();
        }


        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        // 사용자 정의 속성
        public ObservableCollection<MD_PropertyItem> CustomProperties
        {
            get => _customProperties;
            set
            {
                if (_customProperties != value)
                {
                    _customProperties = value;
                    OnPropertyChanged();
                }
            }
        }

        // 설정 속성
        public ObservableCollection<MD_PropertyItem> ConfigurationProperties
        {
            get => _configurationProperties;
            set
            {
                if (_configurationProperties != value)
                {
                    _configurationProperties = value;
                    OnPropertyChanged();
                }
            }
        }

        // 통합된 속성
        public ObservableCollection<MD_PropertyItem> CombinedProperties
        {
            get => _combinedProperties;
            set
            {
                if (_combinedProperties != value)
                {
                    _combinedProperties = value;
                    OnPropertyChanged();
                }
            }
        }

        // 속성 변경 알림
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // SolidWorks에서 속성 가져오기
        private void GetProperties()
        {
            SldWorks swApp = new SldWorks();
            swApp = Marshal.GetActiveObject("SldWorks.Application") as SldWorks;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel != null)
            {
                // 파일 이름 가져오기
                FileName = swModel.GetTitle();

                // 사용자 정의 속성 가져오기
                CustomPropertyManager customPropMgr = swModel.Extension.CustomPropertyManager[""];
                string[] customPropNames = customPropMgr.GetNames();
                if (customPropNames != null)
                {
                    foreach (var propName in customPropNames)
                    {
                        string valOut, resolvedValOut;
                        customPropMgr.Get2(propName, out valOut, out resolvedValOut);
                        // CustomProperties.Add(new MD_PropertyItem { Name = propName, Value = resolvedValOut, IsCustomProperty = true });
                        var customProp = new MD_PropertyItem { Name = propName, Value = resolvedValOut, IsCustomProperty = true };
                        CustomProperties.Add(customProp);
                        CombinedProperties.Add(customProp); // 통합 컬렉션에 추가
                    }
                }

                // 설정 속성 가져오기
                ConfigurationManager configMgr = swModel.ConfigurationManager;
                Configuration config = configMgr.ActiveConfiguration;
                CustomPropertyManager configPropMgr = config.CustomPropertyManager;
                string[] configPropNames = configPropMgr.GetNames();

                if (configPropNames != null)
                {
                    foreach (var propName in configPropNames)
                    {
                        string valOut, resolvedValOut;
                        configPropMgr.Get2(propName, out valOut, out resolvedValOut);
                        var configProp = new MD_PropertyItem
                        {
                            Name = propName,
                            Value = resolvedValOut,
                            IsCustomProperty = false,
                            PropertyType = "설정 속성" // 속성 유형 추가
                        };
                        ConfigurationProperties.Add(configProp);
                        CombinedProperties.Add(configProp); // 통합 컬렉션에 추가
                    }
                }
            }
            else
            {
                FileName = "열린 파일이 없습니다.";
            }
        }

        // LoadData 메서드: SolidWorks 또는 Excel에서 데이터를 가져오는 로직을 여기에 작성
        public void LoadData()
        {
            // SolidWorks에서 속성값을 가져오는 로직 추가
            CustomProperties.Clear();  // 기존 데이터를 초기화

            // SolidWorks에서 모델 속성을 가져오는 코드 추가
            SldWorks swApp = Marshal.GetActiveObject("SldWorks.Application") as SldWorks;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel != null)
            {
                // 사용자 정의 속성 가져오기
                CustomPropertyManager customPropMgr = swModel.Extension.CustomPropertyManager[""];
                string[] customPropNames = customPropMgr.GetNames();

                if (customPropNames != null)
                {
                    foreach (var propName in customPropNames)
                    {
                        string valOut, resolvedValOut;
                        customPropMgr.Get2(propName, out valOut, out resolvedValOut);

                        // 사용자 정의 속성 값을 ObservableCollection에 추가
                        CustomProperties.Add(new MD_PropertyItem
                        {
                            Name = propName,
                            Value = resolvedValOut,
                            IsCustomProperty = true
                        });
                    }
                }
            }
            else
            {
                // 모델이 열려 있지 않은 경우 처리
                FileName = "열린 파일이 없습니다.";
            }
        }

        // 새로고침 메서드
        public void RefreshData()
        {
            CustomProperties.Clear();
            ConfigurationProperties.Clear();
            CombinedProperties.Clear();

            // SolidWorks에서 최신 속성값을 가져오는 로직 추가
            GetProperties(); // 속성 새로고침
        }

        // 검색 버튼 눌렀을 때 팝업창에 조회되는 파일 목록 (10/18)
        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(); // INotifyPropertyChanged 사용 시
                }
            }
        }

        private ObservableCollection<string> _fileNames = new ObservableCollection<string>();
        private ObservableCollection<string> _filteredFileNames = new ObservableCollection<string>();

        private string _propertyName;
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                    OnPropertyChanged(nameof(PropertyName));  // PropertyChanged 이벤트 호출
                }
            }
        }

        public ObservableCollection<string> FilteredFileNames
        {
            get => _filteredFileNames;
            set
            {
                if (_filteredFileNames != value)
                {
                    _filteredFileNames = value;
                    OnPropertyChanged();
                }
            }
        }

        // 검색어에 맞게 파일명 필터링 (10/18 Contains 오류)
        private void FilterFileNames()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                FilteredFileNames = new ObservableCollection<string>(_fileNames);
            }
            else
            {
                FilteredFileNames = new ObservableCollection<string>(_fileNames.Where(f => f.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));
            }
        }







        // public event PropertyChangedEventHandler PropertyChanged;

    }
}
