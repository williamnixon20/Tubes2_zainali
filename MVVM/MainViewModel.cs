using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubes2_zainali.Core;

namespace Tubes2_zainali.MVVM
{
    class MainViewModel: ObservableObject
    {
        public GridViewModel GridVM { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            GridVM = new GridViewModel();
            CurrentView = GridVM;
        }
    }
}
