using System.Runtime.Intrinsics.Arm;

namespace AspNetCoreIdentityApp.Web.Services
{
    public static class CapchaGenerator
    {
        public static string CapchaCreate()
        {
            string CapchaText;
            Random rdm = new Random();
            string k1;
            string[] msv1 = { "A", "B", "C", "D", "E", "a", "r", "f", "y", "m" };
            int s1 = rdm.Next(0, msv1.Length);
            k1 = (msv1[s1]);

            string k2;
            string[] msv2 = { "1", "3", "5", "7", "9", "11", "13", "15", "17", "18" };
            int s2 = rdm.Next(0, msv2.Length);
            k2 = (msv2[s2]);

            string k3;
            string[] msv3 = { "a", "r", "m", "u", "d" };
            int s3 = rdm.Next(0, msv3.Length);
            k3 = (msv3[s3]);

            string k4;
            string[] msv4 = { "*", "0", "#", "@", ">", "!", "<", "+", "=", "/" };
            int s4 = rdm.Next(0, msv4.Length);
            k4 = (msv4[s4]);

            string k5;
            string[] msv5 = { "2", "4", "6", "8", "0" };
            int s5 = rdm.Next(0, msv5.Length);
            k5 = (msv5[s5]);


           return  CapchaText = k1 + k2 + k3 + k4 + k5;
        }
    }
}
