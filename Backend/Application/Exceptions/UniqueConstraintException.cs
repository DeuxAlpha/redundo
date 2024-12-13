using System;

namespace Application.Exceptions
{
	public class UniqueConstraintException : Exception
	{
		public object UniqueValue { get; set; }
		
		public UniqueConstraintException(string objectName, object uniqueValue) 
			: base($"{objectName} with value '{uniqueValue}' already exists.")
		{
			UniqueValue = uniqueValue;
		}

		public UniqueConstraintException(string objectName, Exception exception)
			: base($"{objectName} already exists.", exception)
		{
		}
	}
}