using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Shingles.Errors;
using Shingles.Shingle.Factory;

namespace ShinglesGUI
{
    public class MainViewModel:INotifyPropertyChanged
    {
        #region Приватные поля

        #endregion


        #region Пользовательский интерфейс

        #region Свойства

        public string FirstText { get; set; }

        public string SecondText { get; set; }

        [DefaultValue(1), Range(1, int.MaxValue)]
        public int ShingleSize { get; set; }

        private double _result;
        public double Result
        {
            get { return _result; }
            private set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged("Result");                                        
                }
            }
        }

        private string _resultString;
        public string ResultString
        {
            get { return _resultString; }
            private set
            {
                if (_resultString != value)
                {
                    _resultString = value;
                    OnPropertyChanged("ResultString");
                }
            }            
        }

        private bool _isShingles;
        public bool IsShingles
        {
            get { return _isShingles; }
            set
            {
                if (_isShingles != value)
                {
                    _isShingles = value;
                    OnPropertyChanged("IsShingles");
                }
            }
        }

        private bool _isSuperShingles;
        public bool IsSuperShingles
        {
            get { return _isSuperShingles; }
            set
            {
                if (_isSuperShingles != value)
                {
                    _isSuperShingles = value;
                    OnPropertyChanged("IsSuperShingles");
                }
            }
        }

        private bool _isMegaShingles;
        public bool IsMegaShingles
        {
            get { return _isMegaShingles; }
            set
            {
                if (_isMegaShingles != value)
                {
                    _isMegaShingles = value;
                    OnPropertyChanged("IsMegaShingles");
                }
            }
        }

        #endregion

        #region Команды

        public ICommand CalculateCommand { get; private set; }

        #endregion

        #endregion


        #region Конструктор

        public MainViewModel()
        {

            CalculateCommand = new DelegateCommand(calculate);

            ShingleSize = 10;

            IsShingles = true;
            IsMegaShingles = false;
            IsSuperShingles = false;
        }

        #endregion


        #region Обработчики

        private void calculate()
        {
            try
            {
                var _resolver = new ShingleFactory().GetShingleResolver(IsShingles, IsSuperShingles, IsMegaShingles);
                Result = _resolver.CalculateShingles(FirstText, SecondText, ShingleSize);
                ResultString = Result.ToString();
            }
            catch (ShingleException se)
            {
                ResultString = se.Message;
            }
        }

        #endregion


        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
