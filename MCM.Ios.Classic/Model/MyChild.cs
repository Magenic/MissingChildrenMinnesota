using System;

namespace MCM.Ios.Classic
{
	public class MyChild
	{
		public MyChild (int age, string name)
		{
			this.Age = age;
			this.Name = name;
		}

		public int Age { get; private set;}
		public string Name { get; private set;}
		public int ChecklistPercent {get;set;}
	}
}

