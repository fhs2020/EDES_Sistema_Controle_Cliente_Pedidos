using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_Core_1_0.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            ViewData["SubTitle"] = "Bem vindo! Sistema de pedidos online! ";
            ViewData["Message"] = "Um sistema para controle de pedidos para equipamentos do projeto EDES.";

            return View();
        }

        public IActionResult Minor()
        {

            ViewData["SubTitle"] = "Simple example of second view";
            ViewData["Message"] = "Data are passing to view by ViewData from controller";

            return View();
        }
    }
}