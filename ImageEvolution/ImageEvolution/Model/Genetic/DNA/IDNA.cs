using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.DNA
{
    public interface IDNA
    {
        public void Initialize();
        public IDNA CloneDNA();
    }
}
