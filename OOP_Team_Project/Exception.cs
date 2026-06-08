using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 사용자 정의 예외 1개 이상 만족
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message) { }
    }
}
