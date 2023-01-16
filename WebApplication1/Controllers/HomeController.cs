using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
     
        [HttpGet]
        public ActionResult Index(FormCollection frm)
        {
         
            List<data> reservationList = new List<data>();
         
            ViewBag.err = "";
            return View(reservationList);
        }


        [HttpPost]
        public ActionResult Index(string myText)
        {
            string url = "";
            List<data> reservationList = new List<data>();
            var httpClient = new HttpClient();

            if (myText == "")
            {
                try
                {
                    url = "http://dummy.restapiexample.com/api/v1/employees";
                                     string response = httpClient.GetStringAsync(url).Result;
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                            foreach (JObject structKind in jsonObj["data"])
                    {
                        var data = JsonConvert.DeserializeObject<data>(structKind.ToString());
                        data.anual_salary = AnualSalaryCal(data.employee_salary);
                        reservationList.Add(data);
                        ViewBag.err = "";
                    }
                }catch(Exception ex)
                {
                    ViewBag.err = "External API says : Too Many Requests (Try again later.)";
                }

            }
            else
            {

                ViewBag.pass = myText;

                 url = "http://dummy.restapiexample.com/api/v1/employee/"+myText;

           
          
                HttpClient client = new HttpClient();
                try
                {
                    string response = client.GetStringAsync(url).Result;

                    var data = JsonConvert.DeserializeObject<root>(response);
                    data dt1 = new data();
                    dt1.id = data.data.id;
                    dt1.employee_name = data.data.employee_name;
                    dt1.employee_age = data.data.employee_age;
                    dt1.employee_salary = data.data.employee_salary;
                    dt1.anual_salary = AnualSalaryCal(data.data.employee_salary);

                    reservationList.Add(dt1);
                }catch(Exception ex)
                {
                    ViewBag.err = "External API says : Too Many Requests (Try again later.)";
                }

            }



            return View(reservationList);
        }

        public int AnualSalaryCal(int curSalary)
        {
            return curSalary * 12;
        }

    }
}