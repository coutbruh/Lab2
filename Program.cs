namespace _2lab
{
    public class Program
    {
        string filename = @"C:\toreeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeent\date.txt";
        public static void Main(string[] args)
        {
            SchoolSystem system = new SchoolSystem();

            // Создание объектов
            var teacher1 = new Teacher(1, "Mr. Smith", 10);
            var student1 = new Student(1, "Alice");
            var student2 = new Student(2, "Bob");
            var course1 = new Course(1, "Mathematics");
            course1.Teacher = teacher1;
            course1.Students.Add(student1);
            course1.Students.Add(student2);
            teacher1.Courses.Add(course1);
            student1.Courses.Add(course1);
            student2.Courses.Add(course1);

            system.Teachers.Add(teacher1);
            system.Students.Add(student1);
            system.Students.Add(student2);
            system.Courses.Add(course1);

            // Сохранение в файл
            system.SaveToFile(@"C:\toreeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeent\date.txt");

            // Загрузка из файла
            SchoolSystem loadedSystem = new SchoolSystem();
            loadedSystem.LoadFromFile("data.txt");

            // Проверка загруженных данных
            foreach (var course in loadedSystem.Courses)
            {
                Console.WriteLine(course);
            }
        }
    }

}
