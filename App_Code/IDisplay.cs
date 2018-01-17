using System;
using System.Web;

    public interface IDisplay : IHttpHandler
    {
      string Year { get;  set; }
      string Month { get; set; }
      string Day { get; set; }
      string Topic { get; set; }
    }
