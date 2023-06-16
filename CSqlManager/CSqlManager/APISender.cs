using System;
using System.Data;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CSqlManager;

[ApiController]
[Route("api/[controller]")]
public class APISender : ControllerBase
{
    public ExelReader? exelReader { get; set; } = null;
    
    [HttpGet]
    public IActionResult GetInEcu(string? name = null)
    {
        // Create a JSON object or retrieve data to be sent in the response
        var responseObject =  new string[]{ "there is no ExelReader referenced" };;
        if(exelReader is not null)
        {
            if (name is null)
                throw new NoNullAllowedException("You must give the name of what you search");
            
            responseObject = exelReader.SelectInEcu<string>(name).ToArray();
        }
        
        var json = JsonSerializer.Serialize(responseObject);

        // Return the JSON response with 200 OK status code
        return Ok(json);
    }
    
    [HttpGet]
    public IActionResult GetInBrand(string? name = null)
    {
        // Create a JSON object or retrieve data to be sent in the response
        var responseObject =  new string[]{ "there is no ExelReader referenced" };;
        if(exelReader is not null)
        {
            if (name is null)
                throw new NoNullAllowedException("You must give the name of what you search");
            
            responseObject = exelReader.SelectInBrands<string>(name).ToArray();
        }
        
        var json = JsonSerializer.Serialize(responseObject);

        // Return the JSON response with 200 OK status code
        return Ok(json);
    }
    
    public static void StartListener(string responseContent)
    {
        string url = "http://localhost:8080/brand/";

        // Create an HttpListener and start listening
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();

        Console.WriteLine("Listening on " + url);

        // Handle incoming requests
        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerResponse response = context.Response;

            // Create a response with the received content
            string responseString = responseContent;

            // Convert the response string to bytes
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Set the response content type and length
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;

            // Write the response to the output stream
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}

