using System.Reflection;

namespace Application
{
    public class ProgramInfo
    {
        public static Assembly GetAssembly()
        {
            return typeof(ProgramInfo).GetTypeInfo().Assembly;
        }
    }
}