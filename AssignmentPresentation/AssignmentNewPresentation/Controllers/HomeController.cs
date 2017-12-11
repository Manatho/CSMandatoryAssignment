﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AssignmentNewPresentation.Models;
using AssignmentLogic;
using System.IO;

namespace AssignmentNewPresentation.Controllers
{
    public class HomeController : Controller
    {
        private IGiveAway _giveAway;

        public HomeController(IGiveAway giveAway)
        {
            _giveAway = giveAway;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_giveAway.AllSubmissions());
        }

        [HttpGet]
        public IActionResult Submit()
        {
            if (TempData.ContainsKey("ErrorState"))
                ViewData["ErrorState"] = TempData["ErrorState"].ToString();


            return View();
        }

       [HttpPost]
       public IActionResult Submit(Submission submission)
        {
            SubmitStates state = _giveAway.Submit(submission);

            if (state == SubmitStates.Success)
                return Redirect("./index");
            else
            {
                TempData["ErrorState"] = state.ToString() + submission.Birthday;
                return Redirect("./submit");
            }
                
            

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
