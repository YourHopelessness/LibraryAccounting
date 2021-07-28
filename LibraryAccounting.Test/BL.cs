using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAccounting.BL.Services;
using LibraryAccounting.DAL;
using System;
using Moq;
using Xunit;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.DAL.DB;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace LibraryAccounting.Test
{
    public class BLService
    {
        [Fact]
        public void EmployeeLoginInfoTest()
        {
            var mockBase = new Mock<LibraryDbContext>();
            var mockHttp = new HttpContextAccessor();
            var authTest = new AuthService(mockBase.Object, (IMapper) new MapperConfigurateMap(), (IHttpContextAccessor) mockHttp.HttpContext);
            string username = "dev";
            string password = "admin";
            

            
        }
    }
}
