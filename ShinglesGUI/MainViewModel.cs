using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Shingles.Annotations;

namespace Shingles
{
    public class MainViewModel:INotifyPropertyChanged
    {
        #region Приватные поля

        private readonly IShingleResolver _resolver;

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
                    OnPropertyChanged();                                        
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
                    OnPropertyChanged();
                }
            }            
        }

        #endregion

        #region Команды

        public ICommand CalculateCommand { get; private set; }

        #endregion

        #endregion


        #region Конструктор

        public MainViewModel(IShingleResolver resolver)
        {
            _resolver = resolver;

            CalculateCommand = new DelegateCommand(calculate);

            ShingleSize = 10;
        }

        #endregion


        #region Обработчики

        private void calculate()
        {
            try
            {
                Result = _resolver.Calculate(FirstText, SecondText, ShingleSize);

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
