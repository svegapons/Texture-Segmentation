using System;
using System.Collections.Generic;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioApplication
{
    delegate TxMatrix MatrixProccessing(TxMatrix matrix);

    class DescriptorsHandler
    {
        public DescriptorsHandler(GeneralDescriptorSequence sequence, Environment env)
        {
            this.env = env;
            this.sequence = sequence;
            sequence.Reset();
        }

        public DescriptorsHandler(GeneralDescriptorSequence sequence, Environment env, MatrixProccessing featuresPostProcessing)
            : this(sequence, env)
        {
            this.featuresPostProcessing = featuresPostProcessing;
        }

        GeneralDescriptorSequence sequence;
        Environment env;
        MatrixProccessing featuresPostProcessing;


        public void Reset()
        {
            sequence.Reset();
        }

        public bool MoveNext()
        {
            return sequence.MoveNext();
        }

        public string GetCurrentAbbreviation()
        {
            return AbbreviationAttribute.GetFullAbbreviation((TxAlgorithm)sequence.Current);
        }

        public TxMatrix GetCurrentDescription(TxImage image)
        {
            TxMatrix descriptor;
            if (Properties.Settings.Default.UseDatabase)
            {
                descriptor = env.GetDescriptorApplication(sequence.Current, image);
                if (descriptor == null)
                {
                    descriptor = sequence.Current.GetDescription(image);
                    env.AddDescriptorApplication(sequence.Current, image, descriptor);
                }
            }
            else
                descriptor = sequence.Current.GetDescription(image);

            if (featuresPostProcessing != null)
                return featuresPostProcessing(descriptor);
            return descriptor;
        }
    }
}
