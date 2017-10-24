using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeMark.DAL;
using TradeMark.Models;

namespace TradeMark.BAL
{
    public class USClassService
    {
        USClassProvider oProvider = new USClassProvider();

        /// <summary>
        /// Add US class description 
        /// </summary>
        /// <param name="oUSClass"></param>
        /// <returns></returns>
        public bool AddUSClass(USClassModel oUSClass)
        {
            return oProvider.AddUSClass(oUSClass);
        }

        /// <summary>
        /// Check it's present into USCLassDescription table
        /// </summary>
        /// <param name="usClassId"></param>
        /// <returns></returns>
        public bool CheckUSClassDescription(string usClassId)
        {
            USClassProvider oProvider = new USClassProvider();
            bool usClassDescriptionAvailability = oProvider.CheckUSClassDescriptionAvailability(usClassId);
            if (usClassDescriptionAvailability == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}