using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using User_Authentication.LDAPAuth;

namespace User_Authentication.Models
{
    public class LoginUser
    {
        [Required]
        [Display(Name="User Name")]
        public string Username
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Domain")]
        public string Domain
        {
            get;
            set;
        }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        
        public bool Message { get; set; }
        public bool Authenticate(string username, string password,string domain)
        {
            LDAPAuth.AuthenticateClient authService = new AuthenticateClient();
            string ldapUrl = System.Configuration.ConfigurationManager.AppSettings["ldapUrl"];
            authService.InnerChannel.OperationTimeout = System.TimeSpan.MaxValue;
            return authService.Authenticate(username,domain,password,ldapUrl);
        }
    }
}