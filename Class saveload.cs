using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2lab
{

    public class SchoolSystem
    {

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public List<Course> Courses { get; set; } = new List<Course>();

        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var student in Students)
                    writer.WriteLine(student.ToString());

                foreach (var teacher in Teachers)
                    writer.WriteLine(teacher.ToString());

                foreach (var course in Courses)
                    writer.WriteLine(course.ToString());
            }
        }

        public void LoadFromFile(string filename)
        {
            var studentFactory = new StudentFactory();
            var teacherFactory = new TeacherFactory();
            var courseFactory = new CourseFactory();

            var tempCourses = new Dictionary<int, List<int>>(); // courseId -> List of student Ids
            var tempTeachers = new Dictionary<int, int>(); // courseId -> teacherId

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');

                        switch (data[0].ToLower())
                        {
                            case "student":
                                if (data.Length >= 3)
                                {
                                    Students.Add((Student)studentFactory.CreateEntity(data));
                                }
                                else
                                {
                                    throw new ArgumentException("Недостаточно данных для создания объекта Student.");
                                }
                                break;

                            case "teacher":
                                if (data.Length >= 4)
                                {
                                    Teachers.Add((Teacher)teacherFactory.CreateEntity(data));
                                }
                                else
                                {
                                    throw new ArgumentException("Недостаточно данных для создания объекта Teacher.");
                                }
                                break;

                            case "course":
                                if (data.Length >= 3)
                                {
                                    var course = (Course)courseFactory.CreateEntity(data);
                                    Courses.Add(course);

                                    // Если есть teacherId
                                    if (data.Length > 3 && int.TryParse(data[3], out int teacherId))
                                    {
                                        tempTeachers[course.Id] = teacherId;
                                    }

                                    // Если есть studentIds
                                    if (data.Length > 4)
                                    {
                                        tempCourses[course.Id] = data[4].Split(',').Select(int.Parse).ToList();
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Недостаточно данных для создания объекта Course.");
                                }
                                break;

                            default:
                                throw new ArgumentException($"Неизвестный тип записи: {data[0]}");
                        }
                    }
                }

                // Восстановление связей
                foreach (var course in Courses)
                {
                    // Установка учителя для курса
                    if (tempTeachers.TryGetValue(course.Id, out int teacherId))
                    {
                        course.Teacher = Teachers.FirstOrDefault(t => t.Id == teacherId);
                        if (course.Teacher != null)
                        {
                            course.Teacher.Courses.Add(course);
                        }
                    }

                    // Добавление студентов в курс
                    if (tempCourses.TryGetValue(course.Id, out List<int> studentIds))
                    {
                        foreach (var studentId in studentIds)
                        {
                            var student = Students.FirstOrDefault(s => s.Id == studentId);
                            if (student != null)
                            {
                                course.Students.Add(student);
                                student.Courses.Add(course);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка данных: {ex.Message}");
            }
        }
    }
}
