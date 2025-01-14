﻿using API.Models;
using API.ViewModel;
using Client.Base;
using Client.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeRepository(Address address, string request = "Employees/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext.Session.GetString("JWToken"));
        }

        public async Task<List<getAllVM>> GetAllProfile()
        {
            List<getAllVM> entities = new List<getAllVM>();

            using (var response = await httpClient.GetAsync(address.link + request + "masterdata/"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<getAllVM>>(apiResponse);
            }

            return entities;
        }

        public HttpStatusCode CreateNewProfile(RegisterVM newData)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(newData), Encoding.UTF8, "application/json");
            string req = "accounts/register/";
            var result = httpClient.PostAsync(address.link + req, content).Result;
            return result.StatusCode;
        }
    }
}
