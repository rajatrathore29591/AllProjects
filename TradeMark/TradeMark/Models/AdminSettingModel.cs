using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMark.Models
{
  public class AdminSettingModel
    {
        public bool StatusFlag { get; set; }
        public string LatestFileImportDate { get; set; }

        public string AdminEmail { get; set; }


    }
}
