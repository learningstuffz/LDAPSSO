using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Auth_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAuthenticate
    {


        // TODO: Add your service operations here
      
        [OperationContract]
        bool Authenticate(string username, string domain, string password, string ldapUrl);

        [OperationContract]
        bool isAuthenticated(string username);

        [OperationContract]
        List<Dictionary<string, string>> FindUser(string basedn, string ldapfilter);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class LDAPUser
    {

        string _domain = "";
        string _cn = "cn={0}";
        string _uid = "uid={0}";
        string _url = "";
        string _password = "";
        string _userName = "";
        string _baseDN = "";
        [DataMember]
        public string BaseDN
        {
            get { return _baseDN; }
            set { _baseDN = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _userName; }
        }

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        
        [DataMember]
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }
        
        [DataMember]
        public string UID
        {
            get { return _uid; }
        }

        [DataMember]
        public string Domain
        {
            get { return _domain; }
        }

        [DataMember]
        public string CN
        {
            get { return _cn; }
        }
        

        public LDAPUser(string user, string domain,string url,string password)
        {
            _userName = user;
            _url = url;
            _password = password;
            _domain = String.Join(",", domain.Split('.').Select(d => string.Format("dc={0}", d)));
            
            _cn += "," + _domain;
            _cn = string.Format(_cn, user);

            _uid += "," + _domain;
            _uid = string.Format(_uid, user);
        }

        public LDAPUser(string user, string domain, string url,string password,int port)
        {
            _userName = user;

            _url = url+":"+Convert.ToString(port);//throws error if port is not properly defined
            _password = password;
            _domain = String.Join(",", domain.Split('.').Select(d => string.Format("dc={0}", d)));

            _cn += "," + _domain;
            _cn = string.Format(_cn, user);

            _uid += "," + _domain;
            _uid = string.Format(_uid, user);
        }
    }
}
