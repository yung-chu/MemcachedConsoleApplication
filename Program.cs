using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTISP.ACL.Cache.Memcached
{
    class Program
    {

        static void Main(string[] args)
        {

            string key = "hehe";

            string ss = "i love u yungchu";


            List<Person> list = new List<Person>{
             new  Person{ Id=1, Name="滕景怡"},
               new  Person{ Id=2, Name="滕景怡"}
            };



            MemcachedITS.Cached.CacheSet(key, list);



            var obj = MemcachedITS.Cached.CacheGet(key);

            if (obj != null)
            {
                foreach (var item in (List<Person>)obj)
                {
                     Console.WriteLine(item.Id+" "+item.Name);
                }
            }


       
        }
    }

	   [Serializable]  
	   public class Person  
	   {  
	       private int id;  
	  
	       public int Id  
	       {  
	           get { return id; }  
	           set { id = value; }  
	       }  
	       private string name;  
	  
	       public string Name  
	       {  
	           get { return name; }  
	           set { name = value; }  
	       }  
         
	        /// <summary>  
            /// 重写Tostring()，方便输出验证  
           /// </summary>  
	       /// <returns></returns>  
	       public override string ToString()  
	       {  
	           return "Person:" + "{name:" + Name + ",id:" + Id + "}";  
           }  
     }  

}
