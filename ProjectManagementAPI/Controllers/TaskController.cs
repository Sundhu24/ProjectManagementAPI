using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Models;
using System.Data;
using System.Data.SqlClient;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public TaskController(IConfiguration config, IWebHostEnvironment env)
        {
            _configuration = config;
            _env = env;
        }
        /*
        // GET: api/<TaskController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        // GET api/<TaskController>/5
        [HttpGet("TaskTitle")]
        public DateResponse Get(string task_title)
        {
            DateResponse _objResponseModel = new DateResponse();

            string query = @"
                            select * from
                            Task where task_title=@task_title
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("prjDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@task_title", task_title);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count > 0)
            {
                List<dynamic> tskList = new List<dynamic>();
                Task1 tsk = new Task1();
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    //Task1 tsk = new Task1();
                    tsk.Project_Id = Convert.ToInt32(table.Rows[i]["project_id"]);
                    tsk.Task_Title = table.Rows[i]["task_title"].ToString();
                    tsk.Status = table.Rows[i]["status"].ToString();
                    tsk.Assigned_to = Convert.ToInt32(table.Rows[i]["assigned_to"]);
                    tsk.Assigned_by = Convert.ToInt32(table.Rows[i]["assigned_by"]);
                    tsk.Created_date = Convert.ToDateTime(table.Rows[i]["created_date"]);
                    tsk.Updated_date = Convert.ToDateTime(table.Rows[i]["updated_date"]);
                    tsk.Updated_by = Convert.ToInt32(table.Rows[i]["updated_by"]);
                    tsk.End_date = Convert.ToDateTime(table.Rows[i]["end_date"]);


                    tskList.Add(tsk);
                }


                _objResponseModel.Start_date = tsk.Created_date;
                _objResponseModel.End_date = tsk.End_date;
                _objResponseModel.Message = "Task Data Received successfully";

            }
            else
            {
                
                _objResponseModel.Message = "No Data Found";
            }
            return _objResponseModel;
        }
        /*
        // POST api/<TaskController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        */
        // PUT api/<TaskController>/5
        [HttpPut("Task Status")]
        public StatusResponse Put(Taskstatus tskdata)
        {
            StatusResponse _objResponseModel = new StatusResponse();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from Task
                              where task_title = @task_title";
            string sqlDataSource1 = _configuration.GetConnectionString("prjDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {
                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                    myCommand.Parameters.AddWithValue("@task_title", tskdata.Task_Title);
                    myCommand.Parameters.AddWithValue("@status",tskdata.Status);
                    rowexists = (int)myCommand.ExecuteScalar();
                }
                if (rowexists == 1)
                {
                        string query = @"
                            update Task set
                               status = @status
                               where task_title=@task_title
                               ";

                        DataTable table = new DataTable();
                        SqlDataReader myReader;
                        using (SqlCommand myCommand = new SqlCommand(query, myCon1))
                        {
                          myCommand.Parameters.AddWithValue("@task_title", tskdata.Task_Title);
                          myCommand.Parameters.AddWithValue("@status", tskdata.Status);


                          myReader = myCommand.ExecuteReader();
                          table.Load(myReader);
                          myReader.Close();
                         
                        }
                        _objResponseModel.Status = "Success";
                         _objResponseModel.Message = "Task Data Updated successfully";
                }
                else
                {
                    _objResponseModel.Status = "Failure";
                    _objResponseModel.Message = "Task Data does not exists";

                }

                myCon1.Close();

            }

            return _objResponseModel;
        }


        
        // DELETE api/<TaskController>/5
        [HttpDelete]
        public StatusResponse Delete(string task_title)
        {
            StatusResponse _objResponseModel = new StatusResponse();

            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from Task
                              where task_title = @task_title";
            string sqlDataSource1 = _configuration.GetConnectionString("prjDB");
            int rowexists = 0;

            Task1 tskdata = new Task1();

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {


                    myCommand.Parameters.AddWithValue("@task_title", task_title);
                    rowexists = (Int32)myCommand.ExecuteScalar();

                    if (rowexists == 1)
                    {
                        string query = @"
                           delete from Task where task_tile = @task_title";

                        DataTable table = new DataTable();

                        SqlDataReader myReader;

                        using (SqlCommand myCommand1 = new SqlCommand(query, myCon1))
                        {
                            
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();
                            myCon1.Close();
                        }

                        _objResponseModel.Status = "Success";
                        _objResponseModel.Message = "Task Deleted successfully";

                    }
                    else
                    {
                        _objResponseModel.Status = "Failure";
                        _objResponseModel.Message = "Task does not exists";

                    }
                }

            }
                return _objResponseModel;



           
            
        }
    
    }
}
