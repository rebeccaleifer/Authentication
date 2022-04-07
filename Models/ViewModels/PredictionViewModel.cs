using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models.ViewModels
{
    public class PredictionViewModel
    {
        public PredictorData PredictorData { get; set; }
        public Prediction Prediction { get; set; }
    }
}
