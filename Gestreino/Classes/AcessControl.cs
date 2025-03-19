using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Gestreino.Classes
{
    public class AcessControl
    {

        // ACESS CONTROL VARS

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



        public static List<int> ADM_GROUP_EST = new List<int>(new int[] { 21 }); 
        public static List<int> ADM_GROUP_ADM_FUN = new List<int>(new int[] { 6, 140 }); 
        public static List<int> ADM_GROUP_ADM_FUN_DOC = new List<int>(new int[] { 10 });

        //Authentication Claims
        // Fetch grupos
        //var grupoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid").ToList();
        // Fetch subgrupos
        //var subgrupoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid").ToList();
        // Fetch atomos
        //var atomoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();


        // Authorized
        public static bool Authorized(int atom)
        {
            var Authorized = false;

            // Security Claim
            var claimsIdentity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;
            // Atoms
            var atoms = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();

            foreach (var i in atoms)
            {
                if (int.Parse(i.Value) == atom) Authorized = true; 
            }
            return Authorized;
        }
        // Authorized
        public static bool AuthorizedGroupSessionFUN(List<Claim> group)
        {
            var Authorized = false;

            foreach (var i in group)
            {
                if(ADM_GROUP_ADM_FUN.Contains(int.Parse(i.Value))) Authorized = true;
            }

            return Authorized;
        }
        // Authorized
        public static bool AuthorizedGroupSessionEST(List<Claim> group)
        {
            var Authorized = false;

            foreach (var i in group)
            {
                if (ADM_GROUP_EST.Contains(int.Parse(i.Value))) Authorized = true;
            }

            return Authorized;
        }
        // Authorized
        public static bool AuthorizedGroupSessionDOC(List<Claim> group)
        {
            var Authorized = false;

            foreach (var i in group)
            {
                if (ADM_GROUP_ADM_FUN_DOC.Contains(int.Parse(i.Value))) Authorized = true;
            }

            return Authorized;
        }
        // Counter
        public static int CountSubGroupAuthorized(ClaimsIdentity claimsIdentity)
        {
            var t = 0;
            var subgrupoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid").ToList();

            List<string> s = new List<string>();

            foreach (var i in subgrupoClaim)  { s.Add(i.Value); }

            t=s.Count();

            return t;
        }
    }
}