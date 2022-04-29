namespace ProjectManagementAPI.Models
{
    public class Task1
    {
       
        public int Project_Id { get; set; }

        public string Task_Title { get; set; }
        public string Status { get; set; }
        public int Assigned_to { get; set; }
        public int Assigned_by { get; set; }

        public DateTime Created_date { get; set; }

        public DateTime Updated_date { get; set; }

        public int Updated_by { get; set; }

        public DateTime End_date { get; set; }


    }
    public class Taskstatus
    {
        public string Task_Title { get; set; }
        public string Status { get; set; }

    }
}
