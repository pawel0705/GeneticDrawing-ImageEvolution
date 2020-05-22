using ImageEvolution.Model.Genetic.Evolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Utils
{
    public class IndividualEventArgs : EventArgs
    {
        public Individual Individual { get; set; }
    }
}
