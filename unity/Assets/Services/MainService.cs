// get data from API-Endpoints
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assets.Models;
using UnityEngine;

namespace Assets.Services
{
    public class MainService
    {
        public readonly HttpClient _httpClient;

        // Base URL
        public MainService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7167")
            };
        }
    }
}
