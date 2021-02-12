using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.ExtraClass
{
	interface ISerialize
	{
		void Save<T>(T item, string nameFile) where T : class;

		T Load<T>(string nameFile) where T : class, new();

		void Delete(string path);
	}
}
