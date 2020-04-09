using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.DNA
{
    public interface IDNA
    {
        public void InitializeDNA();

        public void SoftMutation();
        public void MediumMutation();
        public void HardMutation();
        public void GaussianMutation();

        public IDNA CloneDNA();
    }
}
