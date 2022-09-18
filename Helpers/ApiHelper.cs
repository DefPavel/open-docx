﻿using System.Net;
namespace open_docx.Helpers;

public static class ApiHelper
{
    public static async Task<bool> JsonPostWithToken(string token, string queryUrl, string httpMethod, string reportName)
    {
        #pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(queryUrl);    
        #pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                  
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();

        return FileHelper.SaveReport(responseStream, reportName);

    }
}

