using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ServiceWeb.Models
{
    public class Writer
    {
        public static StreamWriter writer;

        public static void WriteLine(string line)
        {
            writer = new StreamWriter("C:\\pics\\log.txt", true);
            writer.WriteLine(line);
            writer.Close();
        }
    }
}