using System;
using System.Collections.Generic;
using FManage.Exceptions;
using FManage.MainMenu;
using FManage.Model;
using FManage.Repositry;
using FManage.Utilities;
using Microsoft.Extensions.Configuration;

namespace FManage.App
{
        public class Program
        {
            static void Main(string[] args)
            {
            
            menu MenuMain = new menu();

            MenuMain.ShowWelcomeMenu();
        }
        }
    }

