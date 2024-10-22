using AddIn.Models; // RelayCommand 가 정의된 네임 스페이스
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AddIn.ViewModels
{
    internal class VM_NewWindow : INotifyPropertyChanged
    {
        // 인터페이스 구현 INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // 기본 선택 상태를 위한 속성
        private bool _isCustomPropertySelected;
        public bool IsCustomPropertySelected
        {
            get => _isCustomPropertySelected;
            set
            {
                if (_isCustomPropertySelected != value)
                {
                    _isCustomPropertySelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isConfigurationPropertySelected;
        public bool IsConfigurationPropertySelected
        {
            get => _isConfigurationPropertySelected;
            set
            {
                if (_isConfigurationPropertySelected != value)
                {
                    _isConfigurationPropertySelected = value;
                    OnPropertyChanged();
                }
            }
        }

        // 명령 예시
        public ICommand ExampleCommand { get; }

        public VM_NewWindow()
        {
            // 기본 선택 상태 설정
            IsCustomPropertySelected = true; // 기본으로 사용자 정의 속성 선택

            // 명령 초기화
            ExampleCommand = new Relay_Command(ExecuteExampleCommand); // 이 부분에서 RelayCommand 사용
        }

        private void ExecuteExampleCommand(object parameter)
        {
            // Console.WriteLine("Example command executed!");
            try
            {
                // SolidWorks와 상호작용하는 코드
                // 예시: SolidWorks.Application.Application.Run();

                Console.WriteLine("SolidWorks가 실행되었습니다.");
            }
            catch (Exception ex)
            {
                // 예외가 발생했을 때 오류 메시지를 출력
                MessageBox.Show($"오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
