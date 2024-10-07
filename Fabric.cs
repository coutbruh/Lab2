using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2lab
{
    // Класс Student
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public Student(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"student {Id} {Name} {string.Join(",", Courses.Select(c => c.Id))}";
        }
    }

    // Класс Teacher
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public Teacher(int id, string name, int experience)
        {
            Id = id;
            Name = name;
            Experience = experience;
        }

        public override string ToString()
        {
            return $"teacher {Id} {Name} {Experience} {string.Join(",", Courses.Select(c => c.Id))}";
        }
    }

    // Класс Course
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();

        public Course(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"course {Id} {Name} {Teacher?.Id} {string.Join(",", Students.Select(s => s.Id))}";
        }
    }


    public abstract class EntityFactory
    {
        public abstract object CreateEntity(string[] data);
    }

    public class StudentFactory : EntityFactory
    {
        public override object CreateEntity(string[] data)
        {
            int id = int.Parse(data[1]);
            string name = data[2];
            return new Student(id, name);
        }
    }

    public class TeacherFactory : EntityFactory
    {
        public override object CreateEntity(string[] data)
        {
            if (data.Length < 4)
            {
                throw new ArgumentException($"Недостаточно данных для создания объекта Teacher. Ожидается 4 элемента, получено: {data.Length}");
            }

            if (int.TryParse(data[1], out int id) && int.TryParse(data[3], out int experience))
            {
                string name = data[2];
                return new Teacher(id, name, experience);
            }
            else
            {
                throw new ArgumentException($"Некорректный формат данных для создания объекта Teacher. Ожидается числовое значение для Id и Experience. Входные данные: Id = {data[1]}, Name = {data[2]}, Experience = {data[3]}");
            }
        }
    }


    public class CourseFactory : EntityFactory
    {
        public override object CreateEntity(string[] data)
        {
            int id = int.Parse(data[1]);
            string name = data[2];
            return new Course(id, name);
        }
    }

}
