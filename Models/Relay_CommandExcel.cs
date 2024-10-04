using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AddIn.Models
{
    internal class Relay_CommandExcel : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Relay_CommandExcel(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class ExcelCustomProperty
    {
        public int Order { get; set; }  // 순서 1부터 시작
        public int Level { get; set; }  // 레벨 0부터 시작
        public string PartName { get; set; }  // 부품명
        public int Quantity { get; set; }  // 수량
        public string SettingName { get; set; }  // 설정명
        public string PARTNAME { get; set; }  // PARTNAME
        public string SPEC { get; set; }  // SPEC 사양
        public string MATERIAL { get; set; }  // MATERIAL 재질
        public string DESIGNED { get; set; }  // DESIGNED
        public string WEIGHT { get; set; }  // WEIGHT 무게
        public string FINISHMAKER { get; set; }  // FINISH/MAKER 마감/제작자
        public bool CHECKED { get; set; }  // CHECKED 확인 여부 (체크박스)
        public bool APPROVED { get; set; }  // APPROVED 승인 여부 (체크박스)
        public string PropertyName { get; internal set; }
        public object PropertyValue { get; internal set; }
    }


}
