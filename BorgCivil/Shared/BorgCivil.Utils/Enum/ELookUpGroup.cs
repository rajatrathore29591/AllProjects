using System;
using System.Collections.Generic;

namespace BorgCivil.Utils.Enum
{
    public enum ELookUpGroup
    {
        Status = 1,
        Pending = 2,
        Allocated = 3,  
        Completed = 4,
        Cancelled = 5
    }

    public static class ETaskStatusHelper
    {
        public static Dictionary<int, string> EtaskStatusList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "Assign");
            return dic;
        }

        ///
        /// Method to get enumeration value from int value.
        ///
        ///
        ///

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])System.Enum.GetValues(typeof(T)))[0];

            foreach (T enumValue in (T[])System.Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }
            }
            return val;
        }

      
    }

}
