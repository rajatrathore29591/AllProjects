using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iCanSpeakAdminPortal.Models
{
    public class DashBoardModel
    {
        public int userCount { get; set; }
        public int activeUserCount { get; set; }
        public int inactiveUserCount { get; set; }

        public int totalCourseCount { get; set; }
        public int activeCourseCount { get; set; }
        public int inactiveCourseCount { get; set; }
        public int totalCoursePaidCount { get; set; }
        public int totalCourseFreeCount { get; set; }

        public int totalPaymentCount { get; set; }
        public int totalPaidAmount { get; set; }
        public int totaltutorcount { get; set; }
        public int activetutorCount { get; set; }
        public int inactivetutorCount { get; set; }


        public int totalVocabCount  { get; set; }
        public int activeVocabCount { get; set; }
        public int inactiveVocabCount { get; set; }
        public int totalVocabPaidCount { get; set; }
        public int totalVocabFreeCount { get; set; }

        public int totalVocabSubCount { get; set; }
        public int activeVocabSubCount { get; set; }
        public int inactiveVocabSubCount { get; set; }

        public int totalWordCount { get; set; }
        public int activeWordCount { get; set; }
        public int inactiveWordCount { get; set; }

        public int totalVocabQueCount { get; set; }
        public int activeVocabQueCount { get; set; }
        public int inactiveVocabQueCount { get; set; }

        public int totalGrammerCount { get; set; }
        public int activeGrammerCount { get; set; }
        public int inactiveGrammerCount { get; set; }
        public int totalGrammerPaidCount { get; set; }
        public int totalGrammerFreeCount { get; set; }

        public int totalGrammerQueCount { get; set; }
        public int activeGrammerQueCount { get; set; }
        public int inactiveGrammerQueCount { get; set; }


        public int totalDialogCount { get; set; }
        public int activeDialogCount { get; set; }
        public int inactiveDialogCount { get; set; }
        public int totalDialogPaidCount { get; set; }
        public int totalDialogFreeCount { get; set; }

        public int totalKeyPhrasesCount { get; set; }
        public int activeKeyPhrasesCount { get; set; }
        public int inactiveKeyPhrasesCount { get; set; }

        public int totalConversationCount { get; set; }
        public int activeConversationCount { get; set; }
        public int inactiveConversationCount { get; set; }

        public int totalDialogAssiQueCount { get; set; }
        public int activeDialogAssiQueCount { get; set; }
        public int inactiveDialogAssiQueCount { get; set; }
    }
}