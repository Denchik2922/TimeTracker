using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TimeTracker.ExtraClass
{
	public class SerializeData : ISerialize
	{
		/// <summary>
		/// Путь для сохранения файлов
		/// </summary>
		private string LocalAppData;

		/// <summary>
		/// Создание нового сериализатора
		/// </summary>
		/// <param name="nameFile">Название файла.</param>
		public SerializeData()
		{
			LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		}

		/// <summary>
		/// Загразка данных.
		/// </summary>
		/// <typeparam name="T">Class.</typeparam>
		/// <returns>List<T>.</returns>
		public T Load<T>(string nameFile) where T : class, new()
		{
			var filePath = Path.Combine(LocalAppData, $"{nameFile}.dat");

			T item;
			try
			{
				FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
				if (fileStream.Length == 0)
				{
					item = new T();
				}
				else
				{
					BinaryFormatter bf = new BinaryFormatter();
					item = (bf.Deserialize(fileStream) as T);

				}
				fileStream.Close();
			}
			catch
			{
				item = new T();
			}
			return item;
		}

		/// <summary>
		/// Сохраниее данных.
		/// </summary>
		/// <typeparam name="T">Class.</typeparam>
		/// <param name="array">Масив даных.</param>
		public void Save<T>(T item, string nameFile) where T : class
		{
			var filePath = Path.Combine(LocalAppData, $"{nameFile}.dat");

			try
			{
				FileStream fileStream = new FileStream(filePath, FileMode.Create);
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fileStream, item);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				throw new ArgumentException($"Ошибка при сохранении данных \n Код ошибки {ex}");
			}
		}

		public void Delete(string nameFile)
		{
			var filePath = Path.Combine(LocalAppData, $"{nameFile}.dat");

			File.Delete(filePath);
		}
	}
}
