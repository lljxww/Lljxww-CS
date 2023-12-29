using System;

namespace Lljxww.ApiCaller.Models.Exceptions;

public class CallerException : Exception
{
    public CallerException(string message) : base(message)
    {
    }
}