using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AddIn.Models
{
    public class MD_PropertyItem : INotifyPropertyChanged
    {
        private int _order; // 순서
        private int _level; // 레벨
        private string _partName; // 부품명
        private int _quantity; // 수량
        private string _settingName; // 설정명
        // 10/16 오후 추가

        private string _name;
        private string _value;
        private bool _isCustomProperty;

        private SldWorks _swApp;
        private ModelDoc2 _swModel;
        private CustomPropertyManager _propMgr;
        

        public MD_PropertyItem()
        {
            // SolidWorks 객체 초기화
            _swApp = new SldWorks();
            _swModel = (ModelDoc2)_swApp.ActiveDoc;

            if (_swModel != null)
            {
                // 사용자 정의 속성 또는 설정 속성에 따라 CustomPropertyManager 초기화
                if (IsCustomProperty)
                {
                    _propMgr = _swModel.Extension.CustomPropertyManager[""];
                }
                else
                {
                    ConfigurationManager configMgr = _swModel.ConfigurationManager;
                    Configuration config = configMgr.ActiveConfiguration;
                    _propMgr = config.CustomPropertyManager;
                }
            }
        }

        // 10/16 오후 추가
        // 순서
        public int Order
        {
            get => _order;
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged();
                }
            }
        }

        // 레벨
        public int Level
        {
            get => (int)_level;
            set
            {
                if (_level != value) 
                {
                    _level = value;
                    OnPropertyChanged();
                }
            }
        }

        // 부품명
        public string PartName
        {
            get => _partName;
            set
            {
                if (_partName != value)
                {
                    _partName = value;
                    OnPropertyChanged();
                }
            }
        }

        // 수량
        public int Quantity
        {
            get => _quantity;
            set
            {
                if( _quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        // 설정명
        public string SettingName
        {
            get => _settingName;
            set
            {
                if( _settingName != value)
                {
                    _settingName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    UpdateProperty(); // 값이 변경될 때만 SOLIDWORKS 속성 업데이트
                }
            }
        }

        public bool IsCustomProperty
        {
            get => _isCustomProperty;
            set
            {
                _isCustomProperty = value;
                OnPropertyChanged();
            }
        }

        public string PropertyType { get; set; } // 속성 타입 (사용자 정의 속성 또는 설정 속성)


        // public bool IsCustomProperty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // SOLIDWORKS 속성 업데이트 로직
        private void UpdateProperty()
        {
            if (_swModel != null && _propMgr != null)
            {
                // 사용자 정의 속성 또는 설정 속성을 업데이트
                int result = _propMgr.Set2(Name, Value); // Set2 메서드를 통해 속성 값 업데이트

                // 결과 확인 및 처리
                if (result == (int)swCustomInfoAddResult_e.swCustomInfoAddResult_AddedOrChanged)
                {
                    Console.WriteLine($"속성 '{Name}'이(가) 성공적으로 업데이트되었습니다.");
                }
                else
                {
                    Console.WriteLine($"속성 '{Name}' 업데이트 중 오류가 발생했습니다.");
                }

                // 변경 사항을 문서에 저장하지는 않음
                _swModel.EditRebuild3(); // 재빌드 호출로 모델에 반영 (필요 시)
            }
            else
            {
                Console.WriteLine("SOLIDWORKS 모델이나 속성 관리자에 접근할 수 없습니다.");
            }
        }
    }
}
