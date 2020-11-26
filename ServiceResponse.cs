using System;

public class ServiceResponse<T> 
{
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public bool Success { get; set; } = false;
    public string Message { get; set; }
    public T Data { get; set; }
}
