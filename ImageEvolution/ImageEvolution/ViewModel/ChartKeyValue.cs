using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.ViewModel
{
    class ChartKeyValue : BindableBase
    {
        private int _Key;
        public int Key { get { return _Key; } set { SetProperty(ref _Key, value); } }

        private double _Value;
        public double Value { get { return _Value; } set { SetProperty(ref _Value, value); } }
    }
}
