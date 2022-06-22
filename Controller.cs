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



//-------------Site Status

        [HttpGet("site_status")] //http://localhost:5000/api/OpDashboard/site_status
        public IEnumerable<Models.OpDashboard.SiteStatus> GetSiteStatus()
        {

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

            while (reader.Read())   // read
            {

                site_status.Add(reader.GetValue(0).ToString());
                status_total.Add(int.Parse(reader.GetValue(2).ToString()));
                last_status_total.Add(int.Parse(reader.GetValue(4).ToString()));
            }
            
            return Enumerable.Range(0, vrai_site_status.Count).Select(index => new Models.OpDashboard.SiteStatus
            {
                label = site_status[index],
                status_total = status_total[index],
                last_status_total = last_status_total[index]
            }).ToArray();

        }


}
