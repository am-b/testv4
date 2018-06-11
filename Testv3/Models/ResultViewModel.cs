using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testv3.Models
{
    public class ResultViewModel
    {
        //psych test 
        public int countAgree { get; set; }
        public int countSomewhatAgree { get; set; }
        public int countDisagree { get; set; }
        public string questionItem { get; set; }
        public int total { get; set; }

        //student inventory
        public int countMale { get; set; }
        public int countFemale { get; set; }
        public int countScholar{ get; set; }
        public int countNotScholar { get; set; }
        public int countTotalPersonalChoice { get; set; }
        public int countTotalNotPersonalChoice { get; set; }

        //new vm
        public string WhyMMCC { get; set; }
        public int countWhyMMCC { get; set; }

        public string HowMMCC { get; set; }
        public int countHowMMCC { get; set; }

        public string WhoInfluenced { get; set; }
        public int countWhoInfluenced { get; set; }

    }
}