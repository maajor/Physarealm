using System;
using System.Reflection;
using System.Drawing;
using Grasshopper.Kernel;

namespace Physarealm
{
    public class PhysarealmInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Physarealm";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("480c3228-e84d-4696-9a2d-c654f63b4cbb");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Ma Yidong @ THU.SA";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "hello_myd@126.com";
            }
        }
        public override string Version
        {
            get
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Version version = executingAssembly.GetName().Version;
                return version.ToString();
            }
        }
    }
}
