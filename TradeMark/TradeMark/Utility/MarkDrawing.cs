using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeMark.Utility
{
    public class MarkDrawing
    {
        /// <summary>
        /// give mark drawing type information before 2nd nov 2003
        /// </summary>
        /// <param name="markFeatureCode">code of mark feature available in xml</param>
        /// <returns></returns>
        public static string PriorToNovember(string markFeatureCode)
         {
            string markFeature = "";
            switch (markFeatureCode)
                {
                case "1":
                    {
                        markFeature = "1 Typeset: Word(s)/letter(s)/number(s) ";
                        break;
                    }
                case "2":
                    {
                        markFeature = "2 Illustration: Drawing or design without any word(s)/letter(s)/ number(s)";
                        break;
                    }
                case "3":
                    {
                        markFeature = "3 Illustration: Drawing or design which also includes word(s)/ letter(s)/number(s)";
                        break;
                    }
                case "4":
                    {
                        markFeature = "4 Illustration: Drawing with word(s)/letter(s)/number(s) in Block form ";
                        break;
                    }
                case "5":
                    {
                        markFeature = "5 Illustration: Drawing with word(s)/letter(s)/number(s) in Stylized form";
                        break;
                    }
                case "6":
                    {
                        markFeature = "6 Where no drawing is possible, such as for sound";
                        break;
                    }

            }

            return markFeature;
        }
        /// <summary>
        /// Give information about mark drawing type application register after 2nd of nov 2003
        /// </summary>
        /// <param name="markFeatureCode"></param>
        /// <returns></returns>
        public static string AfterToNovember(string markFeatureCode)
        {
            string markFeature = "";
            switch (markFeatureCode)
            {
                case "1":
                    {
                        markFeature = "1 No longer used  ";
                        break;
                    }
                case "2":
                    {
                        markFeature = "2 Illustration: Drawing or design without any word(s)/letter(s)/number(s) ";
                        break;
                    }
                case "3":
                    {
                        markFeature = "3 Illustration: Drawing or design which also includes word(s)/letter(s) / number(s) ";
                        break;
                    }
                case "4":
                    {
                        markFeature = "4 Standard character mark ";
                        break;
                    }
                case "5":
                    {
                        markFeature = "5 Illustration: Drawing with word(s)/letter(s)/number(s) in Stylized form ";
                        break;
                    }
                case "6":
                    {
                        markFeature = "6 Where no drawing is possible, such as for sound ";
                        break;
                    }

            }

            return markFeature;
        }
    }
}