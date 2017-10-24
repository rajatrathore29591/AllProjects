using System;

namespace BorgCivil.Utils.Models
{
    public class DemoModel
    {
        
        public Guid DemoId { get; set; }
      
        public string Name { get; set; }
        
        public string Address { get; set; }

        public DateTime? CurrentDate { get; set; }
        
        public string RadioGender { get; set; }
       
        public string CheckBoxGender { get; set; }

        public string DropDownGender { get; set; }


    }
}