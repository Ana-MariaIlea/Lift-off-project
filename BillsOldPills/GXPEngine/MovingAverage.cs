using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class MovingAverage
    {
        private Queue<float> samples = new Queue<float>();
        private int windowSize = 16;
        private float sampleAccumulator = 0;
        //------------------------------------------------------------------------------------------------------------
        //                                                  Constructor
        //------------------------------------------------------------------------------------------------------------
        public MovingAverage()
        {
        }
        //------------------------------------------------------------------------------------------------------------
        //                                                  Constructor
        //------------------------------------------------------------------------------------------------------------
        public MovingAverage(int windowSize)
        {
            this.windowSize = windowSize;
        }

        public float Average { get; private set; }

        //------------------------------------------------------------------------------------------------------------
        //                                                  ComputeAverage
        //------------------------------------------------------------------------------------------------------------
        public void ComputeAverage(float newSample)
        {
            sampleAccumulator += newSample;
            samples.Enqueue(newSample);

            if (samples.Count > windowSize)
            {
                sampleAccumulator -= samples.Dequeue();
            }

            Average = sampleAccumulator / samples.Count;
        }
    }
}
