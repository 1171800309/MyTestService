using Common.Helper;
using Common.Model;
using Jose;
using Microsoft.IdentityModel.Logging;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Common.Auth
{
    public class JwtHelper
    {
        public static string JavaInterfaceJwt(Sys_User user, int minute = 10)
        {
            UserInfo userInfo = new UserInfo()
            {
                deptName = user.DeptName,
                deptNameEn = null,
                code = user.UserNumber,
                deptId = user.DeptCode,
                name = user.UserName,
                userId = user.UserCode,
                deptCode = null,
                username = user.UserNumber,
                status = 1
            };

            var payload = new Dictionary<string, object>
            {
                { "sub", "api" },
                { "iss", "sangfor" },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(minute).ToUnixTimeSeconds() },
                { "user_name", userInfo},
                { "client_id", "sangfor"}
            };

            var header = new Dictionary<string, object>
            {
                { "alg", "RS256" }
            };

            //RSACryptoServiceProvider rsaCrypto = PemHelper.GetRSAProviderFromPem(KeyHandler.privateKey);//此方法在生产环境获取不到Token
            RSACryptoServiceProvider rsaCrypto = PMSJwtTokenHelper.GetRSACryptoProvider(KeyHandler.privateKey);
            var token = JWT.Encode(payload, rsaCrypto, JwsAlgorithm.RS256, extraHeaders: header);
            LogHelper.LogInformation("当前用户：" + user.UserName + ",获取Java-JWT-Token：" + token);
            return "Bearer " + token;
        }


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static UserToken SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new UserToken
            {
                Role = role != null ? role.ToString() : "",
                UserNumber = jwtToken.Claims.First(a => a.Type == "UserNumber").Value,
                UserCode = jwtToken.Claims.First(a => a.Type == "UserCode").Value,
                UserName = jwtToken.Claims.First(a => a.Type == "UserName").Value,
                DeptCode = jwtToken.Claims.First(a => a.Type == "DeptCode").Value,
                DeptName = jwtToken.Claims.First(a => a.Type == "DeptName").Value,

                AreaTrade = jwtToken.Claims.First(a => a.Type == "AreaTrade").Value,
                DYNewDataRight = jwtToken.Claims.First(a => a.Type == "DYNewDataRight").Value,
                IsSuper = jwtToken.Claims.First(a => a.Type == "IsSuper").Value,
                SubUser = jwtToken.Claims.First(a => a.Type == "SubUser").Value,
                DataHeadCode = jwtToken.Claims.First(a => a.Type == "DataHeadCode").Value,
                DivisionCode = jwtToken.Claims.First(a => a.Type == "DivisionCode").Value,
                DataAreaCode = jwtToken.Claims.First(a => a.Type == "DataAreaCode").Value,
                DataAgencyCode = jwtToken.Claims.First(a => a.Type == "DataAgencyCode").Value,
                DataGroupCode = jwtToken.Claims.First(a => a.Type == "DataGroupCode").Value,
                DataTrade2 = jwtToken.Claims.First(a => a.Type == "DataTrade2").Value,
                DataTrade1 = jwtToken.Claims.First(a => a.Type == "DataTrade1").Value,
                NewDataCusRight = jwtToken.Claims.First(a => a.Type == "NewDataCusRight").Value,
                NewDataPrjRight = jwtToken.Claims.First(a => a.Type == "NewDataPrjRight").Value,
                RoleCode = jwtToken.Claims.First(a => a.Type == "RoleCode").Value
            };
            return tm;
        }

        /// <summary>
        /// 创建和Pms系统对接的JwtToken
        /// </summary>
        public static string CreatePmsJwt()
        {
            string publicKeyFilePath = Appsettings.app(new string[] { "AppSettings", "PmsSetting", "PublicKeyFilePath" });
            string privateKeyFilePath = Appsettings.app(new string[] { "AppSettings", "PmsSetting", "PrivateKeyFilePath" });
            string publicKey = File.ReadAllText(publicKeyFilePath);
            string privateKey = File.ReadAllText(privateKeyFilePath);
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var payload = new Dictionary<string, object>
            {
                {"iss","pms"},
                {"aud",new string[]{ "crm"} },
                {"exp",((int)DateTime.UtcNow.AddMinutes(5).Subtract(UnixEpoch).TotalSeconds).ToString(System.Globalization.CultureInfo.InvariantCulture)}
            };
            var gettoken = JwtEncryptRS256(payload, privateKey);   //私钥加密

            return gettoken;
        }

        public static string CreatePmsJwt2()
        {
            string publicKeyFilePath = Appsettings.app(new string[] { "AppSettings", "PmsSetting", "PublicKeyFilePath" });
            string privateKeyFilePath = Appsettings.app(new string[] { "AppSettings", "PmsSetting", "PrivateKeyFilePath" });
            string publicKey = File.ReadAllText(publicKeyFilePath);
            string privateKey = File.ReadAllText(privateKeyFilePath);
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var payload = new Dictionary<string, object>
            {
                {"iss","pms"},
                {"aud",new string[]{ "crm"} },
                {"exp",((int)DateTime.UtcNow.AddMinutes(5).Subtract(UnixEpoch).TotalSeconds).ToString(System.Globalization.CultureInfo.InvariantCulture)}
            };
            var gettoken = JwtEncryptRS256(payload, privateKey);   //私钥加密

            return gettoken;
        }

        /// <summary>
        /// 校验PMS的JwtToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsValidPmsJwtToken(string token, out string msg)
        {
            msg = "";
            string publicKeyFilePath = Appsettings.app(new string[] { "AppSettings", "PmsSetting", "PublicKeyFilePath" });
            string publicKey = File.ReadAllText(publicKeyFilePath);
            var result = JwtDecryptRS256(token, publicKey);   //公钥解密
            if (result != null)
            {
                var iss = string.Empty;
                var aud = string.Empty;
                int exp = 0;
                if (result.ContainsKey("iss"))
                {
                    result.TryGetValue("iss", out object outIss);
                    iss = outIss.ToString();
                    if (iss != "pms")
                    {
                        msg = "iss参数错误";
                        return false;
                    }
                }
                if (result.ContainsKey("exp"))
                {
                    result.TryGetValue("exp", out object outExp);
                    exp = Convert.ToInt32(outExp.ToString());
                    DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    var nowtimespan = (int)DateTime.UtcNow.Subtract(UnixEpoch).TotalSeconds;
                    if (nowtimespan > exp)
                    {
                        msg = "Token已过期";
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// JWT-RS256 加密
        /// 私钥加密，公钥解密
        /// </summary>
        /// <param name="payload">负荷部分，存储使用的信息</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string JwtEncryptRS256(IDictionary<string, object> payload, string privateKey)
        {
            //可以扩展header
            //var headers = new Dictionary<string, object>()
            //{
            //     { "typ", "JWT" },
            //     { "cty", "JWT" },
            //     { "keyid", "111-222-333"}
            //};

            using (var rsa = GetRSACryptoServiceProvider(privateKey, true)) //读取私钥
            {
                var token = Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
                //var token = Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256, headers);  //带header
                return token;
            }
        }

        /// <summary>
        /// JWT-RS256 解密
        /// </summary>
        /// <param name="token">要解密的token</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static Dictionary<string, object> JwtDecryptRS256(string token, string publicKey)
        {
            try
            {
                using (var rsa = GetRSACryptoServiceProvider(publicKey, false)) //读取公钥
                {
                    var payload = Jose.JWT.Decode<Dictionary<string, object>>(token: token, key: rsa);
                    return payload;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 根据 私钥 or 公钥 返回参数
        /// </summary>
        /// <param name="key">私钥 or 公钥</param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetRSACryptoServiceProvider(string key, bool isprivate)
        {
            try
            {
                RSAParameters rsaParams;
                if (isprivate)
                {
                    using (var sr = new StringReader(key))
                    {
                        // use BouncyCastle to convert the key to RSA parameters
                        //需要引用包 BouncyCastle
                        var pemReader = new PemReader(sr);
                        var keyPair = pemReader.ReadObject() as RsaPrivateCrtKeyParameters;
                        if (keyPair == null)
                        {
                            //没有读到Privatekey
                        }
                        rsaParams = DotNetUtilities.ToRSAParameters(keyPair);
                    }
                }
                else
                {
                    using (var sr = new StringReader(key))
                    {
                        var pemReader = new PemReader(sr);
                        var keyPair = pemReader.ReadObject() as RsaKeyParameters;
                        if (keyPair == null)
                        {
                            //没有读到Publickey
                        }
                        rsaParams = DotNetUtilities.ToRSAParameters(keyPair);
                    }
                }
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
