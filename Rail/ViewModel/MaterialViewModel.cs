using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.ViewModel
{
    public class MaterialViewModel : BaseViewModel
    {
        public int Number { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
    }
}
