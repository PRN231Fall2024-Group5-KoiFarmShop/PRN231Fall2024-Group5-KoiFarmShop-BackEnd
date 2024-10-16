﻿using System.Reflection;

namespace Koi.Repositories.Utils
{
    public static class ResourceHelper
    {
        public static string ReadResource(string relativePath, Assembly fromAssembly)
        {
            var assembly = fromAssembly;
            if ((object)assembly == null)
                assembly = typeof(ResourceHelper).Assembly;
            var str = relativePath.Replace('/', '.').Replace('\\', '.');

            using (var manifestResourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + "." + str))
            {
                if (manifestResourceStream == null)
                    throw new IOException("Failed to read manifest resource.");
                using (var streamReader = new StreamReader(manifestResourceStream))
                    return streamReader.ReadToEnd();
            }
        }

        public static string ReadJsonResource(
            string relativePath,
            Assembly fromAssembly,
            bool stripWhitespace = false)
        {
            return !stripWhitespace ? ReadResource(relativePath, fromAssembly) : ReadResource(relativePath, fromAssembly).StripJsonWhitespace();
        }

        public static int DateTimeValidate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new Exception("Invalid date time input: EndDate cannot be earlier than StartDate.");
            }
            TimeSpan duration = endDate - startDate;

            // Trả về số ngày dưới dạng số nguyên
            return duration.Days;
        }
    }
}