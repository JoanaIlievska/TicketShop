using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using OnlineCinema.Domain.Identity;
using ExcelDataReader;
using Microsoft.AspNetCore.Identity;
using OnlineCinema.Services.Interface;

namespace OnlineCinema.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<OnlineCinemaApplicationUser> _userManager;


        public UserController(UserManager<OnlineCinemaApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();

        }


        [HttpGet("[action]")]
        public IActionResult ImportUsers()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult ImportUsers(IFormFile file)
        {

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";


            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            //read data from uploaded file

            List<OnlineCinemaApplicationUser> users = getUsersFromExcelFile(file.FileName);

   

            return RedirectToAction("Index", "User");
        }

        private List<OnlineCinemaApplicationUser> getUsersFromExcelFile(string fileName)
        {

            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<OnlineCinemaApplicationUser> userList = new List<OnlineCinemaApplicationUser>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new OnlineCinema.Domain.Identity.OnlineCinemaApplicationUser
                        {
                            FirstName = reader.GetValue(0).ToString(),
                            LastName = reader.GetValue(1).ToString(),
                            Email = reader.GetValue(2).ToString(),
                            Password = reader.GetValue(3).ToString(),
                            ConfirmedPassword = reader.GetValue(4).ToString(),
                            PhoneNumber = reader.GetValue(5).ToString()
                        });

                    }

                }
            }

            return userList;

        }
    }

   
}
