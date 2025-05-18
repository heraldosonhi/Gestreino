using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Gestreino.Classes
{
    public class AcessControl
    {
        public static int ADM_USERS_ATOMS_LIST_VIEW_SEARCH = 1;
        public static int ADM_USERS_ATOMS_NEW = 2;
        public static int ADM_USERS_ATOMS_EDIT = 3;
        public static int ADM_USERS_ATOMS_DELETE = 4;

        public static int ADM_USERS_PROFILES_LIST_VIEW_SEARCH = 5;
        public static int ADM_USERS_PROFILES_NEW = 6;
        public static int ADM_USERS_PROFILES_EDIT = 7;
        public static int ADM_USERS_PROFILES_DELETE = 8;

        public static int ADM_USERS_GROUPS_LIST_VIEW_SEARCH = 9;
        public static int ADM_USERS_GROUPS_NEW = 10;
        public static int ADM_USERS_GROUPS_EDIT = 11;
        public static int ADM_USERS_GROUPS_DELETE = 12;

        public static int ADM_USERS_ATOMS_GROUPS_LIST_VIEW_SEARCH = 13;
        public static int ADM_USERS_ATOMS_GROUPS_NEW = 14;
        public static int ADM_USERS_ATOMS_GROUPS_DELETE = 15;

        public static int ADM_USERS_ATOMS_PROFILES_LIST_VIEW_SEARCH = 16;
        public static int ADM_USERS_ATOMS_PROFILES_NEW = 17;
        public static int ADM_USERS_ATOMS_PROFILES_DELETE = 18;

        public static int ADM_USERS_PROFILE_USERS_LIST_VIEW_SEARCH = 19;
        public static int ADM_USERS_PROFILE_USERS_NEW = 20;
        public static int ADM_USERS_PROFILE_USERS_DELETE = 21;

        public static int ADM_USERS_GROUP_USERS_LIST_VIEW_SEARCH = 22;
        public static int ADM_USERS_GROUP_USERS_NEW = 23;
        public static int ADM_USERS_GROUP_USERS_DELETE = 24;

        public static int ADM_USERS_USERS_LIST_VIEW_SEARCH = 25;
        public static int ADM_USERS_USERS_NEW = 26;
        public static int ADM_USERS_USERS_EDIT = 27;
        public static int ADM_USERS_USERS_ALTER_PASSWORD = 28;
        public static int ADM_USERS_USERS_CLEAR_PWD_ATTEMPT = 29;
        public static int ADM_USERS_LOGIN_LOGS_LIST_VIEW_SEARCH = 30;

        public static int ADM_SEC_TOKENS_LIST_VIEW_SEARCH = 31;

        public static int ADM_CONFIG_INST_EDIT = 32;
        public static int ADM_CONFIG_SETINGS_EDIT = 33;
        public static int ADM_CONFIG_FILEMGR = 34;

        public static int ADM_CONFIG_PARAM_PES = 35;
        public static int ADM_CONFIG_PARAM_ADM = 36;
        public static int ADM_CONFIG_PARAM_GT = 37;

        public static int GT_ATHLETES_LIST_VIEW_SEARCH = 38;
        public static int GT_ATHLETES_NEW = 39;
        public static int GT_ATHLETES_EDIT = 40;
        public static int GT_ATHLETES_ALTER_IMG = 41;
        public static int GT_ATHLETES_FILEMGR = 42;

        public static int GT_ATHLETES_IDENTIFICATION_LIST_VIEW_SEARCH = 43;
        public static int GT_ATHLETES_IDENTIFICATION_NEW = 44;
        public static int GT_ATHLETES_IDENTIFICATION_EDIT = 45;
        public static int GT_ATHLETES_IDENTIFICATION_DELETE = 46;

        public static int GT_ATHLETES_PROFESSIONAL_LIST_VIEW_SEARCH = 47;
        public static int GT_ATHLETES_PROFESSIONAL_NEW = 48;
        public static int GT_ATHLETES_PROFESSIONAL_EDIT = 49;
        public static int GT_ATHLETES_PROFESSIONAL_DELETE = 50;

        public static int GT_ATHLETES_FAM_LIST_VIEW_SEARCH = 51;
        public static int GT_ATHLETES_FAM_NEW = 52;
        public static int GT_ATHLETES_FAM_EDIT = 53;
        public static int GT_ATHLETES_FAM_DELETE = 54;

        public static int GT_ATHLETES_DEFICIENCY_LIST_VIEW_SEARCH = 55;
        public static int GT_ATHLETES_DEFICIENCY_NEW = 56;
        public static int GT_ATHLETES_DEFICIENCY_EDIT = 57;
        public static int GT_ATHLETES_DEFICIENCY_DELETE = 58;

        public static int GT_PLANS_BODYMASS_LIST_VIEW_SEARCH = 59;
        public static int GT_PLANS_BODYMASS_UPDATE = 60;
        public static int GT_PLANS_BODYMASS_DELETE = 61;

        public static int GT_PLANS_CARDIO_LIST_VIEW_SEARCH = 62;
        public static int GT_PLANS_CARDIO_UPDATE = 63;
        public static int GT_PLANS_CARDIO_DELETE = 64;

        public static int GT_QUEST_ANXIETY_LIST_VIEW_SEARCH = 65;
        public static int GT_QUEST_ANXIETY_UPDATE = 66;
        public static int GT_QUEST_ANXIETY_DELETE = 67;

        public static int GT_QUEST_SELFCONCEPT_LIST_VIEW_SEARCH = 68;
        public static int GT_QUEST_SELFCONCEPT_UPDATE = 69;
        public static int GT_QUEST_SELFCONCEPT_DELETE = 70;

        public static int GT_QUEST_CORONARYRISK_LIST_VIEW_SEARCH = 71;
        public static int GT_QUEST_CORONARYRISK_UPDATE = 72;
        public static int GT_QUEST_CORONARYRISK_DELETE = 73;

        public static int GT_QUEST_HEALTH_LIST_VIEW_SEARCH = 74;
        public static int GT_QUEST_HEALTH_UPDATE = 75;
        public static int GT_QUEST_HEALTH_DELETE = 76;

        public static int GT_QUEST_FLEXIBILITY_LIST_VIEW_SEARCH = 77;
        public static int GT_QUEST_FLEXIBILITY_UPDATE = 78;
        public static int GT_QUEST_FLEXIBILITY_DELETE = 79;

        public static int GT_QUEST_BODYCOMPOSITION_LIST_VIEW_SEARCH = 80;
        public static int GT_QUEST_BODYCOMPOSITION_UPDATE = 81;
        public static int GT_QUEST_BODYCOMPOSITION_DELETE = 82;

        public static int GT_QUEST_CARDIO_LIST_VIEW_SEARCH = 83;
        public static int GT_QUEST_CARDIO_UPDATE = 84;
        public static int GT_QUEST_CARDIO_DELETE = 85;

        public static int GT_QUEST_ELDERLY_LIST_VIEW_SEARCH = 86;
        public static int GT_QUEST_ELDERLY_UPDATE = 87;
        public static int GT_QUEST_ELDERLY_DELETE = 88;

        public static int GT_QUEST_FORCE_LIST_VIEW_SEARCH = 89;
        public static int GT_QUEST_FORCE_UPDATE = 90;
        public static int GT_QUEST_FORCE_DELETE = 91;

        public static int GT_QUEST_FUNCTIONAL_LIST_VIEW_SEARCH = 92;
        public static int GT_QUEST_FUNCTIONAL_UPDATE = 93;
        public static int GT_QUEST_FUNCTIONAL_DELETE = 94;

        public static int GT_SEARCH_PRESCRIPTIONS_LIST_VIEW_SEARCH = 95;
        public static int GT_SEARCH_EVALUATIONS_LIST_VIEW_SEARCH = 96;
        public static int GT_SEARCH_MEDIUMWEIGHT_LIST_VIEW_SEARCH = 97;
        public static int GT_SEARCH_ANALYSIS_LIST_VIEW_SEARCH = 98;
        public static int GT_SEARCH_RANKING_LIST_VIEW_SEARCH = 99;
        public static int GT_SEARCH_OTHERS_LIST_VIEW_SEARCH = 100;

        public static int GT_REPORTS_LIST_VIEW_SEARCH = 101;

        public static List<int> ADM_GROUP_EST = new List<int>(new int[] { 21 }); 
        public static List<int> ADM_GROUP_ADM_FUN = new List<int>(new int[] { 6, 140 }); 
        public static List<int> ADM_GROUP_ADM_FUN_DOC = new List<int>(new int[] { 10 });

        public static bool Authorized(int atom)
        {
            var Authorized = false;

            var claimsIdentity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;

            var atoms = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();

            foreach (var i in atoms)
            {
                if (int.Parse(i.Value) == atom) Authorized = true; 
            }
            return Authorized;
        }
    }
}