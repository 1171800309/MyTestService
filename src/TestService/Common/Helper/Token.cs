using Jose;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helper
{
    class Token
    {
        public static string Generate()
        {
            UserInfo userInfo = new UserInfo()
            {
                deptName = "信息系统部平台组",
                deptNameEn = null,
                code = "21401",
                deptId = "2824",
                name = "陈健行",
                userId = "19535",
                deptCode = null,
                username = "21401",
                status = 1
            };

            var payload = new Dictionary<string, object>
            {
                { "sub", "api" },
                { "iss", "sangfor" },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds() },
                { "user_name", userInfo},
                { "client_id", "sangfor"}
            };

            var header = new Dictionary<string, object>
            {
                { "alg", "RS256" }
                //{ "typ", "JWT" }
            };

            RSACryptoServiceProvider rsaCrypto = PemHelper.GetRSAProviderFromPem(KeyHandler.privateKey);
            var token = JWT.Encode(payload, rsaCrypto, JwsAlgorithm.RS256, extraHeaders: header);

            //RSAprovider
            // RSACryptoServiceProvider rsaCryptoPub = PemHelper.GetRSAProviderFromPem(KeyHandler.publicKey);
            // var data = JWT.Decode(token, rsaCryptoPub);

            return token;
        }
    }
}

public class UserInfo
{
    public string deptName { get; set; }
    public string deptNameEn { get; set; }
    public string code { get; set; }
    public string deptId { get; set; }
    public string name { get; set; }
    public string userId { get; set; }
    public string deptCode { get; set; }
    public string username { get; set; }
    public int status { get; set; }
}
