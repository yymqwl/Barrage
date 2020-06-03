
using System;
using System.Runtime.InteropServices.ComTypes;

namespace GameFramework
{
    public static class IdStringGenerater
    {

        public static string GenerateIdString(int len)
        {
            var idguild= Guid.NewGuid().ToString("N");
            return idguild.Substring(0, len);
        }

    }
}