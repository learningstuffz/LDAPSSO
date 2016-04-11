using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Xml;

namespace Auth_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class LDAPAuth : IAuthenticate
    {
        LdapConnection connectionAuth;
        public bool Authenticate(string username, string domain, string password, string ldapUrl)
        {
            try
            {
                var ldapUser = new LDAPUser(username, domain, ldapUrl, password);

                /*checking using LDAP CN authentication*/
                if (AuthenticateCN(ldapUser) || AuthenticateUID(ldapUser))
                {
                    //user is authenticated
                    //storeUserDetails(ldapUser);
                }
                else
                {
                    throw new Exception("Authentication failed!!!");
                }



            }
            catch (Exception ex)
            {
                return false;

            }

            return true;
        }
        bool AuthenticateCN(LDAPUser user)
        {
            try
            {
                var serverId = new LdapDirectoryIdentifier(user.URL);
                var credentials = new NetworkCredential(user.CN, user.Password);
                var connection = new LdapConnection(serverId, credentials, AuthType.Basic);
                connection.SessionOptions.ProtocolVersion = 3;//Because we are using LDAPv3 for checking
                connection.Bind();
                connectionAuth = connection;
                user.BaseDN = user.CN;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        bool AuthenticateUID(LDAPUser user)
        {
            try
            {
                var serverId = new LdapDirectoryIdentifier(user.URL);
                var credentials = new NetworkCredential(user.UID, user.Password);
                var connection = new LdapConnection(serverId, credentials, AuthType.Basic);
                connection.SessionOptions.ProtocolVersion = 3;//Because we are using LDAPv3 for checking
                connection.Bind();
                connectionAuth = connection;
                user.BaseDN = user.UID;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool isAuthenticated(string username)
        {

            return false;
        }

        bool storeUserDetails(LDAPUser user)
        {
            using (XmlWriter writer = XmlWriter.Create(user.UserName))
            {
                writer.WriteStartDocument();
                
                writer.WriteStartElement("User");
                writer.WriteElementString("UserName", user.UserName.ToString());
                writer.WriteElementString("Login", Convert.ToString(DateTime.Now));
                
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return true;

        }


        public List<Dictionary<string, string>> FindUser(string baseDn, string ldapFilter)
        {
            var request = new SearchRequest(baseDn, ldapFilter, SearchScope.Subtree, null);
            var response = (SearchResponse)connectionAuth.SendRequest(request);

            var result = new List<Dictionary<string, string>>();

            foreach (SearchResultEntry entry in response.Entries)
            {
                var dic = new Dictionary<string, string>();
                dic["DN"] = entry.DistinguishedName;

                foreach (string attrName in entry.Attributes.AttributeNames)
                {
                    //For simplicity, we ignore multi-value attributes
                    dic[attrName] = string.Join(",", entry.Attributes[attrName].GetValues(typeof(string)));
                }

                result.Add(dic);
            }

            return result;
        }
    }
}
