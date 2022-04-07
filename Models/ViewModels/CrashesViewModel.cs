using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models.ViewModels
{
    public class CrashesViewModel
    {
        public IList<Crash> Crashes { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
