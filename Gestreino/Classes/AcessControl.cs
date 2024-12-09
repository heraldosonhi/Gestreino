using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Gestreino.Classes
{
    public class AcessControl
    {

        // ACESS CONTROL VARS
        public static int ADM_INST_INSTITUTIONS_LIST_VIEW_SEARCH = 1;
        public static int ADM_INST_INSTITUTIONS_NEW = 2;
        public static int ADM_INST_INSTITUTIONS_EDIT = 3;
        public static int ADM_INST_INSTITUTIONS_DELETE = 4;
        public static int ADM_INST_INSTITUTIONS_FILEMGR = 5;

        public static int ADM_INST_LEVELS_LIST_VIEW_SEARCH = 6;
        public static int ADM_INST_LEVELS_NEW = 7;
        public static int ADM_INST_LEVELS_EDIT = 8;
        public static int ADM_INST_LEVELS_DELETE = 9;

        public static int ADM_INST_UNITS_LIST_VIEW_SEARCH = 10;
        public static int ADM_INST_UNITS_NEW = 11;
        public static int ADM_INST_UNITS_EDIT = 12;
        public static int ADM_INST_UNITS_DELETE = 13;

        public static int ADM_INST_APPLICATIONS_LIST_VIEW_SEARCH = 14;
        public static int ADM_INST_APPLICATIONS_NEW = 15;
        public static int ADM_INST_APPLICATIONS_EDIT = 16;
        public static int ADM_INST_APPLICATIONS_DELETE = 17;

        public static int ADM_INST_MODULES_LIST_VIEW_SEARCH = 18;
        public static int ADM_INST_MODULES_NEW = 19;
        public static int ADM_INST_MODULES_EDIT = 20;
        public static int ADM_INST_MODULES_DELETE = 21;

        public static int ADM_INST_CAMPUS_LIST_VIEW_SEARCH = 22;
        public static int ADM_INST_CAMPUS_NEW = 23;
        public static int ADM_INST_CAMPUS_EDIT = 24;
        public static int ADM_INST_CAMPUS_DELETE = 25;
        public static int ADM_INST_CAMPUS_FILEMGR = 26;

        public static int ADM_INST_BUILDINGS_LIST_VIEW_SEARCH = 27;
        public static int ADM_INST_BUILDINGS_NEW = 28;
        public static int ADM_INST_BUILDINGS_EDIT = 29;
        public static int ADM_INST_BUILDINGS_DELETE = 30;
        public static int ADM_INST_BUILDINGS_FILEMGR = 35;

        public static int ADM_INST_FLOORS_LIST_VIEW_SEARCH = 31;
        public static int ADM_INST_FLOORS_NEW = 32;
        public static int ADM_INST_FLOORS_EDIT = 33;
        public static int ADM_INST_FLOORS_DELETE = 34;
        public static int ADM_INST_FLOORS_FILEMGR = 36;

        public static int ADM_INST_SPACES_LIST_VIEW_SEARCH = 37;
        public static int ADM_INST_SPACES_NEW = 38;
        public static int ADM_INST_SPACES_EDIT = 39;
        public static int ADM_INST_SPACES_DELETE = 40;
        public static int ADM_INST_SPACES_FILEMGR = 41;

        public static int ADM_INST_SPACES_UTILIZATION_LIST_VIEW_SEARCH = 42;
        public static int ADM_INST_SPACES_UTILIZATION_NEW = 43;
        public static int ADM_INST_SPACES_UTILIZATION_EDIT = 44;
        public static int ADM_INST_SPACES_UTILIZATION_DELETE = 45;

        public static int ADM_INST_SPACES_CHARACTERISTICS_LIST_VIEW_SEARCH = 46;
        public static int ADM_INST_SPACES_CHARACTERISTICS_NEW = 47;
        public static int ADM_INST_SPACES_CHARACTERISTICS_EDIT = 48;
        public static int ADM_INST_SPACES_CHARACTERISTICS_DELETE = 49;

        public static int ADM_INST_SPACES_UTILIZATION_ENTITY_LIST_VIEW_SEARCH = 50;
        public static int ADM_INST_SPACES_UTILIZATION_ENTITY_NEW = 51;
        public static int ADM_INST_SPACES_UTILIZATION_ENTITY_EDIT = 52;
        public static int ADM_INST_SPACES_UTILIZATION_ENTITY_DELETE = 53;

        public static int ADM_USERS_ATOMS_LIST_VIEW_SEARCH = 54;
        public static int ADM_USERS_ATOMS_NEW = 55;
        public static int ADM_USERS_ATOMS_EDIT = 56;
        public static int ADM_USERS_ATOMS_DELETE = 57;

        public static int ADM_USERS_PROFILES_LIST_VIEW_SEARCH = 58;
        public static int ADM_USERS_PROFILES_NEW = 59;
        public static int ADM_USERS_PROFILES_EDIT = 60;
        public static int ADM_USERS_PROFILES_DELETE = 61;

        public static int ADM_USERS_GROUPS_LIST_VIEW_SEARCH = 62;
        public static int ADM_USERS_GROUPS_NEW = 63;
        public static int ADM_USERS_GROUPS_EDIT = 64;
        public static int ADM_USERS_GROUPS_DELETE = 65;

        public static int ADM_USERS_SUBGROUPS_LIST_VIEW_SEARCH = 66;
        public static int ADM_USERS_SUBGROUPS_NEW = 67;
        public static int ADM_USERS_SUBGROUPS_EDIT = 68;
        public static int ADM_USERS_SUBGROUPS_DELETE = 69;

        public static int ADM_USERS_ATOMS_GROUPS_LIST_VIEW_SEARCH = 70;
        public static int ADM_USERS_ATOMS_GROUPS_NEW = 71;
        public static int ADM_USERS_ATOMS_GROUPS_DELETE = 72;

        public static int ADM_USERS_ATOMS_PROFILES_LIST_VIEW_SEARCH = 73;
        public static int ADM_USERS_ATOMS_PROFILES_NEW = 74;
        public static int ADM_USERS_ATOMS_PROFILES_DELETE = 75;

        public static int ADM_USERS_PROFILE_USERS_LIST_VIEW_SEARCH = 76;
        public static int ADM_USERS_PROFILE_USERS_NEW = 77;
        public static int ADM_USERS_PROFILE_USERS_DELETE = 78;

        public static int ADM_USERS_PROFILE_PROFILE_LIST_VIEW_SEARCH = 79;
        public static int ADM_USERS_PROFILE_PROFILE_NEW = 80;
        public static int ADM_USERS_PROFILE_PROFILE_DELETE = 81;

        public static int ADM_USERS_GROUP_USERS_LIST_VIEW_SEARCH = 82;
        public static int ADM_USERS_GROUP_USERS_NEW = 83;
        public static int ADM_USERS_GROUP_USERS_EDIT = 84;
        public static int ADM_USERS_GROUP_USERS_DELETE = 85;

        public static int ADM_USERS_USERS_LIST_VIEW_SEARCH = 86;
        public static int ADM_USERS_USERS_NEW = 87;
        public static int ADM_USERS_USERS_EDIT = 88;
        public static int ADM_USERS_USERS_ALTER_PASSWORD = 89;
        public static int ADM_USERS_USERS_CLEAR_PWD_ATTEMPT = 90;
        public static int ADM_USERS_LOGIN_LOGS_LIST_VIEW_SEARCH = 91;

        public static int ADM_USERS_ACCESS_CONTROL = 120;
        
        public static int ADM_SEC_TOKENS_LIST_VIEW_SEARCH = 92;
        public static int ADM_SEC_TOKENS_NEW = 93;
        public static int ADM_SEC_TOKENS_EDIT = 94;
        public static int ADM_SEC_TOKENS_DELETE = 95;

        public static int ADM_CONFIG_INST_EDIT = 96;
        public static int ADM_CONFIG_NETWORK_EDIT = 97;
        public static int ADM_CONFIG_SECURITY_EDIT = 98;
        public static int ADM_CONFIG_API_EDIT = 99;

        public static int ADM_CONFIG_PARAM_SEC = 285;
        public static int ADM_CONFIG_PARAM_ADM = 286;
        public static int ADM_CONFIG_PARAM_GA = 287;
        public static int ADM_CONFIG_PARAM_GP = 288;
        public static int ADM_CONFIG_PARAM_GPAG = 289;

        public static int ADM_CONFIG_MESSAGE_CENTER_SMS_LIST_VIEW_SEARCH = 254;
        public static int ADM_CONFIG_MESSAGE_CENTER_SMS_NEW = 269;
        public static int ADM_CONFIG_MESSAGE_CENTER_CONFIGURATION = 255;
        

        public static int GA_COURSES_LIST_VIEW_SEARCH = 100;
        public static int GA_COURSES_NEW = 101;
        public static int GA_COURSES_EDIT = 102;
        public static int GA_COURSES_DELETE = 103;
        public static int GA_COURSES_FILEMGR = 232;

        public static int GA_PROCESS_LIST_VIEW_SEARCH = 104;
        public static int GA_PROCESS_NEW = 105;
        public static int GA_PROCESS_EDIT = 106;
        public static int GA_PROCESS_DELETE = 107;
        public static int GA_PROCESS_FILEMGR = 108;
        public static int GA_PROCESS_COURSES_CLASS_PARAMETERS = 109;
        public static int GA_PROCESS_COURSES_CLASS_PARAMETERS_NEW = 149;
        public static int GA_PROCESS_COURSES_CLASS_PARAMETERS_EDIT = 150;
        public static int GA_PROCESS_COURSES_CLASS_PARAMETERS_DELETE = 375;
        public static int GA_PROCESS_COURSES_NEW = 110;
        public static int GA_PROCESS_COURSES_EDIT = 111;
        public static int GA_PROCESS_COURSES_DELETE = 112;

        public static int GA_APPLICATIONS_LIST_VIEW_SEARCH = 113;
        public static int GA_APPLICATIONS_EDIT = 114;
        public static int GA_APPLICATIONS_EDIT_RESULT = 270;
        public static int GA_APPLICATIONS_EDIT_EXCEPTION = 271;
        public static int GA_APPLICATIONS_COURSE_CHANGE = 115;
        public static int GA_APPLICATIONS_TRANSFER_TO_SIGARRA = 116;
        public static int GA_APPLICATIONS_CANCEL = 117;
        public static int GA_APPLICATIONS_PDF_REPORT = 118;
        public static int GA_APPLICATIONS_FILEMGR = 119;
        public static int GA_APPLICATIONS_DELETE = 233;
        public static int GA_APPLICATIONS_ENROL_STUDENTS = 234;
        public static int GA_APPLICATIONS_ENROL_STUDENTS_ALTER_CLASS = 243;

        public static int GA_RAMOS_LIST_VIEW_SEARCH = 151;
        public static int GA_RAMOS_NEW = 152;
        public static int GA_RAMOS_EDIT = 153;
        public static int GA_RAMOS_DELETE = 154;

        public static int GA_SUBJECTS_LIST_VIEW_SEARCH = 155;
        public static int GA_SUBJECTS_NEW = 156;
        public static int GA_SUBJECTS_EDIT = 157;
        public static int GA_SUBJECTS_DELETE = 158;

        public static int GA_PROCESS_LRES_NEW = 159;
        public static int GA_PROCESS_LRES_LIST_VIEW_SEARCH = 160;
        public static int GA_PROCESS_LRES_EDIT = 161;
        public static int GA_PROCESS_LRES_SUBMIT = 162;
        public static int GA_PROCESS_LRES_PUBLISH = 163;
        public static int GA_PROCESS_LRES_EVENTS = 164;

        public static int GA_STUDY_PLANS_LIST_VIEW_SEARCH = 165;
        public static int GA_STUDY_PLANS_NEW = 166;
        public static int GA_STUDY_PLANS_EDIT = 167;
        public static int GA_STUDY_PLANS_DELETE = 168;
        public static int GA_STUDY_PLANS_MANAGE = 169;
        public static int GA_STUDY_PLANS_FILEMGR = 263;

        public static int GA_CLASSES_LIST_VIEW_SEARCH = 170;
        public static int GA_CLASSES_NEW = 171;
        public static int GA_CLASSES_EDIT = 172;
        public static int GA_CLASSES_DELETE = 173;
        public static int GA_CLASSES_GROUPS_NEW = 174;
        public static int GA_CLASSES_GROUPS_EDIT = 175;
        public static int GA_CLASSES_GROUPS_DELETE = 176;
        public static int GA_CLASSES_CLONE = 177;
        public static int GA_CLASSES_SUBJECTS_MANAGE = 178;

        public static int GA_CALENDAR_LIST_VIEW_SEARCH = 191;
        public static int GA_CALENDAR_NEW = 192;
        public static int GA_CALENDAR_EDIT = 193;
        public static int GA_CALENDAR_DELETE = 194;
        public static int GA_CALENDAR_PROCESS_LESSONS = 195;

        public static int GA_CALENDAR_ACTIVITY_LIST_VIEW_SEARCH = 239;
        public static int GA_CALENDAR_ACTIVITY_NEW = 240;
        public static int GA_CALENDAR_ACTIVITY_EDIT = 241;
        public static int GA_CALENDAR_ACTIVITY_DELETE = 242;

        public static int GA_OCCURRENCE_PLANS_LIST_VIEW_SEARCH = 196;
        public static int GA_OCCURRENCE_PLANS_NEW = 197;
        public static int GA_OCCURRENCE_PLANS_EDIT = 198;
        public static int GA_OCCURRENCE_PLANS_DELETE = 199;
        public static int GA_OCCURRENCE_PLANS_COPY = 200;
        public static int GA_OCCURRENCE_PLANS_MANAGE = 201;

        public static int GA_SCHOLARSHIP_ENTITY_LIST_VIEW_SEARCH = 202;
        public static int GA_SCHOLARSHIP_ENTITY_NEW = 203;
        public static int GA_SCHOLARSHIP_ENTITY_EDIT = 204;
        public static int GA_SCHOLARSHIP_ENTITY_DELETE = 205;
        public static int GA_SCHOLARSHIP_ENTITY_VALUES_LIST_VIEW_SEARCH = 206;
        public static int GA_SCHOLARSHIP_ENTITY_VALUES_NEW = 207;
        public static int GA_SCHOLARSHIP_ENTITY_VALUES_EDIT = 208;
        public static int GA_SCHOLARSHIP_ENTITY_VALUES_DELETE = 209;

        public static int GA_STUDENTS_LIST_VIEW_SEARCH = 235;
        public static int GA_STUDENTS_FILEMGR = 237;
        public static int GA_STUDENTS_STATUTES_LIST_VIEW_SEARCH = 277;
        public static int GA_STUDENTS_STATUTES_NEW = 278;
        public static int GA_STUDENTS_STATUTES_EDIT = 279;
        public static int GA_STUDENTS_STATUTES_DELETE = 280;
        public static int GA_STUDENTS_OBSERVATIONS = 281;

        public static int GA_REQUEST_LIST_VIEW_SEARCH = 248;
        public static int GA_REQUEST_NEW = 249;
        public static int GA_REQUEST_EDIT = 250;
        public static int GA_REQUEST_DELETE = 251;
        public static int GA_REQUEST_CANCEL_TYPE_1 = 252;
        public static int GA_REQUEST_CANCEL_TYPE_2 = 253;
        public static int GA_REQUEST_PDF_REPORT = 282;
        public static int GA_REQUEST_PAYMENT_EXEMPT = 290;

        public static int GA_REQUEST_TYPE_LIST_VIEW_SEARCH = 244;
        public static int GA_REQUEST_TYPE_NEW = 245;
        public static int GA_REQUEST_TYPE_EDIT = 246;
        public static int GA_REQUEST_TYPE_DELETE = 247;

        public static int GA_GUIDELINES_PARAMETERS = 256;

        public static int GA_SUBJECTS_FACULTY_SERVICES_LIST_VIEW_SEARCH = 312;
        public static int GA_SUBJECTS_FACULTY_SERVICES_NEW = 313;
        public static int GA_SUBJECTS_FACULTY_SERVICES_EDIT = 314;
        public static int GA_SUBJECTS_FACULTY_SERVICES_DELETE = 315;

        public static int GA_INST_UNITS_LIST_VIEW_SEARCH = 316;

        public static int GA_FACULTY_SERVICES_LIST_VIEW_SEARCH = 317;
        public static int GA_FACULTY_SERVICES_NEW = 318;
        public static int GA_FACULTY_SERVICES_EDIT = 319;
        public static int GA_FACULTY_SERVICES_DELETE = 320;

        public static int GA_LECTURERS_LIST_VIEW_SEARCH = 321;

        public static int GA_GUIDELINES_TEMPLATES_LIST_VIEW_SEARCH = 326;
        public static int GA_GUIDELINES_TEMPLATES_NEW = 327;
        public static int GA_GUIDELINES_TEMPLATES_EDIT = 328;
        public static int GA_GUIDELINES_TEMPLATES_DELETE = 329;
        public static int GA_GUIDELINES_TEMPLATES_COPY = 337; 

        public static int GA_GUIDELINES_TEMPLATES_SUBJECTS_LIST_VIEW_SEARCH = 330;
        public static int GA_GUIDELINES_TEMPLATES_SUBJECTS_NEW = 331;
        public static int GA_GUIDELINES_TEMPLATES_SUBJECTS_DELETE = 332;

        public static int GA_GUIDELINES_TEMPLATES_COMPONENTS_LIST_VIEW_SEARCH = 333;
        public static int GA_GUIDELINES_TEMPLATES_COMPONENTS_NEW = 334;
        public static int GA_GUIDELINES_TEMPLATES_COMPONENTS_EDIT = 335;
        public static int GA_GUIDELINES_TEMPLATES_COMPONENTS_DELETE = 336;
        
        public static int GA_GUIDELINES_LRES_LIST_SEARCH = 339;
        public static int GA_GUIDELINES_LRES_VIEW = 346;
        public static int GA_GUIDELINES_LRES_EDIT = 340;
        public static int GA_GUIDELINES_LRES_PUBLISH = 341;
        public static int GA_GUIDELINES_LRES_CANCEL_PUBLISH = 342;
        public static int GA_GUIDELINES_LRES_EVENTS = 343;
        public static int GA_GUIDELINES_LRES_UNBLOCK = 344;
        public static int GA_GUIDELINES_LRES_DELETE = 345;

        public static int GA_SCHEDULES_LIST_VIEW_SEARCH = 294;
        public static int GA_SCHEDULES_NEW = 295;
        public static int GA_SCHEDULES_EDIT = 296;
        public static int GA_SCHEDULES_DELETE = 297;
        public static int GA_SCHEDULES_COPY = 298;
        public static int GA_SCHEDULES_MANAGE = 299;

        public static int GA_EXAMS_ENROLLMENT_LIST_VIEW_SEARCH = 347;
        public static int GA_EXAMS_ENROLLMENT_NEW = 348;
        public static int GA_EXAMS_ENROLLMENT_NEW_FIN_EXCEPTION = 349;
        public static int GA_EXAMS_ENROLLMENT_CANCEL = 350;
        public static int GA_EXAMS_ENROLLMENT_DELETE = 351;
        public static int GA_EXAMS_ENROLLMENT_EMOLS_LIST_VIEW_SEARCH = 352;
        public static int GA_EXAMS_ENROLLMENT_EMOLS_EDIT = 353;
        public static int GA_EXAMS_ENROLLMENT_NEW_PAYMENT_EXEMPT = 361;


        public static int GA_COURSES_TRANSITION_RULES = 363;

        public static int GA_COURSES_PRECEDENCES_LIST_VIEW_SEARCH = 364;
        public static int GA_COURSES_PRECEDENCES_NEW = 365;
        public static int GA_COURSES_PRECEDENCES_EDIT = 366;
        public static int GA_COURSES_PRECEDENCES_DELETE = 367;

        public static int GA_ENROLLMENTS_LIST_VIEW_SEARCH = 368;
        public static int GA_ENROLLMENTS_NEW = 369;
        public static int GA_ENROLLMENTS_NEW_EXCEPTION = 370;
        public static int GA_ENROLLMENTS_DELETE = 371;

        public static int GA_ENROLLMENTS_VERSION_NEW = 372;
        public static int GA_ENROLLMENTS_VERSION_EDIT = 373;
        public static int GA_ENROLLMENTS_VERSION_DELETE = 374;


        public static int GP_USERS_LIST_VIEW_SEARCH = 121;
        public static int GP_USERS_EDIT = 122;
        public static int GP_USERS_FILEMGR = 123;
        public static int GP_USERS_ALTER_PHOTOGRAPH = 124;

        public static int GP_USERS_ADDRESS_LIST_VIEW_SEARCH = 125;
        public static int GP_USERS_ADDRESS_NEW = 126;
        public static int GP_USERS_ADDRESS_EDIT = 127;
        public static int GP_USERS_ADDRESS_DELETE = 128;

        public static int GP_USERS_IDENTIFICATION_LIST_VIEW_SEARCH = 129;
        public static int GP_USERS_IDENTIFICATION_NEW = 130;
        public static int GP_USERS_IDENTIFICATION_EDIT = 131;
        public static int GP_USERS_IDENTIFICATION_DELETE = 132;

        public static int GP_USERS_PROFESSIONAL_LIST_VIEW_SEARCH = 133;
        public static int GP_USERS_PROFESSIONAL_NEW = 134;
        public static int GP_USERS_PROFESSIONAL_EDIT = 135;
        public static int GP_USERS_PROFESSIONAL_DELETE = 136;

        public static int GP_USERS_ACADEMIC_LIST_VIEW_SEARCH = 137;
        public static int GP_USERS_ACADEMIC_NEW = 138;
        public static int GP_USERS_ACADEMIC_EDIT = 139;
        public static int GP_USERS_ACADEMIC_DELETE = 140;

        public static int GP_USERS_FAM_LIST_VIEW_SEARCH = 141;
        public static int GP_USERS_FAM_NEW = 142;
        public static int GP_USERS_FAM_EDIT = 143;
        public static int GP_USERS_FAM_DELETE = 144;

        public static int GP_USERS_DEFICIENCY_LIST_VIEW_SEARCH = 145;
        public static int GP_USERS_DEFICIENCY_NEW = 146;
        public static int GP_USERS_DEFICIENCY_EDIT = 147;
        public static int GP_USERS_DEFICIENCY_DELETE = 148;

        public static int GP_FILEMGR = 284;  

        public static int GPAG_SERIES_LIST_VIEW_SEARCH = 179;
        public static int GPAG_SERIES_NEW = 180;
        public static int GPAG_SERIES_EDIT = 181;
        public static int GPAG_SERIES_DELETE = 182;

        public static int GPAG_TAXES_LIST_VIEW_SEARCH = 183;
        public static int GPAG_TAXES_NEW = 184;
        public static int GPAG_TAXES_EDIT = 185;
        public static int GPAG_TAXES_DELETE = 186;

        public static int GPAG_PAYMENTMETHODS_LIST_VIEW_SEARCH = 187;
        public static int GPAG_PAYMENTMETHODS_NEW = 188;
        public static int GPAG_PAYMENTMETHODS_EDIT = 189;
        public static int GPAG_PAYMENTMETHODS_DELETE = 190;

        public static int GPAG_TYPOLOGIES_LIST_VIEW_SEARCH = 210;
        public static int GPAG_TYPOLOGIES_NEW = 211;
        public static int GPAG_TYPOLOGIES_EDIT = 212;
        public static int GPAG_TYPOLOGIES_DELETE = 213;

        public static int GPAG_TYPOLOGIES_VALUE_LIST_VIEW_SEARCH = 214;
        public static int GPAG_TYPOLOGIES_VALUE_NEW = 215;
        public static int GPAG_TYPOLOGIES_VALUE_EDIT = 216;
        public static int GPAG_TYPOLOGIES_VALUE_DELETE = 217;

        public static int GPAG_TYPOLOGIES_TAXES_LIST_VIEW_SEARCH = 218;
        public static int GPAG_TYPOLOGIES_TAXES_NEW = 219;
        public static int GPAG_TYPOLOGIES_TAXES_DELETE = 220;

        public static int GPAG_TYPOLOGIES_FINES_LIST_VIEW_SEARCH = 272;
        public static int GPAG_TYPOLOGIES_FINES_NEW = 273;
        public static int GPAG_TYPOLOGIES_FINES_EDIT = 274;
        public static int GPAG_TYPOLOGIES_FINES_DELETE = 275;

        public static int GPAG_PAYMENT_PLANS_LIST_VIEW_SEARCH = 221;
        public static int GPAG_PAYMENT_PLANS_NEW = 222;
        public static int GPAG_PAYMENT_PLANS_EDIT = 223;
        public static int GPAG_PAYMENT_PLANS_DELETE = 224;
        public static int GPAG_PAYMENT_PLANS_COPY = 225;
        public static int GPAG_SUB_PAYMENT_PLANS_LIST_VIEW_SEARCH = 226;
        public static int GPAG_SUB_PAYMENT_PLANS_NEW = 227;
        public static int GPAG_SUB_PAYMENT_PLANS_EDIT = 228;
        public static int GPAG_SUB_PAYMENT_PLANS_MANAGE = 229;
        public static int GPAG_PAYMENT_PLANS_ADDITIONAL_PAYMENTS_LIST_VIEW_SEARCH = 230;
        public static int GPAG_PAYMENT_PLANS_ADDITIONAL_PAYMENTS_MANAGE = 231;

        public static int GPAG_CHECKINGACCOUNTS_LIST_VIEW_SEARCH = 238;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_NEW_DEBIT = 257;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_NEW_CREDIT = 258;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_EDIT_CREDIT = 259;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_EDIT_DEBIT = 260;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_CANCEL_DEBIT = 261;
        public static int GPAG_CHECKINGACCOUNTS_EMOL_CANCEL_CREDIT = 262;
        public static int GPAG_CHECKINGACCOUNTS_FILEMGR = 263;
        public static int GPAG_CHECKINGACCOUNTS_OBSERVATIONS = 264;
        public static int GPAG_CHECKINGACCOUNTS_RECEIPT_CANCEL = 265;
        public static int GPAG_CHECKINGACCOUNTS_PROCESS_PLAN = 276;

        public static int GPAG_FIN_RULES_VIEW_MANAGE = 354; 

        public static int GPAG_TREASURY = 266;

        public static int BI_GPAG_FC_LIST_VIEW_SEARCH = 267;
        public static int BI_GA_APPLICATIONS_LIST_VIEW_SEARCH = 268;
        public static int BI_GA_STUDENTS_LIST_VIEW_SEARCH = 283;
        public static int BI_RH_TIMETRACK_LIST_VIEW_SEARCH = 311;
        public static int BI_GA_EXAMS_LIST_LIST_VIEW_SEARCH = 300; 

        public static int RH_HOLIDAYS_LIST_VIEW_SEARCH = 302;
        public static int RH_HOLIDAYS_NEW = 303;
        public static int RH_HOLIDAYS_EDIT = 304;
        public static int RH_HOLIDAYS_DELETE = 305;

        public static int RH_ABSENCES_LIST_VIEW_SEARCH = 306;
        public static int RH_ABSENCES_NEW = 307;
        public static int RH_ABSENCES_EDIT = 308;
        public static int RH_ABSENCES_DELETE = 309;

        public static int RH_TIMETRACK_PROCESS_MOVEMENTS = 310;

        public static int PA_ASSESSORIA = 322;
        public static int GA_PA_CMS_LIST_VIEW_SEARCH = 323;
        public static int GA_PA_CMS_FILEMGR = 324;
        public static int GA_PA_CMS_SOCIAL = 325;
        public static int GA_PA_CMS_ANNOUNCEMENTS = 355;

        public static int GA_PD_CMS_LIST_VIEW_SEARCH = 358;
        public static int GA_PD_CMS_FILEMGR = 359;
        public static int GA_PD_CMS_ANNOUNCEMENTS = 360;

        public static int GA_STUDENTS_COMPLAINTS_LIST_VIEW_SEARCH = 356;
        public static int GA_STUDENTS_COMPLAINTS_REPLY = 357;

        public static int PD_ASSESSORIA = 338;

        //
        public static int GA_SUMMARY_INIT = 338;
        //

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