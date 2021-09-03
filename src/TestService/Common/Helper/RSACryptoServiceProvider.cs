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
    public class PemHelper
    {
        public static RSACryptoServiceProvider GetRSAProviderFromPem(String pemstr)
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
            // If object has Private/Public property, we have a Private PEM
        }

        public static RSACryptoServiceProvider GetRSAProviderFromPemFile(String pemfile)
        {
            return GetRSAProviderFromPem(File.ReadAllText(pemfile).Trim());
        }

    }
}
