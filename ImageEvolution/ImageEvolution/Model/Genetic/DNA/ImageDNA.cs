using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.DNA
{
    public class ImageDNA : IDNA
    {
        public List<ShapeDNA> Shapes { get; set; }

        public void Initialize()
        {
            Shapes = new List<ShapeDNA>();

            for(int i = 0; i < 50; i++)
            {
                AddShape();
            }
        }

        public IDNA CloneDNA()
        {
            var imageDNA = new ImageDNA();
            imageDNA.Shapes = new List<ShapeDNA>();

            foreach(var shape in Shapes)
            {
                imageDNA.Shapes.Add((ShapeDNA)shape.CloneDNA());
            }

            return imageDNA;
        }

        public void AddShape()
        {
            if(Shapes.Count < 50)
            {
                var shape = new ShapeDNA();
                shape.Initialize();

                Shapes.Add(shape);
            }
        }
    }
}
