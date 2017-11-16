using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ISAT.Business;

namespace TestingObject
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetConnectionString("DefaultConnection");
            TicketTypeDetailInfoList list = TicketTypeDetailInfoList.GetTicketTypeDetailInfoList(8);
        }

        private static void TestingAgent()
        {
            //Agent obj = Agent.NewAgent();
            //obj.idCustAgent = "agen01";
            //obj.CustAgentName = "PT Aplikasi Lintasarta 2005";
            //obj.AgentDesc = "";
            //obj.IsSupervisor = false;
            //obj.CreatedBy = "Admin";
            //obj.CreatedDate = DateTime.Now;
            //obj.UpdatedBy = obj.CreatedBy;
            //obj.UpdatedDate = obj.CreatedDate;

            //obj = obj.Save();

            //obj = Agent.NewAgent();
            //obj.idCustAgent = "spv01";
            //obj.CustAgentName = "PT Aplikasi Lintasarta 2005";
            //obj.AgentDesc = "";
            //obj.IsSupervisor = true;
            //obj.CreatedBy = "Admin";
            //obj.CreatedDate = DateTime.Now;
            //obj.UpdatedBy = obj.CreatedBy;
            //obj.UpdatedDate = obj.CreatedDate;

            //obj = obj.Save();

            AgentInfo info = AgentInfo.GetAgentInfo("agen01");
            AgentInfoList infolist = AgentInfoList.GetAgentInfoList();


        }

        private static void TestingCustomer()
        {
            //Customer obj = Customer.NewCustomer();
            //obj.CustNo = "";
            //obj.FirstName = "Anindya";
            //obj.LastName = "Kurniawan";
            //obj.Email = "anindya@clnydigital.com";
            //obj.CompanyName = "Colony Digital";
            //obj.CreatedBy = "Admin";
            //obj.CreatedDate = DateTime.Now;
            //obj.UpdatedBy = obj.CreatedBy;
            //obj.UpdatedDate = obj.CreatedDate;
            ////obj = obj.Save();

            //obj = Customer.GetCustomer(17);
            //obj.CustNo = "085640850840";
            ////obj.Delete();
            //obj = obj.Save();

            CustomerInfoList infolist = CustomerInfoList.GetCustomerInfoList();
            CustomerList list = CustomerList.GetCustomerList();
        }

        private static void TestingTicketType()
        {
            TicketType obj = TicketType.NewTicketType();
            obj.TicketTypeName = "Badan Hukum - Perseroan";
            obj.TicketDesc = "";
            obj.CreatedBy = "Admin";
            obj.CreatedDate = DateTime.Now;
            obj.UpdatedBy = obj.CreatedBy;
            obj.UpdatedDate = obj.CreatedDate;
            obj = obj.Save();
            obj = TicketType.GetTicketType(obj.idTicketType);
            obj.TicketDesc = "Untuk yang berbadan hukum perseroan";
            obj = obj.Save();

            obj = TicketType.NewTicketType();
            obj.TicketTypeName = "Badan Hukum - Yayasan";
            obj.TicketDesc = "";
            obj.CreatedBy = "Admin";
            obj.CreatedDate = DateTime.Now;
            obj.UpdatedBy = obj.CreatedBy;
            obj.UpdatedDate = obj.CreatedDate;

            obj = obj.Save();
            obj = TicketType.GetTicketType(obj.idTicketType);
            obj.TicketDesc = "Untuk yang berbadan hukum yayasan";
            obj = obj.Save();

            TicketTypeInfoList infolist = TicketTypeInfoList.GetTicketTypeInfoList();
            TicketTypeList list = TicketTypeList.GetTicketTypeList();
        }

        private static void TestingTicket()
        {
            //Ticket obj = Ticket.NewTicket();
            //obj.TicketNo = "TICKET01";
            //obj.TicketSubject = "Testing Create Ticket";
            //obj.Requester = "anindya@clnydigital.com";
            //obj.CallerPosistion = "Provinsi";
            //obj.Priority = Priority.Medium.ToString();
            //obj.TicketOwner = "agen01";
            //obj.TicketType = TicketTypeInfoList.GetTicketTypeInfoList().First().TicketTypeName;
            //obj.TicketStatus = TicketStatus.Draft.ToString();
            //obj.TicketDescription = "";

            //TicketSolutionList list = new TicketSolutionList();
            //TicketSolution sol = new TicketSolution()
            //{
            //    HowToSolving = "Rahasia",
            //    UpdatedBy = obj.TicketOwner,
            //    UpdatedDate = DateTime.Now
            //};
            //list.Add(sol);
            //sol = new TicketSolution()
            //{
            //    HowToSolving = "Kepo",
            //    UpdatedBy = obj.TicketOwner,
            //    UpdatedDate = DateTime.Now
            //};
            //list.Add(sol);
            //string solution = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            //list = null;
            //list = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketSolutionList>(solution);
            //obj.Solution = solution;
            //obj.Escalation = obj.TicketOwner;

            //obj.CreatedBy = "Admin";
            //obj.CreatedDate = DateTime.Now;
            //obj.UpdatedBy = obj.CreatedBy;
            //obj.UpdatedDate = obj.CreatedDate;

            //Ticket obj = Ticket.GetTicket(15);
            //TicketSolutionList list = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketSolutionList>(obj.Solution);
            //TicketSolution sol = new TicketSolution()
            //{
            //    HowToSolving = "Mau tau aja atau mau tau banget?",
            //    UpdatedBy = obj.TicketOwner,
            //    UpdatedDate = DateTime.Now
            //};
            //list.Add(sol);
            //string solution = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            //obj.Solution = solution;

            //obj.Delete();

            //obj = obj.Save();

            TicketInfoList infolist = TicketInfoList.GetTicketInfoList();
            TicketList list = TicketList.GetTicketList();
        }
    }
}
