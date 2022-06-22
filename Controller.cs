using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;

namespace tryone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpDashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public OpDashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly string dwhSource = "Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True";
        string cubeSource = @"Provider=SQLNCLI11.1;Data Source=report.icta.fr;Initial Catalog=ICTA_CUBE;Integrated Security=SSPI";
        //Data Source=ICTAPOWERBI;Initial Catalog=ICTA_DWH;Integrated Security=True






        [HttpGet("test")] //http://localhost:5000/api/OpDashboard/test
        public IEnumerable<Models.OpDashboard> GetTest()
        //public JsonResult GetTest2()
        {

            string cubeQuery = "   SELECT NON EMPTY { [Measures].[Consultation NA], [Measures].[Consultation incomplete], [Measures].[Consultation complete], " +
                "[Measures].[Consultation DEA], [Measures].[Consultation DEB], [Measures].[Consultation clean] } ON COLUMNS, " +
                "NON EMPTY { ([CRF consultation].[CRF consultation].[CRF consultation].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[ATU_MUCOVISCIDOSE] } ) ON COLUMNS FROM ( SELECT ( { [CRF section].[CRF section - requirement type].&[Mandatory (all patients)] } ) ON COLUMNS " +
                "FROM [Cube ICTA])) WHERE ( [CRF section].[CRF section - requirement type].&[Mandatory (all patients)], [Study].[Study name].&[ATU_MUCOVISCIDOSE] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

           

            while (reader.Read())   // read
            {
            }

            return Enumerable.Range(0, 5).Select(index => new Models.OpDashboard
            {
                azer0 = "bien",
                azer1 = 10,
                azer2 = 20, 
                azer3 = 30,
                azer4 = 40,
                azer5 = 50

            }).ToArray();

            //return new JsonResult(datatable);
        }

        //---------------------------------------------------------------------

        #region Clinical Operational API

        [HttpGet("site_identified_per_country")] //http://localhost:5000/api/OpDashboard/site_identified_per_country
        public IEnumerable<Models.OpDashboard.SiteIdentifiedPerCountry> GetSiteIdentifiedCountry()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Site status total] } ON COLUMNS, " +
                "NON EMPTY { ([Division domain].[Division domain - country].[Division domain - country].ALLMEMBERS * [Site status].[Site - status].[Site - status].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[FIRE] } ) ON COLUMNS " +
                "FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[FIRE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> label = new List<string>();
            List<int> value = new List<int>();
            //int value1 = 0;
            //int value2 = 0;

            while (reader.Read())   // read
            {
                if (reader.GetValue(2).ToString() == "Identified")
                {
                    label.Add(reader.GetValue(0).ToString());
                    value.Add(int.Parse(reader.GetValue(4).ToString()));
                }
                //value1 = value1 + int.Parse(reader.GetValue(0).ToString());
                //value2 = value2 + int.Parse(reader.GetValue(1).ToString());
                //Console.WriteLine(reader.GetValue(0).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, 2).Select(index => new Models.OpDashboard.SiteIdentifiedPerCountry
            {
                country = label[index],
                site_identified = value[index]
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("sites")] //http://localhost:5000/api/OpDashboard/sites
        public IEnumerable<Models.OpDashboard.Sites> GetSites()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Nb Potential Site], [Measures].[Site status total] } ON COLUMNS " +
                "FROM ( SELECT ( { [Site status].[Site - status].&[Initiated] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Study].[Study name].&[EPIPARK] } ) ON COLUMNS FROM [Cube ICTA])) " +
                "WHERE ( [Study].[Study name].&[EPIPARK], [Site status].[Site - status].&[Initiated] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            //List<string> label = new List<string>();
            //List<int> value = new List<int>();
            int value1 = 0;
            int value2 = 0;

            while (reader.Read())   // read
            {
                value1 = value1 + int.Parse(reader.GetValue(0).ToString());
                value2 = value2 + int.Parse(reader.GetValue(1).ToString());
                //Console.WriteLine(reader.GetValue(0).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, 1).Select(index => new Models.OpDashboard.Sites
            {
                total_value = value1,
                potential_value = value2
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("patients")] //http://localhost:5000/api/OpDashboard/patients
        public IEnumerable<Models.OpDashboard.Patients> GetPatients()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Nb Potential Patient], [Measures].[Patient status total] } ON COLUMNS " +
                "FROM ( SELECT ( { [Patient status].[Patient - status].[Included] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Study].[Study name].&[EPIPARK] } ) ON COLUMNS " +
                "FROM [Cube ICTA])) " +
                "WHERE ( [Study].[Study name].&[EPIPARK], [Patient status].[Patient - status].[Included] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            //List<string> label = new List<string>();
            //List<int> value = new List<int>();
            int value1 = 0;
            int value2 = 0;

            while (reader.Read())   // read
            {
                value1 = value1 + int.Parse(reader.GetValue(0).ToString());
                value2 = value2 + int.Parse(reader.GetValue(1).ToString());
                //Console.WriteLine(reader.GetValue(0).ToString());
                //Console.WriteLine(reader.GetValue(1).ToString());
            }

            return Enumerable.Range(0, 1).Select(index => new Models.OpDashboard.Patients
            {
                total_value = value1,
                potential_value = value2
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("site_status")] //http://localhost:5000/api/OpDashboard/site_status
        public IEnumerable<Models.OpDashboard.SiteStatus> GetSiteStatus()
        {

            //string cubeQuery = " SELECT NON EMPTY { [Measures].[Site status total] } ON COLUMNS, " +
            //    "NON EMPTY { ([Site status].[Site - status].[Site - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
            //    "FROM ( SELECT ( { [Study].[Study name].&[VERONE] } ) ON COLUMNS FROM [Cube ICTA]) " +
            //    "WHERE ( [Study].[Study name].&[VERONE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Site status total] } ON COLUMNS, " +
                "NON EMPTY { ([Site status].[Site - status].[Site - status].ALLMEMBERS * [Site status].[Site - last status YN].[Site - last status YN].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[VERONE] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[VERONE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            List<string> site_status = new List<string>();
            List<int> status_total = new List<int>();
            List<string> last_status_total = new List<string>();

            List<string> vrai_site_status = new List<string>();
            List<int> vrai_status_total = new List<int>();
            List<int> vrai_last_status_total = new List<int>();

            while (reader.Read())   // read
            {

                
                //for (int i = 0; i < datatable.Rows.Count; i++)
                //{
                //    //status_total[i] = int.Parse(reader.GetValue(1).ToString());
                //    //status_totalzz[i] = reader.GetValue(1).ToString();

                //    //Console.WriteLine(int.Parse(reader.GetValue(1).ToString()));
                //}
                site_status.Add(reader.GetValue(0).ToString());
                last_status_total.Add(reader.GetValue(2).ToString());
                status_total.Add(int.Parse(reader.GetValue(4).ToString()));
                //Console.WriteLine(reader.GetValue(3).ToString());
            }

            for (int i = 0; i < site_status.Count; i++)
            {
                if (i >= site_status.Count - 1)
                {
                    //Console.WriteLine("aie aie aie"+site_status[i]);
                    //i = i + 1;
                    vrai_site_status.Add(site_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    vrai_last_status_total.Add(status_total[i]);
                }
                else if (site_status[i] == site_status[i + 1])
                {
                    vrai_site_status.Add(site_status[i]);
                    Console.WriteLine("bizzbizz" + i);
                    vrai_status_total.Add(status_total[i] + status_total[i+1]);
                    if(last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if(last_status_total[i + 1] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i+1]);
                    }
                    i = i + 1;
                }
                else if(site_status[i] != site_status[i + 1])
                {
                    vrai_site_status.Add(site_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    if (last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if (last_status_total[i] == "No")
                    {
                        vrai_last_status_total.Add(0);
                    }
                    //vrai_last_status_total.Add(status_total[i]);
                }
                
            }
            //----
            for (int i = 0; i < site_status.Count; i++)
            {
                Console.WriteLine("site : "+site_status[i]);
            }
            for (int i = 0; i < status_total.Count; i++)
            {
                Console.WriteLine("value site : " + status_total[i]);
            }
            Console.WriteLine("---------------------");
            for (int i = 0; i < vrai_site_status.Count; i++)
            {
                Console.WriteLine("vrai : "+vrai_site_status[i]);
            }
            for (int i = 0; i < vrai_status_total.Count; i++)
            {
                Console.WriteLine("value vrai : " + vrai_status_total[i]);
            }
            for (int i = 0; i < vrai_last_status_total.Count; i++)
            {
                Console.WriteLine("value last vrai : " + vrai_last_status_total[i]);
            }

            return Enumerable.Range(0, vrai_site_status.Count).Select(index => new Models.OpDashboard.SiteStatus
            {
                label = vrai_site_status[index],
                status_total = vrai_status_total[index],
                last_status_total = vrai_last_status_total[index]
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("patient_status")] //http://localhost:5000/api/OpDashboard/patient_status
        public IEnumerable<Models.OpDashboard.PatientStatus> GetPatientStatus()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[Patient status total] } ON COLUMNS, " +
                "NON EMPTY { ([Patient status].[Patient - status].[Patient - status].ALLMEMBERS * [Patient status].[Patient - last status YN].[Patient - last status YN].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[VERONE] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[VERONE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            //string cubeQuery = " SELECT NON EMPTY { [Measures].[Patient status total] } ON COLUMNS, " +
            //    "NON EMPTY { ([Patient status].[Patient - status].[Patient - status].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
            //    "FROM ( SELECT ( { [Study].[Study name].&[VERONE] } ) ON COLUMNS FROM [Cube ICTA]) " +
            //    "WHERE ( [Study].[Study name].&[VERONE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            List<string> patient_status = new List<string>();
            List<int> status_total = new List<int>();
            List<string> last_status_total = new List<string>();

            List<string> vrai_patient_status = new List<string>();
            List<int> vrai_status_total = new List<int>();
            List<int> vrai_last_status_total = new List<int>();

            while (reader.Read())   // read
            {


                //for (int i = 0; i < datatable.Rows.Count; i++)
                //{
                //    //status_total[i] = int.Parse(reader.GetValue(1).ToString());
                //    //status_totalzz[i] = reader.GetValue(1).ToString();

                //    //Console.WriteLine(int.Parse(reader.GetValue(1).ToString()));
                //}
                patient_status.Add(reader.GetValue(0).ToString());
                last_status_total.Add(reader.GetValue(2).ToString());
                status_total.Add(int.Parse(reader.GetValue(4).ToString()));
                //Console.WriteLine(reader.GetValue(3).ToString());
            }

            for (int i = 0; i < patient_status.Count; i++)
            {
                if (i >= patient_status.Count - 1)
                {
                    //Console.WriteLine("aie aie aie"+site_status[i]);
                    //i = i + 1;
                    vrai_patient_status.Add(patient_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    vrai_last_status_total.Add(status_total[i]);
                }
                else if (patient_status[i] == patient_status[i + 1])
                {
                    vrai_patient_status.Add(patient_status[i]);
                    Console.WriteLine("bizzbizz" + i);
                    vrai_status_total.Add(status_total[i] + status_total[i + 1]);
                    if (last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if (last_status_total[i + 1] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i + 1]);
                    }
                    i = i + 1;
                }
                else if (patient_status[i] != patient_status[i + 1])
                {
                    vrai_patient_status.Add(patient_status[i]);
                    vrai_status_total.Add(status_total[i]);
                    if (last_status_total[i] == "Yes")
                    {
                        vrai_last_status_total.Add(status_total[i]);
                    }
                    else if (last_status_total[i] == "No")
                    {
                        vrai_last_status_total.Add(0);
                    }
                    //vrai_last_status_total.Add(status_total[i]);
                }

            }

            //List<string> patient_status = new List<string>();
            //List<int> status_total = new List<int>();

            //while (reader.Read())   // read
            //{
            //    //for (int i = 0; i < datatable.Rows.Count; i++)
            //    //{
            //    //    //status_total[i] = int.Parse(reader.GetValue(1).ToString());
            //    //    //status_totalzz[i] = reader.GetValue(1).ToString();

            //    //    //Console.WriteLine(int.Parse(reader.GetValue(1).ToString()));
            //    //}
            //    patient_status.Add(reader.GetValue(0).ToString());
            //    status_total.Add(int.Parse(reader.GetValue(2).ToString()));
            //    //Console.WriteLine(reader.GetValue(3).ToString());
            //}

            return Enumerable.Range(0, vrai_patient_status.Count).Select(index => new Models.OpDashboard.PatientStatus
            {
                label = patient_status[index],
                status_total = vrai_status_total[index],
                last_status_total = vrai_last_status_total[index]
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("curve")] //http://localhost:5000/api/OpDashboard/curve
        public IEnumerable<Models.OpDashboard.CurveOfInclusion> GetCurve()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Nb Patient] } ON COLUMNS, " +
                "NON EMPTY { ([Curve].[Month].[Month].ALLMEMBERS * [Curve].[Label].[Label].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[047SIMULTI] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[047SIMULTI] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> month = new List<string>();
            List<int> included = new List<int>();
            List<int> randomised = new List<int>();
            List<int> theoretical = new List<int>();

            while (reader.Read())   // read
            {
                //included.Add(int.Parse(reader.GetValue(2).ToString()));
                if (reader.GetValue(2).ToString() == "Included")
                {
                    included.Add(int.Parse(reader.GetValue(4).ToString()));
                }
                if (reader.GetValue(2).ToString() == "Randomised")
                {
                    randomised.Add(int.Parse(reader.GetValue(4).ToString()));
                }
                if (reader.GetValue(2).ToString() == "Theoretical")
                {
                    theoretical.Add(int.Parse(reader.GetValue(4).ToString()));
                    month.Add(reader.GetValue(0).ToString());
                }
                //Console.WriteLine(reader.GetValue(3).ToString());
            }

            return Enumerable.Range(0, 18).Select(index => new Models.OpDashboard.CurveOfInclusion
            {
                date = month[index],
                included = included[index],
                randomised = randomised[index],
                theoretical = theoretical[index]
            }).ToArray();

            //return new JsonResult(datatable);
        }

        [HttpGet("monitoring")] //http://localhost:5000/api/OpDashboard/monitoring
        public IEnumerable<Models.OpDashboard.Monitoring> GetOpDashboardMonitoring()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Monitoring status total], [Measures].[AVG monitoring per site] } ON COLUMNS, " +
                "NON EMPTY { ([Monitoring].[Monitoring - nature].[Monitoring - nature].ALLMEMBERS * [Monitoring].[Monitoring - mode].[Monitoring - mode].ALLMEMBERS ) } " +
                "DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Monitoring status].[Monitoring - last status YN].&[Yes] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Monitoring].[Monitoring - mode].&[Phone], [Monitoring].[Monitoring - mode].&[Visit] } ) ON COLUMNS " +
                "FROM ( SELECT ( { [Monitoring].[Monitoring - nature].&[Initiation], [Monitoring].[Monitoring - nature].&[Monitoring], " +
                "[Monitoring].[Monitoring - nature].&[Close-out active site], [Monitoring].[Monitoring - nature].&[Close-out inactive site], " +
                "[Monitoring].[Monitoring - nature].&[Data collection] } ) ON COLUMNS FROM ( SELECT ( { [Monitoring].[Monitoring - last status].&[Performed], " +
                "[Monitoring].[Monitoring - last status].&[Successful] } ) ON COLUMNS FROM ( SELECT ( { [Study].[Study name].&[FIRE] } ) ON COLUMNS FROM [Cube ICTA]))))) " +
                "WHERE ( [Study].[Study name].&[FIRE], [Monitoring].[Monitoring - last status].CurrentMember, [Monitoring status].[Monitoring - last status YN].&[Yes] ) CELL " +
                "PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> nature = new List<string>();
            List<string> mode = new List<string>();
            List<int> last_status = new List<int>();
            List<float> avg_monitoring_per_site = new List<float>();

            while (reader.Read())   // read
            {
                nature.Add(reader.GetValue(0).ToString());
                mode.Add(reader.GetValue(2).ToString());
                last_status.Add(int.Parse(reader.GetValue(4).ToString()));
                avg_monitoring_per_site.Add((float)Math.Round(float.Parse(reader.GetValue(5).ToString()), 2));

                //Console.WriteLine("0 : " + reader.GetValue(0).ToString() + "\n");
                //Console.WriteLine("1 : " + reader.GetValue(1).ToString() + "\n");
                //Console.WriteLine("2 : " + reader.GetValue(2).ToString() + "\n");
                //Console.WriteLine("3 : " + reader.GetValue(3).ToString() + "\n");
                //Console.WriteLine("4 : " + reader.GetValue(4).ToString() + "\n");
                //Console.WriteLine("5 : " + reader.GetValue(5).ToString() + "\n");
            }

            return Enumerable.Range(0, 5).Select(index => new Models.OpDashboard.Monitoring
            {
                nature = nature[index],
                mode = mode[index],
                last_status = last_status[index],
                avg_monitoring_per_site = avg_monitoring_per_site[index],

                //nature = "jsk",
                //mode = "sjl",
                //last_status = 45,
                //avg_monitoring_per_site = 3

            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("documents_conformity")] //http://localhost:5000/api/OpDashboard/documents_conformity
        public IEnumerable<Models.OpDashboard.Documents> GetDocumentsConformity()
        {

            string cubeQuery = " SELECT NON EMPTY { [Measures].[Document receipt], [Measures].[Document default unresolved] } ON COLUMNS, " +
                "NON EMPTY { ([Document].[Document - conform].[Document - conform].ALLMEMBERS ) } DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS " +
                "FROM ( SELECT ( { [Study].[Study name].&[047SIMULTI] } ) ON COLUMNS FROM [Cube ICTA]) WHERE ( [Study].[Study name].&[047SIMULTI] ) CELL PROPERTIES " +
                "VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> label = new List<string>();
            List<int> value = new List<int>();
            int received = 0;
            int default_unresolved = 0;

            while (reader.Read())   // read
            {
                //included.Add(int.Parse(reader.GetValue(2).ToString()));
                if (reader.GetValue(0).ToString() == "No")
                {
                    label.Add(reader.GetValue(0).ToString());
                    value.Add(int.Parse(reader.GetValue(2).ToString()));
                }
                if (reader.GetValue(0).ToString() == "Yes")
                {
                    label.Add(reader.GetValue(0).ToString());
                    value.Add(int.Parse(reader.GetValue(2).ToString()));
                }
                received = received + int.Parse(reader.GetValue(2).ToString());
                if (reader.IsDBNull(3))
                {
                    default_unresolved = default_unresolved + 0;
                }
                else
                {
                    default_unresolved = default_unresolved + int.Parse(reader.GetValue(3).ToString());
                }
                //Console.WriteLine(reader.GetValue(5).ToString());
            }

            return Enumerable.Range(0, 2).Select(index => new Models.OpDashboard.Documents
            {
                no_yes = label[index],
                value = value[index],
                received = received,
                default_unresolved = default_unresolved
            }).ToArray();

            //return new JsonResult(datatable);
        }


        [HttpGet("safety_ae")] //http://localhost:5000/api/OpDashboard/safety_ae
        public IEnumerable<Models.OpDashboard.Safety> GetSafetyAE()
        {

            string cubeQuery = "  SELECT NON EMPTY { [Measures].[AE total], [Measures].[AE ack not received] } ON COLUMNS, " +
                "NON EMPTY { ([AE].[AE - type].[AE - type].ALLMEMBERS * [AE].[AE - seriousness type].[AE - seriousness type].ALLMEMBERS ) } DIMENSION " +
                "PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS FROM ( SELECT ( { [Study].[Study name].&[FIRE] } ) ON COLUMNS FROM [Cube ICTA]) " +
                "WHERE ( [Study].[Study name].&[FIRE] ) CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS";

            AdomdConnection con = new AdomdConnection(cubeSource);
            con.Open();

            AdomdDataAdapter adapt = new AdomdDataAdapter(cubeQuery, con);
            DataTable datatable = new DataTable();
            adapt.Fill(datatable);

            AdomdCommand cmd = new AdomdCommand(cubeQuery, con);
            AdomdDataReader reader = cmd.ExecuteReader(); //Execute query

            Console.WriteLine(datatable.Rows.Count);

            List<string> label = new List<string>();
            string[] labels = { "Initial", "Follow-up" };
            Console.WriteLine(labels[0] + "sf" + labels[1]);
            List<int> value = new List<int>();
            int initial = 0;
            int followup = 0;
            int serious = 0;
            int ack_not_received = 0;

            while (reader.Read())   // read
            {
                //included.Add(int.Parse(reader.GetValue(2).ToString()));
                if (reader.GetValue(0).ToString() == "Initial")
                {
                    initial = initial + int.Parse(reader.GetValue(4).ToString());
                    //label.Add(reader.GetValue(0).ToString());
                    //value.Add(int.Parse(reader.GetValue(2).ToString()));
                }
                if (reader.GetValue(0).ToString() == "Follow-up")
                {
                    followup = followup + int.Parse(reader.GetValue(4).ToString());
                    //label.Add(reader.GetValue(0).ToString());
                    //value.Add(int.Parse(reader.GetValue(2).ToString()));
                }
                if (reader.GetValue(2).ToString() != "Non-serious")
                {
                    serious = serious + int.Parse(reader.GetValue(5).ToString());
                }
                Console.WriteLine(reader.GetValue(5).ToString()); //0 2 4 5
                ack_not_received = ack_not_received + int.Parse(reader.GetValue(5).ToString());
            }

            value.Add(initial);
            value.Add(followup);

            return Enumerable.Range(0, 2).Select(index => new Models.OpDashboard.Safety
            {
                initial_followup = labels[index],
                value = value[index],
                value2 = serious,
                ack_not_received = ack_not_received
            }).ToArray();

            //return new JsonResult(datatable);
        }
        #endregion Clinical Operational API
}
