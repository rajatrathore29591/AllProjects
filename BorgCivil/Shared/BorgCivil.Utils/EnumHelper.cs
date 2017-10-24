using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Utils
{
    public class KeyValuePair
    {
        public string Key { get; set; }

        public int Value { get; set; }

        public static List<KeyValuePair> ListFrom<T>()
        {
            var array = (T[])(System.Enum.GetValues(typeof(T)).Cast<T>());
            return array
              .Select(a => new KeyValuePair
              {
                  Key = a.ToString(),
                  Value = Convert.ToInt32(a)
              })
                .OrderBy(kvp => kvp.Key)
               .ToList();
        }
    }
}
