using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMark.Models
{
   public static  class ScoringColoringModel
    {
        public const string Group1 = "Group1";
        public const string Group2 = "Group2";
        public const string Group3 = "Group3";
        public const string Group4 = "Group4";

        public const string Red = "Red";
        public const string Yellow = "Yellow";
               
        public const string Green = "Green";

        public const string RedLightMsg = "<div class='redLightMsg alert alert-danger'><b>WARNING!</b><br> The preliminary search revealed one or more marks that may prevent the registration of your proposed mark.You may want to choose a new mark or consult with a trademark attorney about how you may be able to overcome a refusal to register your proposed mark from the United States Patent and Trademark Office based on any one of the marks identified in this preliminary search.</div>";
        public const string YellowLightMsg = "<div  style='line-height: 2rem; padding: 15px; margin-bottom: 20px; border: 1px solid transparent; border-radius: 4px; color: #8a6d3b;background-color: #fff1a7;border-color: #faebcc;'><b>CAUTION!</b><br> The preliminary search revealed that either a crowded field may exist for your proposed mark or that the marks identified in this preliminary search present a moderate risk to preventing the registration of your proposed mark.If you cannot tolerate some risk with respect to the registration of your proposed mark, you may want to choose a new mark, or consult with a trademark attorney about how you may be able to overcome a refusal from the United States Patent and Trademark Office to register your proposed mark based on any one of the marks identified in this preliminary search. If you decide to move forward with a trademark application for your proposed mark, you should consider commissioning a comprehensive trademark search before filing the application with the United States Patent and Trademark Office.A trademark attorney can help you identify a reputable comprehensive search company. </div>";
        public const string GreenLightMsg = "<div class='successmsg alert alert-success'><b>CONGRATULATIONS!</b><br> The preliminary search did not reveal an obvious threat to the registration of your proposed mark.If you decide to move forward with a trademark application for your proposed mark, you should consider commissioning a comprehensive trademark search before filing the application with the United States Patent and Trademark Office.A trademark attorney can help you identify a reputable comprehensive search company.</div>";

        public const string grpscoreA1 = "a 1";

        public const string grpscoreA2 = "a 2";
        public const string grpscoreA3 = "a 3";
        public const string grpscoreA4 = "a 4";
        public const string grpscoreA5 = "a 5";


        // below are the static part for pdf generation

        public const string pdfRedLightHeadingMsg = "WARNING!\n";
        public const string pdfYellowLightHeadingMsg = "CAUTION!\n";
        public const string pdfGreenLightHeadingMsg = "CONGRATULATIONS!\n";
        public const string pdfRedLightMsg = "The preliminary search revealed one or more marks that may prevent the registration of your proposed mark.You may want to choose a new mark or consult with a trademark attorney about how you may be able to overcome a refusal to register your proposed mark from the United States Patent and Trademark Office based on any one of the marks identified in this preliminary search. \n";
        public const string pdfYellowLightMsg = "The preliminary search revealed that either a crowded field may exist for your proposed mark or that the marks identified in this preliminary search present a moderate risk to preventing the registration of your proposed mark.If you cannot tolerate some risk with respect to the registration of your proposed mark, you may want to choose a new mark, or consult with a trademark attorney about how you may be able to overcome a refusal from the United States Patent and Trademark Office to register your proposed mark based on any one of the marks identified in this preliminary search.\n If you decide to move forward with a trademark application for your proposed mark, you should consider commissioning a comprehensive trademark search before filing the application with the United States Patent and Trademark Office.A trademark attorney can help you identify a reputable comprehensive search company. \n";
        public const string pdfGreenLightMsg = " The preliminary search did not reveal an obvious threat to the registration of your proposed mark.If you decide to move forward with a trademark application for your proposed mark, you should consider commissioning a comprehensive trademark search before filing the application with the United States Patent and Trademark Office.A trademark attorney can help you identify a reputable comprehensive search company. \n";
        public const string pdfFontSet = "gothic";
        public const string pdfFirstCellHeading = "Serial Number";
        public const string pdfSecondCellHeading = "Registration Number";
        public const string pdfThirdCellHeading = "Word Mark";
        public const string pdfOwnerCellHeading = "Owner";
        public const string pdfGoodsServicesCellHeading = "Goods or Services Description";

        public const string talkToAttorneyHelp = "One of BOB’s guests has requested: a trademark attorney’s opinion of the attached preliminary trademark search results.";
        public const string talkToAttorneyAssistance = "One of BOB’s guests has requested: assistance with the filing of a trademark application.";
        public const string talkToAttorneyBoth = "One of BOB’s guests has requested: a trademark attorney’s opinion of the attached preliminary trademark search results and assistance with the filing of a trademark application.";
        public const string  talktoAttorneyBody= "<br/>Please contact BOB’s guest as soon as possible, but in no event more than 24 hours of this e-mail, to assist with the request. The contact information for BOB’s guest is included in the attached search report.";
        public const string talktoAttorneyThanks= "<br/>Thanks for working with BOB on this important legal matter for BOB’s guest!<br/><br/>Very truly yours,<br/><br/>BOB";


    }
}
