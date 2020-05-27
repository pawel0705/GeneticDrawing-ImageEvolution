using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.ViewModel
{
    class ChartData : BindableBase
    {
        private List<ChartKeyValue> _DataList;
        public List<ChartKeyValue> DataList { get { return _DataList; } set { SetProperty(ref _DataList, value); } }
    }
}
