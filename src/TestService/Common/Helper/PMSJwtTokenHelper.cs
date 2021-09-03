using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helper
{
    public class PMSJwtTokenHelper
    {

        public static RSACryptoServiceProvider GetRSACryptoProvider(string pemstr)
        {
            var rsa = GetRSACryptoServiceProvider(pemstr, true);
            return rsa;
        }
        private static RSACryptoServiceProvider GetRSACryptoServiceProvider(string key, bool isprivate)
        {
            try
            {
                RSAParameters rsaParams;
                if (isprivate)
                {
                    using (var sr = new StringReader(key))
                    {
                        //use BouncyCastle to convert the key to RSA parameters
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



        public static RSACryptoServiceProvider GetRSAProviderFromPem(string pemstr)
        {
            try
            {
                CspParameters cspParameters = new CspParameters();
                cspParameters.KeyContainerName = "MyKeyContainer";
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParameters);

                Func<RSACryptoServiceProvider, RsaKeyParameters, RSACryptoServiceProvider> MakePublicRCSP = (RSACryptoServiceProvider rcsp, RsaKeyParameters rkp) =>
                {
                    RSAParameters rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                    rcsp.ImportParameters(rsaParameters);
                    return rsaKey;
                };

                Func<RSACryptoServiceProvider, RsaPrivateCrtKeyParameters, RSACryptoServiceProvider> MakePrivateRCSP = (RSACryptoServiceProvider rcsp, RsaPrivateCrtKeyParameters rkp) =>
                {
                    RSAParameters rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                    rcsp.ImportParameters(rsaParameters);
                    return rsaKey;
                };

                PemReader reader = new PemReader(new StringReader(pemstr));
                object kp = reader.ReadObject();
                return (kp.GetType() == typeof(RsaPrivateCrtKeyParameters)) ? MakePrivateRCSP(rsaKey, (RsaPrivateCrtKeyParameters)kp) : MakePublicRCSP(rsaKey, (RsaKeyParameters)kp);
            }
            catch (Exception ex)
            {
                //LogHelper.Error("GetRSAProviderFromPem异常：", ex);
                return null;
            }
            // If object has Private/Public property, we have a Private PEM
        }

        public static RSACryptoServiceProvider GetRSAProviderFromPemFile(string pemfile)
        {
            return GetRSAProviderFromPem(File.ReadAllText(pemfile).Trim());
        }
    }
}
