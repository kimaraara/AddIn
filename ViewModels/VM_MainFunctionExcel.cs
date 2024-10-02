using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AddIn.Models;

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

            for(int i=0; i<100; i++)
            {
                CustomProperties.Add(new ExcelCustomProperty
                {
                    Order = i + 1, // Order(순서) 는 1부터 시작
                    Level = 0,      // Level(레벨) 은 0부터 시작
                    Quantity = i + 1,
                    PartName = "Part " + (i + 1) // 예시로 부품명 추가

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
