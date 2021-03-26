using System;
using System.Collections.Generic;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new DailyLogger(@"C:\Users\Home\logs");
            logger.Info("Запуск программы");
            logger.Info("Обнаружилась ошибка", new Exception("Что-то неверно"));
            logger.Info("Информация с параметрами", 1, 'c', 8.0);
            logger.Debug("Отладочная информация");
            logger.Debug("Отладочная информация", new Exception("Исключение"));
            logger.DebugFormat("Отладочная информация", 1, 'c', 8.0);
            logger.Error("Ошибка");
            logger.Error("Ошибка", new Exception("Выход за границы массива"));
            logger.ErrorUnique("Уникальная ошибка", new Exception("Исключение"));
            logger.ErrorUnique("Уникальная ошибка", new Exception("Исключение"));
            logger.Fatal("Критическая ошибка", new Exception("NullReference"));
            logger.Fatal("Критическая ошибка");
            logger.Warning("Предупреждение");
            logger.Warning("Предупреждение", new Exception("Исключение"));
            logger.WarningUnique("Предупреждение");
            logger.WarningUnique("Предупреждение");
            logger.SystemInfo("Системная информация", new Dictionary<object, object>{
                { 1,"Первое сообщение"},
                { 2,"Второе сообщение"},
                { 3,"Третье сообщение"}
            });
        }
    }
}
